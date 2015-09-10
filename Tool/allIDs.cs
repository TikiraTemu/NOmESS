using System;
using System.Collections.Generic;
using System.IO;

namespace Tool
{
    class allIDs {

        public Dictionary<string, List<string>> GetDict (string filePath, int keyColumn, int columnOfValueStart) {

            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
            string tempLine = "";

            //read in file line by line
            StreamReader file = new StreamReader(filePath);

            while (( tempLine = file.ReadLine()) != null ) {

                //remove newline & splitting the tab separated line & packing the items in an array 
                tempLine = tempLine.Replace("\n", "");
                string [] line = tempLine.Split('\t');

                string key = line [keyColumn];
                List<string> value = new List<string>();  

                if (dict.ContainsKey(key)) {
                    value = dict [key];
                    for (int i = columnOfValueStart ; i < line.Length ; i++) {
                        value.Add(line [i]);
                    }
                    dict [key] = value;
                    
                }
                else{
                    for (int i = columnOfValueStart; i < line.Length; i++) {
                        value.Add(line [i]);
                    } 
                       dict.Add(key, value);
                }
                
                                

            }//end while

            file.Close();

            if (dict.ContainsKey("")) {
                dict.Remove("");               
            }


            return dict;

        }//end GetDict

        

        public void createFinalIDOutput(Dictionary<string,List<string>> idsStep0, Dictionary<string, List<string>> idsStep1, Dictionary<string, List<string>> idsStep2, Dictionary<string,List<string>> idsStep3, Dictionary<string,List<string>> idsHomolog,
                                        Dictionary<string,string> databaseOriginal, Dictionary<string, string> databaseAfter0, Dictionary<string, string> databaseAfter1, Dictionary<string, string> databaseAfter2, Dictionary<string, string> databaseAfter3, Dictionary<string,string> homologDatabase,
                                        string idOutputPath, string sequenceFilesPath, int nameOfFiles) {

            StreamWriter idFile = new StreamWriter(idOutputPath);
            List<string> ID3Keys = new List<string>(idsStep3.Keys);
            string idFileLine;
            string homologID;
            string ID2;

            List<string> valuesOfID3 = new List<string>();
            List<string> valuesOfID2 = new List<string>();
            List <List<string>> valuesOfID1 = new List<List<string>>();
            List<string> valuesOfID0 = new List<string>();

            List<string> valueOfOneID= new List<string>();
                        
            Boolean isFirst;
            
            foreach (string ID3 in ID3Keys) {

                isFirst = true;

                valuesOfID3 = idsStep3 [ID3];
                homologID = valuesOfID3[0];
                ID2 = valuesOfID3[1];

                idFileLine = ID3 + "\t" + valuesOfID3 [0] + "\t" + idsHomolog[homologID][0] + "\t" + valuesOfID3 [1] + "\t";

                valuesOfID2 = idsStep2 [ID2];
                
                foreach (string ID1 in valuesOfID2) {

                    if (isFirst) { idFileLine += ID1; isFirst = false; }
                    else{idFileLine += ";" + ID1;}

                    valuesOfID1.Add(idsStep1 [ID1]);
                     
                }//end foreach ID1

                isFirst = true;
                
                foreach (List<string> list0 in valuesOfID1) {
                    idFileLine += "\t";
                    foreach (string ID0 in list0){

                        if(isFirst){idFileLine += ID0; isFirst = false;}
                        else{idFileLine += ";" + ID0;}

                        valueOfOneID = idsStep0[ID0];
                        valuesOfID0.Add(valueOfOneID[0]);

                    }//end foreach ID0

                    isFirst = true;
                    valueOfOneID = new List<string>();

                }//end foreach list0
                    
                foreach (string original in valuesOfID0) {
                    idFileLine += "\t" + original;
                }//end foreach original
                             
                idFile.WriteLine(idFileLine);

                //new set of the using data structures
                idFileLine = "";
                valuesOfID3 = new List<string>();
                valuesOfID2 = new List<string>();
                valuesOfID1 = new List<List<string>>();
                valuesOfID0 = new List<string>();
                valueOfOneID = new List<string>();

            }//end foreach ID3

            idFile.Close();
                
        }//end createAllSequencesFiles
        
    }//end class
}//end namespace
