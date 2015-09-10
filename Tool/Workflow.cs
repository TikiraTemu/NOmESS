using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;


namespace Tool
{
    public class Workflow
    {
        //input required
        private string _storageLocationPath;
        private string _blastPath;
        private string _cdHitPath;
        private string _inputSetPath;
        private string _scaffoldSetPath;

        //input optional
        //parameters for the beginning of the new ID
        private string _prefixNewId1;
        private string _prefixNewId2;
        private string _prefixNewId3;
        private string _prefixNewId4;
        //parameter for preprocessing
        private string _noAaMotiv;
        private Boolean _preprocessing;
        private int _minimalSeqLength;
        //parameters for cd-hit
        private double _cS;
        private double _c1;
        private double _c2;
        private double _c3;  
        //parameters for filtering BLAST output 1
        private double _percentageIdentity2;
        private double _eValue2;
        private double _bitScore2;
        private int _queryLength2;
        private int _alignmentLength2;
        //parameters for filtering BLAST output 2
        private double _percentageIdentity3;
        private double _eValue3;
        private double _bitScore3;
        private int _queryLength3;
        private int _alignmentLength3;
        //parameters for filtering BLAST output 3
        private double _percentageIdentity4;
        private double _eValue4;
        private double _bitScore4;
        private int _queryLength4;
        private int _alignmentLength4;
        

        public delegate void ProgressDelegate(string message, int percent);
        public event ProgressDelegate Progress;
        

        private void RaiseProgress(string message, int percent)
        {
            if (Progress != null)
            {
                Progress(message, percent);
            }
        }

        public void SetRequiredParameters(string storageLoc,string blastP,string cdHitP,string inputSet,string ScaffoldSet)
        {
             _storageLocationPath = storageLoc;
             _blastPath = blastP;
             _cdHitPath = cdHitP;
             _inputSetPath = inputSet;
             _scaffoldSetPath = ScaffoldSet;
        }//end SetRequiredParameters
               
        
        public void SetOptionalParameters(string beginningId1,string beginningId2,string beginningId3,string beginningId4,
            Boolean preproc, string noAA, int minSeqLength,
            double ch, double c1, double c2, double c3,
            double percentage2, double eVal2, double bitSc2, int queryLeng2,int aliLeng2,
            double percentage3, double eVal3, double bitSc3, int queryLeng3,int aliLeng3,
            double percentage4, double eVal4, double bitSc4, int queryLeng4,int aliLeng4){
         
            _prefixNewId1 = ">" + beginningId1;
            _prefixNewId2 = ">" + beginningId2;
            _prefixNewId3 = ">" + beginningId3;
            _prefixNewId4 = ">" + beginningId4;
            
            _preprocessing = preproc;
            _noAaMotiv = noAA;
            _minimalSeqLength = minSeqLength;

            _cS = ch;
            _c1 = c1;
            _c2 = c2;
            _c3 = c3;  
        
            _percentageIdentity2 = percentage2;
            _eValue2 = eVal2;
            _bitScore2 = bitSc2;
            _queryLength2 = queryLeng2;
            _alignmentLength2 = aliLeng2;

            _percentageIdentity3 = percentage3;
            _eValue3 = eVal3;
            _bitScore3 = bitSc3;
            _queryLength3 = queryLeng3;
            _alignmentLength3 = aliLeng3;

            _percentageIdentity4 = percentage4;
            _eValue4 = eVal4;
            _bitScore4 = bitSc4;
            _queryLength4 = queryLeng4;
            _alignmentLength4 = aliLeng4;
         
        }//end SetOptionalParameters
        
        
        //calls the cd-hit.exe
        public void CdHitCall(string workingDirectoryPath, string input, string output, double seqident)
        {
            int num = 0;
            
            if ((seqident <= 1.0) && (seqident >= 0.7)) { num = 5; }
            if ((seqident < 0.7) &&  (seqident >= 0.6)) { num = 4; }
            if ((seqident < 0.6) &&  (seqident >= 0.5)) { num = 3; }
            if ((seqident < 0.5) &&  (seqident >= 0.4)) { num = 2; }
            
            Process proc_CDHIT = new Process();
            proc_CDHIT.StartInfo.WorkingDirectory = @workingDirectoryPath;
            proc_CDHIT.StartInfo.FileName = workingDirectoryPath + "cd-hit.exe";
            proc_CDHIT.StartInfo.Arguments = "-i " + input + " -o " + output + " -c " + seqident.ToString() + " -n " + num.ToString() + " -G 1 -g 1 -b 20 -s 0.0 -aL 0.0 -aS 0.0";
            proc_CDHIT.StartInfo.UseShellExecute = true;
            proc_CDHIT.StartInfo.RedirectStandardError = false;
            proc_CDHIT.Start();
            proc_CDHIT.WaitForExit();
            proc_CDHIT.Close(); 
        }
        

