using BeFaster.Runner.Exceptions;
using System.Collections.Generic;

namespace BeFaster.App.Solutions.CHK
{
    public static class CheckoutSolution
    {
        public static int ComputePrice(string skus)
        {
            Dictionary<char, int> checkoutPrices = new Dictionary<char, int>();
            checkoutPrices['A'] = 50;
            checkoutPrices['B'] = 30;
            checkoutPrices['C'] = 20;
            checkoutPrices['D'] = 15;

            int a_count = 0;
            int a_multipules_of_3 = 0;
            int b_count = 0;
            int b_multipules_2 = 0;
            int c_count = 0;
            int d_count = 0;

            foreach (char c in skus)
            {
                if (c == 'A')
                {
                    a_count++;
                    if (a_count == 3)
                    {
                        a_multipules_of_3++;
                    }
                }
                if (c == 'B')
                {
                    b_count++;
                    if (b_count == 3)
                    {
                        b_multplies_of_2++;
                    }

                }
                if (c == 'C')
                {
                    c_count++;
                }
                if (c == 'D')
                {
                    d_count++;
                }
            }

            int a_price =
            int total_price = 0;


        }
    }
}
