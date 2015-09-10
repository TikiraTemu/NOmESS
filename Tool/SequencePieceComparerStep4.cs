//using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;

namespace Tool {
    class SequencePieceComparerStep4 : IComparer<SequencePiece> {

        public int Compare(SequencePiece piece1, SequencePiece piece2) {

            int length1 = piece1.GetQueryLength();
            int length2 = piece2.GetQueryLength();

            double bitScore1 = piece1.GetBitScore();
            double bitScore2 = piece2.GetBitScore();

            double eValue1 = piece1.GetEValue();
            double eValue2 = piece2.GetEValue();
      

                if (( length1 > length2 ) ||
                    (( length1 == length2 ) && (bitScore1 > bitScore2)) ||
                    (( length1 == length2 ) && ( bitScore1 == bitScore2 ) && (eValue1 < eValue2) )
                    ){
                    return -1;
                }else{
                    return 1;
                }

        }//end Compare
    }
}
