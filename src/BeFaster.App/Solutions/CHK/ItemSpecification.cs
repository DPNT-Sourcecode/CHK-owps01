using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeFaster.App.Solutions.CHK
{
    internal class ItemSpecification
    {
        public OfferType OfferType { get; set; }
        public Tuple<int, int> OfferMultiple2 { get; set; }
        public Tuple<int, int> OfferMultiple {  get; set; }
        public char FreebieRecepient { get; set; }
        public int BasePrice { get; set; }  
    }
}

