using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLBibleToRDFCreator
{
    public class BibleVerse
    {
        public string VerseKey { get; set; }
        public List<BibleWord> Words { get; set; }
        public string VerseText { get; set; }
        public int VerseNumber { get; set; }
    }
}
