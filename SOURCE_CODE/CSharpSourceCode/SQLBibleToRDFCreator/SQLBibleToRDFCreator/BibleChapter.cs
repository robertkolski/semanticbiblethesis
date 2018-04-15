using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLBibleToRDFCreator
{
    public class BibleChapter
    {
        public string ChapterKey { get; set; }
        public short ChapterNumber { get; set; }
        public List<BibleVerse> BibleVerses { get; set; }
    }
}
