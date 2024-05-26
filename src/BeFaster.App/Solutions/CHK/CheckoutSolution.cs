using BeFaster.Runner.Exceptions;
using System.Collections.Generic;

namespace BeFaster.App.Solutions.CHK
{
    public static class CheckoutSolution
    {
        public static int ComputePrice(string skus)
        {
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
                    if (a_count % 3 == 0)
                    {
                        a_multipules_of_3++;
                    }
                }
                else if (c == 'B')
                {
                    b_count++;
                    if (b_count % 2 == 0)
                    {
                        b_multipules_2++;
                    }
                }
                else if (c == 'C')
                {
                    c_count++;
                }
                else if (c == 'D')
                {
                    d_count++;
                }
                else
                {
                    return -1;
                }
            }

            int remaining_a_count = a_count - (a_multipules_of_3 * 3);
            int a_price = (a_multipules_of_3 * 130) + remaining_a_count * 50;

            int remaining_b_count = b_count - (b_multipules_2 * 2);
            int b_price = (b_multipules_2 * 45) + remaining_b_count * 30;

            return (d_count * 15) + (c_count * 20) + b_price + a_price;
        }
    }
}


