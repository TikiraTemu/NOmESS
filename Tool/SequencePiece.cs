//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

namespace Tool
{
    
    public class SequencePiece {

        private string _id;
        private int _queryLength;
        private int _subjectLength;
        private int _alignmentLength;
        private int _numberIdentity;
        private double _percentageIdentity;
        private int _misMatch;
        private int _gaps;
        private int _queryStart;
        private int _queryEnd;
        private int _subjectStart;
        private int _subjectEnd;
        private double _bitScore;
        private double _eValue;
        private string _sequence;

       
        //empty constructor
        public SequencePiece(){
        }//end
        
        
        //constructor with parameters
        public SequencePiece (string id, int queryLength, int subjectLength, int alignmentLength, int numberIdentity,
            double percentageIdentity, int misMatch, int gaps, int queryStart, int queryEnd, int subjectStart, int subjectEnd,
            double bitScore, double eValue,string sequence){

            _id = id;
            _queryLength = queryLength;
            _subjectLength = subjectLength;
            _alignmentLength = alignmentLength;
            _numberIdentity = numberIdentity;
            _percentageIdentity = percentageIdentity;
            _misMatch = misMatch;
            _gaps = gaps;
            _queryStart = queryStart;
            _queryEnd = queryEnd;
            _subjectStart = subjectStart;
            _subjectEnd = subjectEnd;
            _bitScore = bitScore;
            _eValue = eValue;
            _sequence = sequence;


        }//end constructor






        //GETTER AND SETTER

        //string id;
        public string GetId (){
            return _id;
        }
        public void SetId(string i){
            _id = i;
        }


        //int queryLength;
        public int GetQueryLength(){
            return _queryLength;
        }
        public void SetQueryLength(int qL){
            _queryLength = qL;
        }
        

        //int subjectLength;
        public int GetSubjectLength(){
            return _subjectLength;
        }
        public void SetSubjectLength(int sL){
            _subjectLength = sL;
        }


        //int alignmentLength;
        public int GetAlignmentLength(){
            return _alignmentLength;
        }
        public void SetAlignmentLength(int aL){
            _alignmentLength = aL;
        }


        //int numberIdentity;
        public int GetNumberIdentity(){
            return _numberIdentity;
        }
        public void SetNumberIdentity(int nI){
            _numberIdentity = nI;
        }


        //double percentageIdentity;
        public double GetPercentageIdentity(){
            return _percentageIdentity;
        }
        public void Set(double pI){
            _percentageIdentity = pI;
        }


        //int misMatch;
        public int GetMisMatch(){
            return _misMatch;
        }
        public void SetMisMatch(int mM){
            _misMatch = mM;
        }
        
        
        //int gaps;
        public int GetGaps(){
            return _gaps;
        }
        public void SetGaps(int g){
            _gaps = g;
        }


        //int queryStart;
        public int GetQueryStart(){
            return _queryStart;
        }
        public void SetQueryStart(int qS){
            _queryStart = qS;
        }


        //int queryEnd;
        public int GetQueryEnd(){
            return _queryEnd;
        }
        public void SetQueryEnd(int qE){
            _queryEnd = qE;
        }


        //int subjectStart;
        public int GetSubjectStart(){
            return _subjectStart;
        }
        public void SetSubjectStart(int sS){
            _subjectStart = sS;
        }


        //int subjectEnd;
        public int GetSubjectEnd(){
            return _subjectEnd;
        }
        public void SetSubjectEnd(int sE){
            _subjectEnd = sE;
        }


        //double bitScore;
        public double GetBitScore(){
            return _bitScore;
        }
        public void SetBitScore(double bS){
            _bitScore = bS;
        }


        //double eValue;
        public double GetEValue(){
            return _eValue;
        }
        public void SetEValue(double eV){
            _eValue = eV;
        }


        //string sequence
        public string GetSequence(){
            return _sequence;
        }
        public void SetSequence(string seq){
            _sequence = seq;
        }

        
    }//end class
}
