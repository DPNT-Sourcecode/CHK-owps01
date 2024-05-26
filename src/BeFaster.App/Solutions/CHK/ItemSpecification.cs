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
        public List<DiscountOffer> DiscountOffers { get; set; }
        public FreebieOffer FreebieOffer { get; set; }
        public int BasePrice { get; set; }  
    }
}
