using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLBibleToRDFCreator
{
    public class BibleEdition
    {
        public string BibleKey { get; set; }
        public string BibleName { get; set; }
        public List<BibleBook> BibleBooks { get; set; }
    }
}