        //calls the blastp.exe
        public void BlastCall(string workingDirectoryPath, string querySet, string output )
        {
            Process proc_Blast = new Process();
            proc_Blast.StartInfo.WorkingDirectory = @workingDirectoryPath;
            proc_Blast.StartInfo.FileName = workingDirectoryPath + "blastp.exe";
            proc_Blast.StartInfo.Arguments = "-query " + querySet + " -db scaffoldDatabase -out " + output + " -outfmt \"6 qseqid sseqid qlen slen length nident pident mismatch gaps qstart qend sstart send bitscore evalue\" -num_descriptions 1";
            proc_Blast.StartInfo.UseShellExecute = true;
            proc_Blast.StartInfo.RedirectStandardError = false;
            proc_Blast.Start();
            proc_Blast.WaitForExit();
            proc_Blast.Close();
        }
        

        public void StartWorkflow()
        {

            //--------------------------------------------------------------------------------------------------------------

RaiseProgress("Initializing...", 0);
            /* internal program stuff
             * creating all necessary strings */

            Blast blastObject = new Blast();
            allIDs allIDsObject = new allIDs();

            string path_wholeOutput = _storageLocationPath + "NOmESS_Output\\";
            string path_temporaryWorkingDirectory = path_wholeOutput + "temp\\";
            string path_step1 = path_wholeOutput + "Step 1\\";
            string path_step2 = path_wholeOutput + "Step 2\\";
            string path_step3 = path_wholeOutput + "Step 3\\";
            string path_step4 = path_wholeOutput + "Step 4\\";
            string path_finalOutput = path_wholeOutput + "Final_Output\\";
            string path_sortedDictionaryFiles_step2 = path_step2 + "SortedDictionaryFiles_Step2\\";
            string path_sortedDictionaryFiles_step3 = path_step3 + "SortedDictionaryFiles_Step3\\";
            string path_allSequenceFilesHomologName = path_finalOutput + "allSequences_filename=scaffoldID\\";

            //create all needed folders 
            Directory.CreateDirectory(path_wholeOutput);
            Directory.CreateDirectory(path_temporaryWorkingDirectory);
            Directory.CreateDirectory(path_step1);
            Directory.CreateDirectory(path_step2);
            Directory.CreateDirectory(path_step3);
            Directory.CreateDirectory(path_step4);
            Directory.CreateDirectory(path_sortedDictionaryFiles_step2);
            Directory.CreateDirectory(path_sortedDictionaryFiles_step3);

            /*copy the executable files into the working directory 
             * --> blastp.exe and makeblastdb.exe 
             * --> cd-hit.exe */
            File.Copy(_blastPath + "blastp.exe", path_temporaryWorkingDirectory + "blastp.exe", true);
            File.Copy(_blastPath + "makeblastdb.exe", path_temporaryWorkingDirectory + "makeblastdb.exe", true);
            File.Copy(_cdHitPath + "cd_hit.exe", path_temporaryWorkingDirectory + "cd-hit.exe", true);

            /*print settings in a file
             */
            StreamWriter writer = new StreamWriter(path_wholeOutput + "used_settings.txt");

            writer.WriteLine("Required parameters");
            writer.WriteLine();
            writer.WriteLine("Storage location\t" + _storageLocationPath);
            writer.WriteLine("Blastp\t" + _blastPath);
            writer.WriteLine("Cd-hit\t" + _cdHitPath);
            writer.WriteLine("Input set\t" + _inputSetPath);
            writer.WriteLine("Scaffold set\t" + _scaffoldSetPath);
            writer.WriteLine(); writer.WriteLine(); writer.WriteLine(); writer.WriteLine(); writer.WriteLine();

            writer.WriteLine("Optional parameters");
            writer.WriteLine();
            writer.WriteLine("ID prefixes");
            writer.WriteLine("Prefix step 1\t" + _prefixNewId1);
            writer.WriteLine("Prefix step 2\t" + _prefixNewId2);
            writer.WriteLine("Prefix step 3\t" + _prefixNewId3);
            writer.WriteLine("Prefix step 4\t" + _prefixNewId4);
            writer.WriteLine();

            writer.WriteLine("Preprocessing");
            writer.WriteLine("Preprocessing ( step 1 )\t" + _preprocessing);
            writer.WriteLine("Non amino acid characters\t" + _noAaMotiv);
            writer.WriteLine("Minimal sequence length\t" + _minimalSeqLength);
            writer.WriteLine();

            writer.WriteLine("Identity thresholds (cd-hit)");
            writer.WriteLine("Scaffold set\t" + _cS);
            writer.WriteLine("Step 1\t" + _c1);
            writer.WriteLine("Step 2\t" + _c2);
            writer.WriteLine("Step 3\t" + _c3);
            writer.WriteLine();

            writer.WriteLine("Homology thresholds (blastp)");
            writer.WriteLine("Step 2: percentage identity\t" + _percentageIdentity2);
            writer.WriteLine("Step 2: e-value\t" + _eValue2);
            writer.WriteLine("Step 2: bit score\t" + _bitScore2);
            writer.WriteLine("Step 2: query length\t" + _queryLength2);
            writer.WriteLine("Step 2: alignment length\t" + _alignmentLength2);
            writer.WriteLine("Step 3: percentage identity\t" + _percentageIdentity3);
            writer.WriteLine("Step 3: e-value\t" + _eValue3);
            writer.WriteLine("Step 3: bit score\t" + _bitScore3);
            writer.WriteLine("Step 3: query length\t" + _queryLength3);
            writer.WriteLine("Step 3: alignment length\t" + _alignmentLength3);
            writer.WriteLine("Step 4: percentage identity\t" + _percentageIdentity4);
            writer.WriteLine("Step 4: e-value\t" + _eValue4);
            writer.WriteLine("Step 4: bit score\t" + _bitScore4);
            writer.WriteLine("Step 4: query length\t" + _queryLength4);
            writer.WriteLine("Step 4: alignment length\t" + _alignmentLength4);

            writer.Close();

            /*use cd-hit for reducing sequences of homologous database --> 0.95 percentage identity
             *                                                         --> copy homologous database into working directory
             *                                                         --> copy output file in storage location */
            String ids_scaffold = path_wholeOutput + "IDs_scaffoldOrganism.txt";
            Dictionary<String, String> oldDatabase = Database.GetDictionary(_scaffoldSetPath);
            Database.CreateInternalScaffoldId(oldDatabase, ">scaffold_", path_temporaryWorkingDirectory + "scaffoldSet.fasta", ids_scaffold);
            
            CdHitCall(path_temporaryWorkingDirectory, "scaffoldSet.fasta", "scaffoldSet-Cl.fasta", _cS);
            File.Copy(path_temporaryWorkingDirectory + "scaffoldSet-Cl.fasta", path_wholeOutput + "scaffoldSet-Cl.fasta", true);
            string scaffoldSet = _storageLocationPath + "scaffoldSet-Cl.fasta";
            File.Delete(path_temporaryWorkingDirectory + "scaffoldSet.fasta");



            /*preprocessing / step 0 --> cut the sequences containing non aa characters before and after those
             *                       --> check if sequence longer than given minimal length */
            string database_step1 = path_step1 + "STEP1.fasta";
            string ids_step1 = path_step1 + "IDs_Step1.txt";

            if (_preprocessing)
            {
RaiseProgress("Step 1: Preprocessing...", 5);
                Database.Preprocessing(_inputSetPath, database_step1, ids_step1, _noAaMotiv, _minimalSeqLength, _prefixNewId1);
            }
            else
            {
                Dictionary<String, String> oldInputDatabase = Database.GetDictionary(_inputSetPath);
                Database.CreateInternalScaffoldId(oldInputDatabase, _prefixNewId1, database_step1, ids_step1);
            }

            /*use cd-hit for reducing sequences of database_step1 --> 1.0 percentage identity
             *                                                    --> copy database_step1 into working directory
             *                                                    --> copy output file in Output_Step0 */
            File.Copy(database_step1, path_temporaryWorkingDirectory + "STEP1.fasta", true);
            CdHitCall(path_temporaryWorkingDirectory, "STEP1.fasta", "STEP1-Cl.fasta",_c1);

            File.Copy(path_temporaryWorkingDirectory + "STEP1-Cl.fasta", path_step1 + "STEP1-Cl.fasta", true);



RaiseProgress("Step 2: Fragment grouping...", 15);
            /*first blast --> copy homologous database into working directory
             *            --> use makeblastdb.exe and blastp.exe 
                          --> copy blast output into Output_Step1 */
            Process proc_MakeDB = new Process();
            proc_MakeDB.StartInfo.WorkingDirectory = @path_temporaryWorkingDirectory;
            proc_MakeDB.StartInfo.FileName = _blastPath + "makeblastdb.exe";
            proc_MakeDB.StartInfo.Arguments = "-in scaffoldSet-Cl.fasta -out scaffoldDatabase";
            proc_MakeDB.StartInfo.UseShellExecute = true;
            proc_MakeDB.StartInfo.RedirectStandardError = false;
            proc_MakeDB.Start();
            proc_MakeDB.WaitForExit();
            proc_MakeDB.Close();

            BlastCall(path_temporaryWorkingDirectory, "STEP1-Cl.fasta", "blastForStep2.txt");

            File.Copy(path_temporaryWorkingDirectory + "blastForStep2.txt", path_step2 + "blastForStep2.txt", true);
            File.Delete(path_temporaryWorkingDirectory + "STEP1.fasta");
            File.Delete(path_temporaryWorkingDirectory + "blastForStep2.txt");

            /*step 1 --> sequence assembly */
            string blastOutput_step2 = path_step2 + "blastForStep2.txt";
            string database_step2 = path_step2 + "STEP2.fasta";
            string ids_step2 = path_step2 + "IDs_Step2.txt";
            string sortedList_step2 = path_step2 + "sortedLists_Step2.txt";
            string restSequences_step2 = path_step2 + "restSequencePieces_Step2.fasta";
            string blastDictionary_step2 = path_step2 + "BlastDictionary_Step2.txt";
            string notTakenSequences_step2 = path_step2 + "SNT2.fasta";
            string seqsWithoutScaffoldPath = path_step2 + "SWS.fasta";
            Boolean printSWS = true;

RaiseProgress("Step 2: Assembly...", 15);
            blastObject.CreateNonRedundantDatabase_step2(blastOutput_step2, scaffoldSet, path_step1 + "STEP1-Cl.fasta", database_step2, ids_step2, sortedList_step2, restSequences_step2, blastDictionary_step2, path_sortedDictionaryFiles_step2, notTakenSequences_step2, _percentageIdentity2, _eValue2, _bitScore2, _queryLength2, _alignmentLength2, _prefixNewId2, seqsWithoutScaffoldPath, printSWS);

            /*use cd-hit for reducing sequences of database_step2 --> 0.95 percentage identity
             *                                                    --> copy database_step2 into working directory
             *                                                    --> copy output file in Output_Step1 */
            File.Copy(database_step2, path_temporaryWorkingDirectory + "STEP2.fasta", true);
            CdHitCall(path_temporaryWorkingDirectory, "STEP2.fasta", "STEP2-Cl.fasta", _c2);

            File.Copy(path_temporaryWorkingDirectory + "STEP2-Cl.fasta", path_step2 + "STEP2-Cl.fasta", true);
            File.Delete(path_temporaryWorkingDirectory + "STEP2.fasta");



RaiseProgress("Step 3: Fragment grouping...", 55);            
            /*second blast --> use blastp.exe 
                           --> copy blast output into Output_Step2 */
            BlastCall(path_temporaryWorkingDirectory, "STEP2-Cl.fasta", "blastForStep3.txt");

            File.Copy(path_temporaryWorkingDirectory + "blastForStep3.txt", path_step3 + "blastForStep3.txt", true);
            File.Delete(path_temporaryWorkingDirectory + "STEP2-Cl.fasta");
            File.Delete(path_temporaryWorkingDirectory + "blastForStep3.txt");
            
            /* step 2 --> concatenation of non-overlapping sequence pieces */
            string blastOutput_step3 = path_step3 + "blastForStep3.txt";
            string database_step3 = path_step3 + "STEP3.fasta";
            string ids_step3 = path_step3 + "IDs_Step3.txt";
            string sortedList_step3 = path_step3 + "sortedLists_Step3.txt";
            string blastDictionary_step3 = path_step3 + "BlastDictionary_Step3.txt";
            string notTakenSequences_step3 = path_step3 + "SNT3.fasta";

RaiseProgress("Step 3: Concatenation...", 55);
            blastObject.CreateNonRedundantDatabase_step3(blastOutput_step3, path_step2 + "STEP2-Cl.fasta", database_step3, ids_step3, sortedList_step3, path_sortedDictionaryFiles_step3, blastDictionary_step3, notTakenSequences_step3, _percentageIdentity3, _eValue3, _bitScore3, _queryLength3, _alignmentLength3, _prefixNewId3);
            
            /*use cd-hit for reducing sequences of database_step3 --> 0.95 percentage identity
             *                                                    --> copy database_step3 into working directory
             *                                                    --> copy output file in Output_Step2 */
            File.Copy(database_step3, path_temporaryWorkingDirectory + "STEP3.fasta", true);
            CdHitCall(path_temporaryWorkingDirectory, "STEP3.fasta", "STEP3-Cl.fasta", _c3);

            File.Copy(path_temporaryWorkingDirectory + "STEP3-Cl.fasta", path_step3 + "STEP3-Cl.fasta", true);
            File.Delete(path_temporaryWorkingDirectory + "STEP3.fasta");



RaiseProgress("Step 4: Fragment grouping...", 80);            
            /*third blast --> use blastp.exe 
                          --> copy blast output into Output_Step3 */
            BlastCall(path_temporaryWorkingDirectory, "STEP3-Cl.fasta", "blastForStep4.txt");

            File.Copy(path_temporaryWorkingDirectory + "blastForStep4.txt", path_step4 + "blastForStep4.txt", true);
          
            /*step 3 --> take longest representant for each homologous protein */
            string blastOutput_step4 = path_step4 + "blastForStep4.txt";
            string ids_step4 = path_step4 + "IDs_Step4.txt";
            string sortedList_step4 = path_step4 + "sortedLists_Step4.txt";
            string longestSequences_step4 = path_step4 + "STEP4.fasta";
            string shorterSequences_step4 = path_step4 + "shorterSequences_Step4.fasta";
            string blastDictionary_step4 = path_step4 + "BlastDictionary_Step4.txt";
            string notTakenSequences_step4 = path_step4 + "SNT4.fasta";

RaiseProgress("Step 4: Representative selection...", 80);
            blastObject.CreateNonRedundantDatabase_step4(blastOutput_step4, path_step3 + "STEP3-Cl.fasta", ids_step4, sortedList_step4, longestSequences_step4, shorterSequences_step4, blastDictionary_step4, notTakenSequences_step4, _percentageIdentity4, _eValue4, _bitScore4, _queryLength4, _alignmentLength4, _prefixNewId4);
            
            //creating file with all IDs and for each homolog sequence all including sequences            
            Dictionary<string, List<string>> ids_0 = allIDsObject.GetDict(ids_step1, 0, 1);
            Dictionary<string, List<string>> ids_1 = allIDsObject.GetDict(ids_step2, 0, 2);
            Dictionary<string, List<string>> ids_2 = allIDsObject.GetDict(ids_step3, 0, 2);
            Dictionary<string, List<string>> ids_3 = allIDsObject.GetDict(ids_step4, 0, 1);
            Dictionary<string, List<string>> ids_homo = allIDsObject.GetDict(ids_scaffold, 0, 1);

            Dictionary<string, string> databaseOriginal = Database.GetDictionary(_inputSetPath);
            Dictionary<string, string> databaseAfter0 = Database.GetDictionary(path_step1 + "STEP1-Cl.fasta");
            Dictionary<string, string> databaseAfter1 = Database.GetDictionary(path_step2 + "STEP2-Cl.fasta");
            Dictionary<string, string> databaseAfter2 = Database.GetDictionary(path_step3 + "STEP3-Cl.fasta");
            Dictionary<string, string> databaseAfter3 = Database.GetDictionary(path_step4 + "STEP4.fasta");
            Dictionary<string, string> scaffoldDatabase = Database.GetDictionary(path_wholeOutput + "scaffoldSet-Cl.fasta");

            string idOutputPath = path_wholeOutput + "IDs.txt";
            allIDsObject.createFinalIDOutput(ids_0, ids_1, ids_2, ids_3, ids_homo, databaseOriginal, databaseAfter0, databaseAfter1, databaseAfter2, databaseAfter3, scaffoldDatabase, idOutputPath, path_allSequenceFilesHomologName, 1);
            
            //copying all interesting files into the output folder
            File.Copy(path_step1 + "STEP1-Cl.fasta", path_wholeOutput + "STEP1-Cl.fasta",true);
            File.Copy(path_step2 + "STEP2.fasta", path_wholeOutput + "STEP2.fasta", true);
            File.Copy(path_step2 + "STEP2-Cl.fasta", path_wholeOutput + "STEP2-Cl.fasta", true);
            File.Copy(path_step3 + "STEP3.fasta", path_wholeOutput + "STEP3.fasta", true);
            File.Copy(path_step3 + "STEP3-Cl.fasta", path_wholeOutput + "STEP3-Cl.fasta", true);
            File.Copy(path_step4 + "STEP4.fasta", path_wholeOutput + "STEP4.fasta", true);
            File.Copy(path_step2 + "SNT2.fasta", path_wholeOutput + "SNT2.fasta", true);
            File.Copy(path_step3 + "SNT3.fasta", path_wholeOutput + "SNT3.fasta", true);
            File.Copy(path_step4 + "SNT4.fasta", path_wholeOutput + "SNT4.fasta", true);
            File.Copy(path_step2 + "SWS.fasta", path_wholeOutput + "SWS.fasta", true);


            //deleting the non necessary files
            Directory.Delete(path_step1, true);
            Directory.Delete(path_step2, true);
            Directory.Delete(path_step3, true);
            Directory.Delete(path_step4, true);
            Directory.Delete(path_wholeOutput + "temp", true);
            File.Delete(path_wholeOutput + "scaffoldSet-Cl.fasta");
            File.Delete(path_wholeOutput + "IDs.txt");
            File.Delete(path_wholeOutput + "IDs_scaffoldOrganism.txt");


            //pop up "Done!" window
RaiseProgress("Done!", 100);
            Done done = new Done();
            done.ShowDialog();
        }//end StartWorkflow


    }//end class
}//end namespace
