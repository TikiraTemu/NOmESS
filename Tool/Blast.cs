using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Tool
{
    public class Blast{
        
        public int Counter = 0;

        //returns dict: fasta header scaffold (key) ---> all statistical values & id of input seq (value)
        public Dictionary <string ,List<SequencePiece>> BlastOutputData (string blastPath, string inputSetPath, string dictionaryFilePath, string notTakenSeqsPath, double percentage_ident_before, double e_value, double bit_score, int query_length, int ali_length, string seqsWithoutScaffPath, Boolean step2 ){
            
            Dictionary <string, List<SequencePiece>> blast = new Dictionary<string, List<SequencePiece>>();
            Dictionary<string, string> inputSet = Database.GetDictionary(inputSetPath);
            string tempLine = "";
            double percentage_ident = percentage_ident_before*100;

            //dictionary of input sequence set to gain all seqs without scaffold
            Dictionary<string, string> swsDict = Database.GetDictionary(inputSetPath);

            //read in blast output
            StreamReader inFile = new StreamReader(blastPath);

            //prints not taken sequences from blast output
            StreamWriter notTaken = new StreamWriter(notTakenSeqsPath);

            while ((tempLine=inFile.ReadLine())!= null){

                //remove newline & splitting the tab separated line & packing the items in an array 
                tempLine = tempLine.Replace("\n", "");
                object[] line = tempLine.Split('\t');

                //save the scaffold sequence for each line in variable
                string keyScaffold = ">"+line[1];

                //save the values of the hit in variables
                string idInput = ">"+line[0];

                //only continue if the dictionary contains the specific input ID
                if (inputSet.ContainsKey(idInput)){

                    int queryLength = Convert.ToInt32(line[2]);
                    int subjectLength = Convert.ToInt32(line[3]);
                    int alignmentLength = Convert.ToInt32(line[4]);
                    int numberIdentity = Convert.ToInt32(line[5]);
                    double percentageIdentity = Convert.ToDouble(line[6]);
                    int misMatch = Convert.ToInt32(line[7]);
                    int gaps = Convert.ToInt32(line[8]);
                    int queryStart = Convert.ToInt32(line[9]);
                    int queryEnd = Convert.ToInt32(line[10]);
                    int subjectStart = Convert.ToInt32(line[11]);
                    int subjectEnd = Convert.ToInt32(line[12]);
                    double bitScore = Convert.ToDouble(line[13]);
                    double eValue = Convert.ToDouble(line[14]);
                    string sequence = inputSet[idInput];
                
                    SequencePiece newInputPiece = new SequencePiece(idInput, queryLength, subjectLength, alignmentLength, numberIdentity,
                                               percentageIdentity, misMatch, gaps, queryStart, queryEnd, subjectStart,
                                               subjectEnd, bitScore, eValue,sequence);
                                                             

                    SequencePiece tempPiece = new SequencePiece();

                    //delete input sequences with scaffold
                    if (swsDict.ContainsKey(idInput))
                    {
                            swsDict.Remove(idInput);
                    }

                    //filtering which sequence pieces are taken
                    if ((( percentageIdentity > percentage_ident ) && ( eValue < e_value ) && ( gaps < alignmentLength / 2 ) && ( bitScore > bit_score ) && ( queryLength > query_length ) && ( alignmentLength > ali_length ))) {
                 
                        //checking if the dictionary already contains the scaffold sequence (key)
                        if (blast.ContainsKey(keyScaffold)){
                            List<SequencePiece> list = blast[keyScaffold];
                            Boolean inside = false;
                            int scoreTemp = 0;
                            int scoreNew = 0;
                        
                            //check if the list already contains piece with the specific ID
                            for (int i = 0; i < list.Count(); i++){
                                if (list[i].GetId()== idInput){
                                    inside = true;
                                    tempPiece = list[i];
                                }//end if  
                            }//end for, going through list 
                        
                            //check which SequencePiece is better
                            //asumption tempPiece is better than newInputPiece
                            if (inside){
                                if (tempPiece.GetEValue() < eValue)                         {scoreTemp++;}else{scoreNew++;}
                                if (tempPiece.GetAlignmentLength() > alignmentLength)       {scoreTemp++;}else{scoreNew++;}
                                if (tempPiece.GetQueryLength() > queryLength)               {scoreTemp++;}else{scoreNew++;}
                                if (tempPiece.GetBitScore() > bitScore)                     {scoreTemp++;}else{scoreNew++;}
                                if (tempPiece.GetPercentageIdentity() > percentageIdentity) {scoreTemp++;}else{scoreNew++;}

                                //random choice if scores are equal
                                if (scoreTemp==scoreNew){
                                    Random rnd = new Random();
                                    int Rnd1 = rnd.Next(1, 3);

                                    if (Rnd1 == 1) { list.Add(tempPiece); }
                                    else { list.Add(newInputPiece); }
                                
                                }//end if identical score

                                if (scoreTemp>scoreNew){list.Add(tempPiece);}
                                else{list.Add(newInputPiece);}

                            }//end inside

                            else{list.Add(newInputPiece);}

                            blast.Remove(keyScaffold);
                            blast.Add(keyScaffold,list);
                        
                        }//end if contains scaffold id -> yes

                        //if it doesn't contain it create new list and put everything in the dictionary
                        else{
                            List <SequencePiece> list = new List<SequencePiece>();
                            list.Add(newInputPiece);
                            blast.Add(keyScaffold,list);
                        }//end else contains -> no

                    }//end if filtering

                    //print the not taken sequences from the Blast Output
                    else
                    {
                            notTaken.WriteLine(idInput);
                            notTaken.WriteLine(sequence);
                    }
                
                }//end if dictionary contains input ID
            }//end while

            inFile.Close();
            notTaken.Close();

           //printing dictionary of the assignment to the scaffold sequences for each piece
            StreamWriter dictFile = new StreamWriter(dictionaryFilePath);
            
            foreach (string s in blast.Keys){
                               
                dictFile.Write(s+"\t");

                foreach (SequencePiece seq in blast[s]){
                    dictFile.Write(seq.GetId()+"\t");
                } 

                dictFile.Write('\n');
            }
            dictFile.Close();

            //prints sequences without scaffold
            if (step2)
            {
                StreamWriter swsWriter = new StreamWriter(seqsWithoutScaffPath);
                foreach (string inputSeq in swsDict.Keys)
                {
                    swsWriter.WriteLine(inputSeq);
                    swsWriter.WriteLine(swsDict[inputSeq]);
                }
                swsWriter.Close();
            }
            return blast;

        }//end BlastOutputData


        //---------------------------------------------------------------------------------------------------------
        
        //prints new assembled sequences (step 2)        
        public void CreateNonRedundantDatabase_step2(string blastOutputPath, string scaffoldSetPath, string inputSetPath, string outputPath, string idPath, string sortedListPath, string restPath, string dictionaryPath, string sortedDictionaryFilesPath, string notTakenSeqsPath, double percentage_ident, double e_value, double bit_score, int query_length, int ali_length, string newIDstart, string seqsWithoutScaffoldPath, Boolean printSWS) {

            //creating all dictionaries which are needed for mapping
            Dictionary<string, List<SequencePiece>> blastOut = new Blast().BlastOutputData(blastOutputPath, inputSetPath, dictionaryPath,  notTakenSeqsPath, percentage_ident, e_value, bit_score, query_length, ali_length, seqsWithoutScaffoldPath, printSWS);
           
            //creating an own ID
            int ownIdNumber = 0;
            string middle = "";
            string fileName;
            string beginningOfNewId = newIDstart;
            List<string> temporarySeqList;

            //prints new combined sequences
            StreamWriter fastaFile = new StreamWriter(outputPath);
            //prints sequence ids: new ID, scaffold ID, corresponding input set IDs
            StreamWriter idFile = new StreamWriter(idPath);
            //prints sorted list depending on the subject start
            StreamWriter sortedLists = new StreamWriter(sortedListPath);
            //prints the sequences that are not used for making a long one
            StreamWriter rest = new StreamWriter(restPath);


            //going through all the entries from the blast output
            List<string> keysScaffold = new List<string>(blastOut.Keys);

            foreach (string keyScaffold in keysScaffold) {

                temporarySeqList = new List<string>();
                Boolean done = false;
                List<SequencePiece> notYetProcessedSequences = new List<SequencePiece>();

                List<SequencePiece> inputSequenceList = blastOut[keyScaffold];
                
                //sorting list with input pieces by subjectStart low to high, queryLength low to high, bitScore high to low, eValue low to high
                SequencePieceComparerStep2 comparerSubjStart = new SequencePieceComparerStep2();
                inputSequenceList.Sort(comparerSubjStart);  
                
                sortedLists.WriteLine(keyScaffold);
                foreach (SequencePiece s in inputSequenceList) { sortedLists.Write(s.GetId() + "\t"); }
                sortedLists.Write("\n");
                foreach (SequencePiece p in inputSequenceList) { sortedLists.Write(p.GetSubjectStart() + "\t\t"); }
                sortedLists.Write("\n");
                foreach (SequencePiece m in inputSequenceList) { sortedLists.Write(m.GetQueryLength() + "\t\t"); }
                sortedLists.Write("\n");
                foreach (SequencePiece n in inputSequenceList) { sortedLists.Write(n.GetBitScore() + "\t\t"); }
                sortedLists.Write("\n");
                foreach (SequencePiece o in inputSequenceList) { sortedLists.Write(o.GetEValue() + "\t"); }
                sortedLists.Write("\n\n");

                fileName = keyScaffold.Substring(1, keyScaffold.Length - 1);

                StreamWriter temp = new StreamWriter(sortedDictionaryFilesPath + fileName + ".fasta");
                foreach (SequencePiece seq in inputSequenceList) {
                    temp.WriteLine(seq.GetId());
                    temp.WriteLine(seq.GetSequence());
                }
                temp.Close();
                
                while(!done){

                    temporarySeqList = new List<string>();
                    //creating the ID for the output and printing it
                    middle = "";
                    for (int i = 0; i < 6 - ownIdNumber.ToString().Count(); i++){middle += "0";}//end for
                    fastaFile.WriteLine(beginningOfNewId + middle + ownIdNumber.ToString());
                    idFile.Write("\n" + beginningOfNewId + middle + ownIdNumber.ToString());
                    rest.Write("\n" + beginningOfNewId + middle + ownIdNumber.ToString());
                    ownIdNumber++;
                    idFile.Write("\t" + keyScaffold);
                    rest.Write("\t" + keyScaffold + "\n");
                 
                    //checks if entry has only one sequence piece --> printing the ID in idFile and the sequence in fastaFile (result)
                    if (inputSequenceList.Count < 2) {
                        idFile.Write("\t" + inputSequenceList [0].GetId());
                        fastaFile.WriteLine(inputSequenceList [0].GetSequence());
                    }//end if list has 1 item


                     //entry has more than one input sequence
                     else{
                        temporarySeqList = Put2SequencesTogether_step2(inputSequenceList [0].GetSequence(),
                                                         inputSequenceList [1].GetSequence(),rest);
                    
                        //sequence to go on putting together with next piece
                        string continueSequence = "";

                            //check -> two pieces didn't fit together
                           if (temporarySeqList.Count() == 2){

                                //printing first sequence 
                                idFile.Write("\t" + inputSequenceList [0].GetId());
                                continueSequence = temporarySeqList[0];
                                notYetProcessedSequences.Add(inputSequenceList [1]);
                            }//end if

                            //two sequences fit together
                            else{
                                idFile.Write("\t" + inputSequenceList [0].GetId() + "\t" + inputSequenceList [1].GetId());
                                continueSequence = temporarySeqList[0];
                           }//end else

                            //getting the rest of the sequencelist together
                            for (int i = 2; i < inputSequenceList.Count(); i++) {
                                temporarySeqList = new List<string>();
                                temporarySeqList = Put2SequencesTogether_step2(continueSequence, inputSequenceList [i].GetSequence(), rest);

                                //check -> two pieces didn't fit together
                                if (temporarySeqList.Count() == 2){
                                    continueSequence = temporarySeqList[0];
                                    notYetProcessedSequences.Add(inputSequenceList [i]);
                                }//end if

                                //2 sequences fit together
                                else{
                                    idFile.Write("\t" + inputSequenceList [i].GetId());
                                    continueSequence = temporarySeqList[0];
                                }//end else  
                                              
                            }//end for loop rest of SequencePieceList
                    
                            //check which is the missing Sequence
                            fastaFile.WriteLine(continueSequence);

                        }//end else more than one input sequence

                    //checking if there are still input sequence pieces to process
                    if (notYetProcessedSequences.Count == 0) { done = true; }
                    else { 
                        done = false;
                        inputSequenceList = new List<SequencePiece>();
                        inputSequenceList = notYetProcessedSequences;
                        notYetProcessedSequences = new List<SequencePiece>();
                    }
                }//end while
            }//end foreach key

            fastaFile.Close();
            idFile.Close();
            sortedLists.Close();
            rest.Close();

        }//end MakeWholeSequences
             
        
        public List<string> Put2SequencesTogether_step2 (string sequence1, string sequence2, StreamWriter rest){
            
            List<string> resultSequences = new List<string>();
            List<string> pattern = new List<string>();
            
            int numberOfMismatches = 0;
            int toleratedNumberOfMismatches = 0;
            double percentage=0.03;
            int tempStartPosition2=-1;
            int startPosition2=0;
            int endPosition2;
            int minimalOverlappingAminoAcidLength = 10;
            string tempPattern = "";

            Boolean found = false;
            Boolean haveOneMotiv = false;

            //picking last 10 aa
            string end1 = sequence1.Substring(sequence1.Count() - minimalOverlappingAminoAcidLength , minimalOverlappingAminoAcidLength);
            pattern.Add(end1);

            for (int i = 0; i < minimalOverlappingAminoAcidLength; i++) {
                for (int j = 0; j < minimalOverlappingAminoAcidLength; j++) {
                    if(i == j){
                        tempPattern += ".";
                    }
                    else{
                        tempPattern += end1 [j];
                    }
                }
                pattern.Add(tempPattern);
                tempPattern = "";
            }

            foreach (string motif in pattern) {
                
                Regex conservedSeq = new Regex(motif);            
                MatchCollection isMatch = conservedSeq.Matches(sequence2);

                //check if motif is in piece2 
                for (int i = 0 ; i < isMatch.Count ; i++){
                    tempStartPosition2=isMatch[i].Index;
                    if(tempStartPosition2 != -1){
                        found = true;
                        haveOneMotiv = true;
                        break;
                    }
                }//end for loop for a match

                endPosition2 = tempStartPosition2 + minimalOverlappingAminoAcidLength - 1;
                toleratedNumberOfMismatches = (int)(( endPosition2 + 1 ) * percentage);
                
                //check if there is more overlapping sequence                   
                if(found){
                    for (int i = 1; i < sequence1.Length - minimalOverlappingAminoAcidLength + 1; i++){

                        //check if the sequence2 has still a position in his sequence, not --> break!
                        if (tempStartPosition2 - i < 0){break;}

                        //check if the overlapping is larger
                        if (sequence1[sequence1.Length - minimalOverlappingAminoAcidLength - i] != sequence2[tempStartPosition2 - i]){
                            numberOfMismatches++;
                        }
                    
                        startPosition2 = tempStartPosition2 - i;

                        //check if there are more than the tolerated number of mismatches --> break!
                        if (numberOfMismatches > toleratedNumberOfMismatches){
                            break;
                        } //end if mismatches
                        
                      } //end for loop rest of Sequence

                     if (startPosition2 == 0){
                         string seq = sequence1 + sequence2.Substring(endPosition2 + 1, sequence2.Length - (endPosition2 + 1));
                         resultSequences.Add(seq);
                         rest.WriteLine(sequence2.Substring(0, endPosition2 + 1));
                         found = false;
                     }
                     else{
                         resultSequences.Add(sequence1);
                         resultSequences.Add(sequence2);
                         found = false;
                     }
                }//end if found

                if (haveOneMotiv) { break; }

            }//end foreach motif

            if(!haveOneMotiv){
 
                resultSequences.Add(sequence1);
                resultSequences.Add(sequence2);
            }

            return resultSequences;

        }//end Put2SequencesTogether
        

        //---------------------------------------------------------------------------------------------------------
        

        //prints the concatenated non-overlapping sequences (step 3)
        public void CreateNonRedundantDatabase_step3(string blastOutputPath, string inputSetPath, string outputPath, string idPath, string sortedListStep2Path, string sortedDictionaryFilesPath, string dictionaryPath, string notTakenSeqsPath, double percentage_ident, double e_value, double bit_score, int query_length, int ali_length, string newIDstart) {

            //creating all the dictionaries which are needed for the mapping
            Dictionary<string, List<SequencePiece>> blastOut = new Blast().BlastOutputData(blastOutputPath, inputSetPath, dictionaryPath, notTakenSeqsPath,percentage_ident, e_value, bit_score, query_length, ali_length, "", false);

            //creating an own ID
            int ownIdNumber = 0;
            string middle = "";
            string fileName;
            string beginningOfNewId = newIDstart;

            //prints new combined sequences
            StreamWriter fastaFile = new StreamWriter(outputPath);
            //prints sequence ids: new ID, scaffold ID, corresponding input IDs
            StreamWriter idFile = new StreamWriter(idPath);
            //prints the sorted list of step 2
            StreamWriter sortedLists = new StreamWriter(sortedListStep2Path);
 
            //going through all the entries from the blast output
            List<string> keysScaffold = new List<string>(blastOut.Keys);

            foreach (string keyScaffold in keysScaffold) {

                SequencePiece continueSequence = new SequencePiece();
                List<SequencePiece> temporarySeqList = new List<SequencePiece>();
                List<SequencePiece> InputSequenceList = blastOut [keyScaffold];

                //sorting list with input pieces
                SequencePieceComparerStep3 comparerSubjStart = new SequencePieceComparerStep3();
                InputSequenceList.Sort(comparerSubjStart);

                sortedLists.WriteLine(keyScaffold);
                foreach (SequencePiece s in InputSequenceList) { sortedLists.Write(s.GetId() + "\t"); }
                sortedLists.Write("\n");
                foreach (SequencePiece p in InputSequenceList) { sortedLists.Write(p.GetSubjectStart() + "\t\t"); }
                sortedLists.Write("\n");
                foreach (SequencePiece m in InputSequenceList) { sortedLists.Write(m.GetSubjectEnd() + "\t\t"); }
                sortedLists.Write("\n");
                foreach (SequencePiece n in InputSequenceList) { sortedLists.Write(n.GetQueryLength() + "\t\t"); }
                sortedLists.Write("\n");
                foreach (SequencePiece o in InputSequenceList) { sortedLists.Write(o.GetBitScore() + "\t\t"); }
                sortedLists.Write("\n");
                foreach (SequencePiece p in InputSequenceList) { sortedLists.Write(p.GetEValue() + "\t"); }
                sortedLists.Write("\n\n");

                fileName = keyScaffold.Substring(1, keyScaffold.Length - 1);
                StreamWriter temp = new StreamWriter(sortedDictionaryFilesPath + fileName + ".fasta");
                foreach (SequencePiece seq in InputSequenceList) {
                    temp.WriteLine(seq.GetId());
                    temp.WriteLine(seq.GetSequence());
                }
                temp.Close();

                if(InputSequenceList.Count < 2){

                    //creating the ID for the outputStep2 and printing it
                    middle = "";
                    for (int i = 0; i < 6 - ownIdNumber.ToString().Count(); i++) { middle += "0"; }//end for
                    fastaFile.WriteLine(beginningOfNewId + middle + ownIdNumber.ToString());
                    idFile.Write("\n" + beginningOfNewId + middle + ownIdNumber.ToString());

                    ownIdNumber++;
                    idFile.Write("\t" + keyScaffold);

                    idFile.Write("\t" + InputSequenceList [0].GetId());
                    fastaFile.WriteLine(InputSequenceList [0].GetSequence());
                                        
                    }//end if list has 1 item
                
                else{
                    //take the longest possible sequence foreach "group"
                    temporarySeqList = Put2SequencesTogether_step3(InputSequenceList [0], InputSequenceList [1]);

                    //check -> two pieces didn't fit together
                    if (temporarySeqList.Count() == 2) {
                        
                        //creating new ID for the output and printing it
                        middle = "";
                        for (int i = 0; i < 6 - ownIdNumber.ToString().Count(); i++) { middle += "0"; }//end for
                        fastaFile.WriteLine(beginningOfNewId + middle + ownIdNumber.ToString());
                        idFile.Write("\n" + beginningOfNewId + middle + ownIdNumber.ToString());
                        idFile.Write("\t" + keyScaffold);
                        
                        //printing second sequence
                        fastaFile.WriteLine(temporarySeqList [1].GetSequence());
                        idFile.Write("\t" + temporarySeqList [1].GetId());
                        continueSequence = temporarySeqList [0];
                        ownIdNumber++;

                    }//end if

                     //two sequences fit together
                    else {
                        continueSequence = temporarySeqList [0];
                    }//end else

                    //getting the rest of the sequencelist together
                    for (int k = 2; k < InputSequenceList.Count(); k++) {

                        temporarySeqList = Put2SequencesTogether_step3(continueSequence, InputSequenceList [k]);

                        //check -> two pieces didn't fit together
                        if (temporarySeqList.Count() == 2) {

                            //creating new ID for the output and printing it
                            middle = "";
                            for (int i = 0; i < 6 - ownIdNumber.ToString().Count(); i++) { middle += "0"; }//end for
                            fastaFile.WriteLine(beginningOfNewId + middle + ownIdNumber.ToString());
                            idFile.Write("\n" + beginningOfNewId + middle + ownIdNumber.ToString());
                            
                            idFile.Write("\t" + keyScaffold);
                            
                            //printing second sequence
                            fastaFile.WriteLine(temporarySeqList [1].GetSequence());
                            idFile.Write("\t" + temporarySeqList [1].GetId());

                            continueSequence = temporarySeqList [0];                            
                            ownIdNumber++;

                        }//end if

                        //2 sequences fit together
                        else {
                            continueSequence = temporarySeqList [0];
                        }//end else  

                    }//end for loop rest of SequencePieceList

                    //creating new ID for the output and printing it
                    middle = "";
                    for (int i = 0; i < 6 - ownIdNumber.ToString().Count(); i++) { middle += "0"; }//end for
                    fastaFile.WriteLine(beginningOfNewId + middle + ownIdNumber.ToString());
                    idFile.Write("\n" + beginningOfNewId + middle + ownIdNumber.ToString());                    
                    idFile.Write("\t" + keyScaffold);
 
                    //printing sequence consisting of non-overlapping contigs
                    idFile.Write("\t" + continueSequence.GetId());
                    fastaFile.WriteLine(continueSequence.GetSequence());

                    ownIdNumber++;
                    
               }//end else more than one input sequence

            }//end foreach key

            fastaFile.Close();
            idFile.Close();
            sortedLists.Close();

        }//end MakeWholeSequencesStep2
                
        public List<SequencePiece> Put2SequencesTogether_step3 (SequencePiece sequence1, SequencePiece sequence2){

            List<SequencePiece> resultSequences = new List<SequencePiece>();
            int newQueryLength;

            //overlapping sequences
            if (sequence2.GetSubjectStart() <= sequence1.GetSubjectEnd()) {
                resultSequences.Add(sequence1);
                resultSequences.Add(sequence2);                
            }

            //non-overlapping sequences
            else{
                SequencePiece temp = new SequencePiece();
                temp.SetSequence(sequence1.GetSequence() + "X" +  sequence2.GetSequence());
                temp.SetSubjectStart(sequence2.GetSubjectStart());
                temp.SetSubjectEnd(sequence2.GetSubjectEnd());
                temp.SetId(sequence1.GetId() /*+ "\t" + "NOC"*/ + "\t" + sequence2.GetId());
                newQueryLength = sequence1.GetQueryLength() + sequence2.GetQueryLength();
                temp.SetQueryLength(newQueryLength);
                resultSequences.Add(temp);
            }
            
            return resultSequences;

        }//end Put2SequencesTogetherStep2
        

        //---------------------------------------------------------------------------------------------------------


        //prints longest representant for each scaffold sequence (step 4)
        public void CreateNonRedundantDatabase_step4(string blastOutputPath, string InputSetPath, string idPath, string sortedListPath, string longestSequencesPath, string shorterSequencesPath, string dictionaryPath, string notTakenSeqsPath, double percentage_ident, double e_value, double bit_score, int query_length, int ali_length, string newIDstart) {

            //creating all dictionaries needed for the mapping
            Dictionary<string, List<SequencePiece>> blastOut = new Blast().BlastOutputData(blastOutputPath, InputSetPath, dictionaryPath, notTakenSeqsPath, percentage_ident, e_value, bit_score, query_length, ali_length, "", false);
                 
            
            //prints the sequence ids: new ID, scaffold ID, corresponding IDs
            StreamWriter idFile = new StreamWriter(idPath);
            //prints sorted list of step 3
            StreamWriter sortedForStep3 = new StreamWriter(sortedListPath);
            //prints longest representants for each scaffold
            StreamWriter longestSequences = new StreamWriter(longestSequencesPath);
            //prints only shorter representants for each scaffold
            StreamWriter shorterSequences = new StreamWriter(shorterSequencesPath);
            
            //going through all the entries from the blast output
            List<string> keysScaffold = new List<string>(blastOut.Keys);

            //creating an own ID
            int ownIdNumber = 0;
            string middle = "";
            string beginningOfNewId = newIDstart;
            string newID;

            //go through all keys
            foreach (string keyScaffold in keysScaffold) {

                List<SequencePiece> toSeparate = blastOut [keyScaffold];
                
                //sort list according to the length of the sequences
                SequencePieceComparerStep4 comparerQueryLength = new SequencePieceComparerStep4();
                toSeparate.Sort(comparerQueryLength);

                sortedForStep3.WriteLine(keyScaffold);
                foreach (SequencePiece s in toSeparate) { sortedForStep3.Write(s.GetId() + "\t"); }
                sortedForStep3.Write("\n");
                foreach (SequencePiece s in toSeparate) { sortedForStep3.Write(s.GetSequence().Length + "\t\t\t"); }
                sortedForStep3.Write("\n");
                foreach (SequencePiece s in toSeparate) { sortedForStep3.Write(s.GetBitScore() + "\t\t\t"); }
                sortedForStep3.Write("\n");
                foreach (SequencePiece s in toSeparate) { sortedForStep3.Write(s.GetEValue() + "\t\t"); }
                sortedForStep3.Write("\n");
                sortedForStep3.Write("\n");
                
                //creating ID for the outputStep and printing it
                middle = "";
                for (int i = 0; i < 6 - ownIdNumber.ToString().Count(); i++) { middle += "0"; }//end for
                newID = beginningOfNewId + middle + ownIdNumber.ToString();
                idFile.WriteLine(newID + "\t" + keyScaffold + "\t" + toSeparate[0].GetId());
                ownIdNumber++;

                longestSequences.WriteLine(newID);
                longestSequences.WriteLine(toSeparate [0].GetSequence());
                
                //printing shorter sequences
                for (int s = 1; s < toSeparate.Count; s++) {
                    shorterSequences.WriteLine(toSeparate [s].GetId());
                    shorterSequences.WriteLine(toSeparate [s].GetSequence());
                }
                
            }//end foreach key

            idFile.Close();
            longestSequences.Close();
            shorterSequences.Close();
            sortedForStep3.Close();

        }//end MakeNewSequencesStep3
          
    }//end class
}
