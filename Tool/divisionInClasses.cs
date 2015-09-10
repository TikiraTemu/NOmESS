using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Tool
{
    
    class divisionInClasses {

        public static void GetClasses(string inputofAllIds, string fullLengthPath, string onlyAssembledPath, string onlyConcatenatedPath, string assembledAndConcatenatedPath, string summaryPath){

            System.IO.StreamReader inputFile = new StreamReader(inputofAllIds);
            System.IO.StreamWriter fullLength = new StreamWriter(fullLengthPath);
            System.IO.StreamWriter onlyAssembled = new StreamWriter(onlyAssembledPath);
            System.IO.StreamWriter onlyConcatenated = new StreamWriter(onlyConcatenatedPath);
            System.IO.StreamWriter assembledAndConcatenated = new StreamWriter(assembledAndConcatenatedPath);
            System.IO.StreamWriter summary = new StreamWriter(summaryPath);

            int numberFullLength = 0;
            int numberOnlyAssem = 0;
            int numberOnlyConcat = 0;
            int numberAssemAndConcat = 0;

            string tempLine;

            Regex separator = new Regex(";");

            while (( tempLine = inputFile.ReadLine() ) != null) {

                //remove newline & splitting the tab separated line & packing the items in an array 
                tempLine = tempLine.Replace("\n", "");
                string [] line = tempLine.Split('\t');

                //check if ID3 is a ssembled one
                Match isAssem = separator.Match(line [4]);
                Match isConcat = separator.Match(line [3]);

                if (( isAssem.Success == true ) && ( isConcat.Success == true )) { assembledAndConcatenated.WriteLine(tempLine); numberAssemAndConcat++; }
                else if (( isAssem.Success == true ) && ( isConcat.Success == false)) { onlyAssembled.WriteLine(tempLine); numberOnlyAssem++; }
                else if (( isAssem.Success == false) && ( isConcat.Success == true )) {

                    string [] tempID1 = line [3].Split(';');
                    int numberOfIDs1 = tempID1.Length;
                    Boolean both = false;

                    for (int i = 5; i < 4 + numberOfIDs1; i++) {
                        Match isAssemRestID1 = separator.Match(line [i]);
                        if (isAssemRestID1.Success) { both = true; }
                        else{ }
                    }//end for loop                         
                    
                    if(both){assembledAndConcatenated.WriteLine(tempLine); numberAssemAndConcat++;}
                    else{onlyConcatenated.WriteLine(tempLine); numberOnlyConcat++;}

                }//end if

                else if (( isAssem.Success == false) && ( isConcat.Success == false)) { fullLength.WriteLine(tempLine); numberFullLength++; }
                
            }//end while

            summary.WriteLine("number of sequences in every classification");
            summary.WriteLine("full length:\t" + numberFullLength);
            summary.WriteLine("only assembled:\t" + numberOnlyAssem);
            summary.WriteLine("only concatenated:\t" + numberOnlyConcat);
            summary.WriteLine("assembled & concatenated:\t" + numberAssemAndConcat);

            inputFile.Close();
            fullLength.Close();
            onlyAssembled.Close();
            onlyConcatenated.Close();
            assembledAndConcatenated.Close();
            summary.Close();

        }//end GetClasses


    }//end class
}//end namespace
