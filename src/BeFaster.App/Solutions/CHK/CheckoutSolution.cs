using BeFaster.Runner.Exceptions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BeFaster.App.Solutions.CHK
{
    public class CheckoutSolution
    {
        Dictionary<char, ItemSpecification> SpecificationOfEachItem;
        Dictionary<char, int> CountOfEachItem;
        public int ComputePrice(string skus)
        {
            SpecificationOfEachItem = GetSpecificationOfEachItem();
            CountOfEachItem = GetCountOfEachItem(skus);

            if (CountOfEachItem == null)
            {
                return -1;
            }

            int TotalPrice = 0;

            foreach (KeyValuePair<char, int> keyValuePair in CountOfEachItem)
            {
                if (SpecificationOfEachItem[keyValuePair.Key].OfferType == OfferType.None)
                {
                    TotalPrice += SpecificationOfEachItem[keyValuePair.Key].BasePrice * keyValuePair.Value;
                }
                else
                {
                    TotalPrice += GetItemPrice(SpecificationOfEachItem[keyValuePair.Key], keyValuePair.Value);
                }
            }

            return TotalPrice;
        }

        private int GetItemPrice(ItemSpecification itemSpecification, int itemCount)
        {
            int ItemPrice = 0;
            switch (itemSpecification.OfferType)
            {
                case OfferType.Discount:
                    ItemPrice = GetItemDiscountPrice(itemSpecification, itemCount);
                    break;
                case OfferType.MultipleDiscounts:
                    ItemPrice = GetItemWithMultipleDiscountPrice(itemSpecification, itemCount);
                    break;
                case OfferType.FreebieOfSameItem:
                    ItemPrice = GetFreebieOfSameItemPrice(itemSpecification, itemCount);
                    break;
                case OfferType.FreebieOfDifferentItem:
                    ItemPrice = GetFreebieOfDifferentItemPrice(itemSpecification, itemCount);
                    break;
            }
            return ItemPrice;
        }

        private int GetFreebieOfDifferentItemPrice(ItemSpecification itemSpecification, int itemCount)
        {
            int Price = itemSpecification.BasePrice * itemCount;
            int NumberOfFreebies = (itemCount - (itemCount % itemSpecification.FreebieOffer.Multiple)) / itemSpecification.FreebieOffer.Multiple;

            int RecipientCount = CountOfEachItem.ContainsKey(itemSpecification.FreebieOffer.Recipient) ? CountOfEachItem[itemSpecification.FreebieOffer.Recipient] : 0;

            if (RecipientCount == 0)
            {
                return Price;
            }

            ItemSpecification RecipientItem = SpecificationOfEachItem[itemSpecification.FreebieOffer.Recipient];
            int PriceToPayForRecipientItems = RecipientCount * RecipientItem.BasePrice;

            if (RecipientCount > NumberOfFreebies)
            {
                if (RecipientItem.DiscountOffers.Count == 1)
                {
                    PriceToPayForRecipientItems = GetItemDiscountPrice(RecipientItem, RecipientCount - NumberOfFreebies);
                }
                else if (RecipientItem.DiscountOffers.Count > 1)
                {
                    PriceToPayForRecipientItems = GetItemWithMultipleDiscountPrice(RecipientItem, RecipientCount - NumberOfFreebies);
                }

                return Price - PriceToPayForRecipientItems);
            }
            else
            {
                if (RecipientItem.DiscountOffers.Count == 1)
                {
                    PriceToPayForRecipientItems = GetItemDiscountPrice(RecipientItem, RecipientCount);
                }
                else if (RecipientItem.DiscountOffers.Count > 1)
                {
                    PriceToPayForRecipientItems = GetItemWithMultipleDiscountPrice(RecipientItem, RecipientCount);
                }

                return Price - PriceToPayForRecipientItems;
            }
        }

        private static int GetFreebieOfSameItemPrice(ItemSpecification itemSpecification, int itemCount)
        {
            FreebieOffer FreebieOffer = itemSpecification.FreebieOffer;
            int Remainder = itemCount % FreebieOffer.Multiple;

            int NumberOfMultiples = (itemCount - Remainder) / FreebieOffer.Multiple;
            return (itemCount - NumberOfMultiples) * itemSpecification.BasePrice;
        }

        private static int GetItemWithMultipleDiscountPrice(ItemSpecification itemSpecification, int itemCount)
        {
            // Assume DiscountOffer are sorted from smallest to largest. 

            DiscountOffer LargerDiscountOffer = itemSpecification.DiscountOffers.Last();
            int Remainder = itemCount % LargerDiscountOffer.Multiple;
            if (Remainder == 0)
            {
                return (itemCount / LargerDiscountOffer.Multiple) * LargerDiscountOffer.Value;
            }
            else
            {
                int NumberOfItemsForOfferMultiple2 = itemCount - Remainder;
                int OfferPrice = (NumberOfItemsForOfferMultiple2 / LargerDiscountOffer.Multiple) * LargerDiscountOffer.Value;

                Remainder = itemCount - NumberOfItemsForOfferMultiple2;
                DiscountOffer SmallerDiscount = itemSpecification.DiscountOffers.First();

                if (Remainder >= SmallerDiscount.Multiple)
                {
                    return OfferPrice + GetItemDiscountPrice(itemSpecification, Remainder);
                }
                else
                {
                    return OfferPrice + (itemSpecification.BasePrice * Remainder);
                }
            }
        }

        private static int GetItemDiscountPrice(ItemSpecification itemSpecification, int itemCount)
        {
            DiscountOffer DiscountOffer = itemSpecification.DiscountOffers.First();
            int Remainder = itemCount % DiscountOffer.Multiple;

            if (Remainder == 0)
            {
                return (itemCount / DiscountOffer.Multiple) * DiscountOffer.Value;
            }
            else
            {
                int OfferPrice = ((itemCount - Remainder) / DiscountOffer.Multiple) * DiscountOffer.Value;
                int remainderPrice = Remainder * itemSpecification.BasePrice;
                return OfferPrice + remainderPrice;
            }
        }

        private static Dictionary<char, int> GetCountOfEachItem(string skus)
        {
            Dictionary<char, int> CountsOfEachItem = new Dictionary<char, int>();

            char[] ValidCharacters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
            'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'};

            foreach (char character in skus)
            {
                if (CountsOfEachItem.ContainsKey(character))
                {
                    CountsOfEachItem[character]++;
                }
                else if (ValidCharacters.Contains(character))
                {
                    CountsOfEachItem[character] = 1;
                }
                else
                {
                    return null;
                }
            }
            return CountsOfEachItem;
        }

        private static Dictionary<char, ItemSpecification> GetSpecificationOfEachItem()
        {
            Dictionary<char, ItemSpecification> SpecificationOfEachItem = new Dictionary<char, ItemSpecification>();
            SpecificationOfEachItem['A'] = new ItemSpecification()
            {
                BasePrice = 50,
                OfferType = OfferType.MultipleDiscounts,
                DiscountOffers = new List<DiscountOffer> {
                    new DiscountOffer()
                    {
                        Multiple = 3,
                        Value = 130
                    },
                    new DiscountOffer()
                    {
                        Multiple = 5,
                        Value = 200
                    }
                }
            };

            SpecificationOfEachItem['B'] = new ItemSpecification()
            {
                BasePrice = 30,
                OfferType = OfferType.Discount,
                DiscountOffers = new List<DiscountOffer> {
                    new DiscountOffer()
                    {
                        Multiple = 2,
                        Value = 45
                    }
                }
            };

            SpecificationOfEachItem['C'] = new ItemSpecification()
            {
                BasePrice = 20,
            };

            SpecificationOfEachItem['D'] = new ItemSpecification()
            {
                BasePrice = 15,
            };

            SpecificationOfEachItem['E'] = new ItemSpecification()
            {
                BasePrice = 40,
                OfferType = OfferType.FreebieOfDifferentItem,

                FreebieOffer = new FreebieOffer()
                {
                    Multiple = 2,
                    Recipient = 'B'
                }
            };

            SpecificationOfEachItem['F'] = new ItemSpecification()
            {
                BasePrice = 10,
                OfferType = OfferType.FreebieOfSameItem,
                FreebieOffer = new FreebieOffer()
                {
                    Multiple = 3,
                    Recipient = 'F'
                }
            };
            SpecificationOfEachItem['G'] = new ItemSpecification()
            {
                BasePrice = 20,
            };
            SpecificationOfEachItem['H'] = new ItemSpecification()
            {
                BasePrice = 10,
                DiscountOffers = new List<DiscountOffer> {
                    new DiscountOffer()
                    {
                        Multiple = 5,
                        Value = 45
                    },
                    new DiscountOffer(){
                        Multiple = 10,
                        Value = 80
                    }
                },
                OfferType = OfferType.MultipleDiscounts
            };
            SpecificationOfEachItem['I'] = new ItemSpecification()
            {
                BasePrice = 35,
            };
            SpecificationOfEachItem['J'] = new ItemSpecification()
            {
                BasePrice = 60,
            };
            SpecificationOfEachItem['K'] = new ItemSpecification()
            {
                BasePrice = 80,
                DiscountOffers = new List<DiscountOffer> {
                    new DiscountOffer()
                    {
                        Multiple = 2,
                        Value = 150
                    }
                },
                OfferType = OfferType.Discount
            };
            SpecificationOfEachItem['L'] = new ItemSpecification()
            {
                BasePrice = 90,
            };
            SpecificationOfEachItem['M'] = new ItemSpecification()
            {
                BasePrice = 15,
            };
            SpecificationOfEachItem['N'] = new ItemSpecification()
            {
                BasePrice = 40,
                FreebieOffer = new FreebieOffer()
                {
                    Multiple = 4,
                    Recipient = 'M'
                },
                OfferType = OfferType.FreebieOfDifferentItem
            };

            SpecificationOfEachItem['O'] = new ItemSpecification()
            {
                BasePrice = 10,
            };
            SpecificationOfEachItem['P'] = new ItemSpecification()
            {
                BasePrice = 50,
                DiscountOffers = new List<DiscountOffer>()
                {
                    new DiscountOffer()
                    {
                        Multiple = 5,
                        Value = 200
                    }
                },
                OfferType = OfferType.Discount
            };
            SpecificationOfEachItem['Q'] = new ItemSpecification()
            {
                BasePrice = 30,
                DiscountOffers = new List<DiscountOffer>()
                {
                    new DiscountOffer()
                    {
                        Multiple = 3,
                        Value = 80
                    }
                },
                OfferType = OfferType.Discount
            };
            SpecificationOfEachItem['R'] = new ItemSpecification()
            {
                BasePrice = 50,
                FreebieOffer = new FreebieOffer()
                {
                    Multiple = 4,
                    Recipient = 'Q'
                },
                OfferType = OfferType.FreebieOfDifferentItem
            };
            SpecificationOfEachItem['S'] = new ItemSpecification()
            {
                BasePrice = 30,
            };
            SpecificationOfEachItem['T'] = new ItemSpecification()
            {
                BasePrice = 20,
            };
            SpecificationOfEachItem['U'] = new ItemSpecification()
            {
                BasePrice = 40,
                FreebieOffer = new FreebieOffer()
                {
                    Multiple = 4,
                    Recipient = 'U'
                },
                OfferType = OfferType.FreebieOfSameItem
            };
            SpecificationOfEachItem['V'] = new ItemSpecification()
            {
                BasePrice = 50,
                DiscountOffers = new List<DiscountOffer> {
                    new DiscountOffer()
                    {
                        Multiple = 2,
                        Value = 90
                    },
                    new DiscountOffer(){
                        Multiple = 3,
                        Value = 130
                    }
                },
                OfferType = OfferType.MultipleDiscounts

            };
            SpecificationOfEachItem['W'] = new ItemSpecification()
            {
                BasePrice = 20,
            };
            SpecificationOfEachItem['X'] = new ItemSpecification()
            {
                BasePrice = 90,
            };
            SpecificationOfEachItem['Y'] = new ItemSpecification()
            {
                BasePrice = 10,
            };
            SpecificationOfEachItem['Z'] = new ItemSpecification()
            {
                BasePrice = 50,
            };

            return SpecificationOfEachItem;
        }


    }
}


