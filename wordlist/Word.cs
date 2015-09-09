using System;
using System.Collections.Generic;
// 
using System.Text;

namespace wordlist
{
    public class Word
    {
        public int id, incorrect, correct;
        public string word = "", meaning = "", sentence = "", pron = "";
        public Word()
        {
        }


        public string Dump()
        {
            string toret = "";
            toret += word;
            toret += ", " + pron;
            toret += "\n" + sentence;
            toret += "\n";
            return toret;
        }
    }
}
