using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Tool
{
    public class Database{

     //returns a dictionary fasta header (key) ---> sequence (value)
     public static Dictionary<string, string> GetDictionary(string path){
    
        Dictionary<string,string> sequences = new Dictionary<string, string>();
        string line = "";
        string header = "";
        string sequence = "";
        int dictionaryCounter = 0;
            
        /*read in file line by line and save in dictionary <Fasta Header, Sequence> */
        StreamReader file = new StreamReader(path);
              
        while ((line = file.ReadLine()) != null){

            line = line.Replace("\n", "");
            Regex fastaHeader = new Regex("^>.*");
            Match mHeader = fastaHeader.Match(line);

            //check if line is a fasta header
            if (mHeader.Success){
                
                //check if fasta header is the first line if not
                if(dictionaryCounter>0){
                   
                    AddToDictionary(sequences,header,sequence);
                    sequence = "";
                   
                }//end if dictionaryCounter

               header = line;

            }//end if mHeader

            else{
                sequence+=line;                         
            }//end else 

            dictionaryCounter++;

        }//end while

        //adding the last sequence of the fasta file
        AddToDictionary(sequences, header, sequence);
        sequence = "";
        file.Close();
        return sequences;

    }//end getDictionary

        
      //adds the sequnece with corresponding fasta header to the dictionary
      private static void AddToDictionary (Dictionary <string,string> dict, string fastaHeader, string seq){
          
          //putting the sequence and the coresponding fasta header in the dictionary
          if (dict.ContainsKey(fastaHeader)){              
          }
          else{
              dict.Add(fastaHeader, seq);
          }
      }//end addToDictionary




    
    //cut sequences with a non aa character in shorter sequences & keep only the sequences with a minimal length of 100 aa
      public static void Preprocessing(string inputPath, string outputPath, string idFilePath, string motivsSep, int minimalSequenceLength, string newIDstart) {
          
          string line = "";
          string sequence = "";
          string fastaHeader = "";
          Boolean haveBoth = false;
          int ownIdNumber = 0;
          string middle = "";
          string newID;
          string beginningOfNewId = newIDstart;

          //creating the ID for the original sequences                
          for (int i = 0; i < 6 - ownIdNumber.ToString().Count(); i++) { middle += "0"; }//end for
          newID = beginningOfNewId + middle + ownIdNumber.ToString();
          string[] motivArray = motivsSep.Split(';');
          string motivs = "";
          for (int i = 0; i < motivArray.Count(); i++)
          {
              motivs += motivArray[i];
          }
          Regex motiv = new Regex("[" + motivs + "]");
          Regex fastaHeaderRegex = new Regex("^>.*");
          StreamReader input = new StreamReader(inputPath);
          StreamWriter output = new StreamWriter(outputPath);
          StreamWriter idFile = new StreamWriter(idFilePath);

          while ((line = input.ReadLine()) != null)
          {

              line = line.Replace("\n", "");
              Match mHeader = fastaHeaderRegex.Match(line);

              //check if line is a faster header
              if (mHeader.Success)
              {

                  //check if header and sequence are availible
                  if (haveBoth)
                  {

                      //check if sequence contains a non amino acid character
                      Match isMotiv = motiv.Match(sequence);
                      if (isMotiv.Success)
                      {

                          //cut sequence bevor and after the non amino acid character
                          string[] seqs = Regex.Split(sequence, motivs);
                          //got through all part sequences
                          foreach (string tempPiece in seqs)
                          {
                              //check minimalSequenceLength
                              if (tempPiece.Length >= minimalSequenceLength)
                              {
                                  output.WriteLine(newID);
                                  output.WriteLine(tempPiece);
                                  idFile.WriteLine(newID + "\t" + fastaHeader);
                                  //creating new ID 
                                  middle = "";
                                  ownIdNumber++;
                                  for (int i = 0; i < 6 - ownIdNumber.ToString().Count(); i++) { middle += "0"; }//end for
                                  newID = beginningOfNewId + middle + ownIdNumber.ToString();
                              }//end if >= 100                    
                          }//end foreach tempPiece (seq)
                      }//end if isMotiv 

                      //sequence doesn't contain non amino acid character --> print!
                      else
                      {
                          //check minimalSequenceLength
                          if (sequence.Length >= minimalSequenceLength)
                          {
                              output.WriteLine(newID);
                              output.WriteLine(sequence);

                              idFile.WriteLine(newID + "\t" + fastaHeader);

                              //creating new ID  
                              middle = "";
                              ownIdNumber++;
                              for (int i = 0; i < 6 - ownIdNumber.ToString().Count(); i++) { middle += "0"; }//end for
                              newID = beginningOfNewId + middle + ownIdNumber.ToString();
                          }//end if minimalSequenceLength
                      }//end else
                  }//end if haveBoth 

                  fastaHeader = line;
                  haveBoth = false;
                  sequence = "";
              }//end if isHeader

              else
              {
                  sequence += line;
                  haveBoth = true;
              }//end else isHeader

          }//end while

          output.Close();
          input.Close();
          idFile.Close();
















/*        string line = "";
        string sequence = "";
        string fastaHeader = "";
        Boolean haveBoth = false;
        int ownIdNumber = 0;
        string middle = "";
        string newID;
        string beginningOfNewId=newIDstart;
        List<string> escapeCharacters=new List<string>(){"*"};

        //creating the ID for the original sequences                
        for (int i = 0; i < 6 - ownIdNumber.ToString().Count(); i++) { middle += "0"; }//end for
        newID = beginningOfNewId + middle + ownIdNumber.ToString();

        string [] nonaasArray = mo.Split(';');     
//Regex motiv = new Regex("[" + mo + "]");
        Regex fastaHeaderRegex = new Regex("^>.*");
        StreamReader input = new StreamReader(inputPath);
        StreamWriter output = new StreamWriter(outputPath);
        StreamWriter idFile = new StreamWriter(idFilePath);

        while ((line = input.ReadLine()) != null){

            line = line.Replace("\n", "");
            Match mHeader = fastaHeaderRegex.Match(line);           

            //check if line is a faster header
            if (mHeader.Success) {  

                //check if header and sequence are availible
                if(haveBoth){

                    //check if sequence contains a non amino acid character
                    List<string> seqPieceList = new List<string>();
                    seqPieceList.Add(sequence);
                    List<string> tempNewPieces = new List<string>();
                    for (int k = 0; k < nonaasArray.Count(); k++)
                    {
                        Regex motiv;
                        if (escapeCharacters.Contains(nonaasArray[k])){motiv = new Regex("\\" + nonaasArray[k]);}
                        else{motiv = new Regex(nonaasArray[k]);}
                        
                        foreach (string seqPiece in seqPieceList)
                        {
                            Match isMotiv = motiv.Match(seqPiece);
                            if (isMotiv.Success)
                            {
                                //cut sequence before and after the non amino acid character
                                string[] seqs = Regex.Split(sequence, nonaasArray[k]);
                                //got through all part sequences
                                foreach (string tempPiece in seqs)
                                {
                                    if (tempPiece != null && tempPiece != "")
                                    {
                                        tempNewPieces.Add(tempPiece);
                                    }
                                } //end foreach tempPiece (seq)
                                seqPieceList = new List<string>(tempNewPieces);
                                tempNewPieces = new List<string>();
                            } //end if isMotiv 
                        }//end foreach seqPiece
                    }//end nonaasArray

                    foreach (string piece in seqPieceList)
                    {
                        if (piece.Length >= minimalSequenceLength)
                        {
                            output.WriteLine(newID);
                            output.WriteLine(piece);
                            idFile.WriteLine(newID + "\t" + fastaHeader);

                            //creating new ID for the input sequences 
                            middle = "";
                            ownIdNumber++;
                            for (int i = 0; i < 6 - ownIdNumber.ToString().Count(); i++) { middle += "0"; }//end for
                            newID = beginningOfNewId + middle + ownIdNumber.ToString();
                        }
                    }
                }//end if haveBoth 
                fastaHeader = line;
                haveBoth=false; 
                sequence="";
            }//end if isHeader
          
            else{
                sequence+=line;
                haveBoth = true;
            }//end else isHeader

        }//end while

          output.Close();
          input.Close();
          idFile.Close();
*/
    }//end preprocessing       



      //creates own Ids for the homolog database
      public static void CreateInternalScaffoldId(Dictionary<String, String> oldDatabase, String idPrefix, String databasePath, String idPath) {
          StreamWriter dbOutput = new StreamWriter(databasePath);
          StreamWriter idOutput = new StreamWriter(idPath);
          int ownIdNumber = 0;
          string middle = "";
          string newID;

          foreach (String header in oldDatabase.Keys) {
              //creating ID                 
              for (int i = 0; i < 6 - ownIdNumber.ToString().Count(); i++) { middle += "0"; }//end for
              newID = idPrefix + middle + ownIdNumber.ToString();
              middle = "";
              ownIdNumber++;

              dbOutput.WriteLine(newID);
              dbOutput.WriteLine(oldDatabase [header]);

              idOutput.WriteLine(newID + "\t" + header);
          }

          dbOutput.Close();
          idOutput.Close();
      }

    }//end class
}//end namespace
