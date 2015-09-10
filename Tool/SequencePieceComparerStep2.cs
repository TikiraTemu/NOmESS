//using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;

namespace Tool
{
    class SequencePieceComparerStep2 : IComparer<SequencePiece>{

        public int Compare(SequencePiece piece1, SequencePiece piece2) {

            int subjectStart1=piece1.GetSubjectStart();
            int subjectStart2=piece2.GetSubjectStart();

            int queryLength1=piece1.GetQueryLength();
            int queryLength2=piece2.GetQueryLength();

            double bitScore1=piece1.GetBitScore();
            double bitScore2=piece2.GetBitScore();

            double eValue1=piece1.GetEValue();
            double eValue2=piece2.GetEValue();


            //sorts the list: subjectStart low to high, queryLength low to high, bitScore high to low, eValue low to high
                if (( subjectStart1 < subjectStart2 ) || 
                    ((subjectStart1 == subjectStart2) && ( queryLength1 < queryLength2)) ||
                    ((subjectStart1 == subjectStart2) && ( queryLength1 == queryLength2) && (bitScore1 > bitScore2)) ||
                    ((subjectStart1 == subjectStart2) && ( queryLength1 == queryLength2) && (bitScore1 == bitScore2) && (eValue1 < eValue2))
                    ) {
                    return -1;
                }
                else{
                    return 1;
                }                       

        }//end Compare



    }
}
