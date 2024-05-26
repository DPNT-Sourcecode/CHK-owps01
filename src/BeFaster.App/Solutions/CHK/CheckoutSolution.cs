﻿using BeFaster.Runner.Exceptions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BeFaster.App.Solutions.CHK
{
    public static class CheckoutSolution
    {
        public static int ComputePrice(string skus)
        {
            Dictionary<char, ItemSpecification> SpecificationOfEachItem = GetSpecificationOfEachItem();
            Dictionary<char, int> CountOfEachItem = GetCountOfEachItem(skus);

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

        private static int GetItemPrice(ItemSpecification itemSpecification, int itemCount)
        {
            int itemPrice = 0;
            switch (itemSpecification.OfferType)
            {
                case OfferType.Discount:
                    itemPrice = GetItemDiscountPrice(itemSpecification, itemCount);
                    break;
                //case OfferType.FreebieOfSameItem:
                //    // Do Something
                //    break;
                //case OfferType.FreebieOfDifferentItem:
                //    // Do Something
                //    break;
                //default:
                //    // Do Something
                //    break;
            }
            return itemPrice;
        }

        private static int GetItemDiscountPrice(ItemSpecification itemSpecification, int itemCount)
        {
            int Remainder = itemCount % itemSpecification.OfferMultiple;
            if (Remainder == 0)
            {
                return itemCount * itemSpecification.OfferMultiple;
            }
            else
            {
                int OfferPrice = (itemCount - Remainder) * itemSpecification.OfferMultiple;
                int remainderPrice = Remainder * itemSpecification.BasePrice;
                return OfferPrice + remainderPrice;
            }
        }

        private static Dictionary<char, int> GetCountOfEachItem(string skus)
        {
            Dictionary<char, int> CountsOfEachItem = new Dictionary<char, int>();

            char[] ValidCharacters = { 'A', 'B', 'C', 'D', 'E', 'F' };

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
                OfferMultiple = 3,
                OfferMultiple2 = 5
            };
            SpecificationOfEachItem['B'] = new ItemSpecification()
            {
                BasePrice = 30,
                OfferType = OfferType.Discount,
                OfferMultiple = 2
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
                OfferMultiple = 2
            };
            SpecificationOfEachItem['F'] = new ItemSpecification()
            {
                BasePrice = 10,
                OfferType = OfferType.FreebieOfSameItem,
                OfferMultiple = 2,
            };

            return SpecificationOfEachItem;
        }

        //    int a_count = 0;
        //    int a_multiples_of_5 = 0;

        //    int b_count = 0;
        //    int b_multiples_2 = 0;

        //    int c_count = 0;
        //    int d_count = 0;
        //    int e_count = 0;
        //    int f_count = 0;
        //    int f_skip_counter = 0;

        //    foreach (char c in skus)
        //    {
        //        if (c == 'A')
        //        {
        //            IncrementA(ref a_count, ref a_multiples_of_5);
        //        }
        //        else if (c == 'B')
        //        {
        //            IncrementB(ref b_count, ref b_multiples_2);
        //        }
        //        else if (c == 'C')
        //        {
        //            c_count++;
        //        }
        //        else if (c == 'D')
        //        {
        //            d_count++;
        //        }
        //        else if (c == 'E')
        //        {
        //            e_count++;
        //            if (e_count % 2 == 0)
        //            {
        //                DecrementB(ref b_count, ref b_multiples_2);
        //            }
        //        }
        //        else if (c == 'F')
        //        {
        //            f_count++;
        //            if (f_count % 3 == 0)
        //            {
        //                f_skip_counter++;
        //            }
        //        }
        //        else
        //        {
        //            return -1;
        //        }
        //    }

        //    int remaining_a_count = a_count - (a_multiples_of_5 * 5);
        //    int three_multiple_price = 0;

        //    if (remaining_a_count >= 3)
        //    {
        //        remaining_a_count -= 3;
        //        three_multiple_price += 130;
        //    }

        //    int a_price = (a_multiples_of_5 * 200) + remaining_a_count * 50 + three_multiple_price;

        //    int remaining_b_count = b_count - (b_multiples_2 * 2);
        //    int b_price = (b_multiples_2 * 45) + remaining_b_count * 30;

        //    if (b_price < 0)
        //    {
        //        b_price = 0;
        //    }

        //    return ((f_count - f_skip_counter) * 10) + (e_count * 40) + (d_count * 15) + (c_count * 20) + b_price + a_price;
        //}

        ////This solution is terrible, I need to refactor this
        //private static void IncrementB(ref int b_count, ref int b_multiples_2)
        //{
        //    b_count++;
        //    if (b_count % 2 == 0 && b_count != 0)
        //    {
        //        b_multiples_2++;
        //    }
        //}

        //private static void DecrementB(ref int b_count, ref int b_multiples_2)
        //{
        //    if (b_count % 2 == 0 && b_count != 0)
        //    {
        //        b_multiples_2--;
        //    }
        //    b_count--;
        //}
        //private static void IncrementA(ref int a_count, ref int a_multiples_of_5)
        //{
        //    a_count++;
        //    if (a_count % 5 == 0)
        //    {
        //        a_multiples_of_5++;
        //    }
        //}
    }
}



