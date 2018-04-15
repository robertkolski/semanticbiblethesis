using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLBibleToRDFCreator
{
    public class BibleBook
    {
        public string BookKey { get; set; }
        public List<BibleChapter> BibleChapters { get; set; }
        public string BookName { get; set; }
    }
}
