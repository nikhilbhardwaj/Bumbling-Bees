using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace BumbleBeeApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        public static int pointsEarned(string str)  // method to calculate score
        {
            //Extremely Rare = {x z j q} = 10
            //Rare = {g b f y w k v} = 7
            //Neutral = {s l c u d p m h} = 5
            //and Abundant = {e a r i o t n} = 3
            //points_earned = (length_of_word) * ( sum_of_points_of_all_the_alphabets_used)
            str = str.ToUpper();
            int points = 0;
            byte lenWord;
            byte asciiEq;
            byte bonus = 0;
            char alphStr;


            lenWord = (byte)str.Length;
            byte[] arrBonus = new byte[lenWord];

            for (byte i = 0; i < lenWord; i++)
            {
                alphStr = str[i];                 //breaking down the string
                asciiEq = (byte)alphStr;



                if (asciiEq == 88 || asciiEq == 90 || asciiEq == 74 || asciiEq == 81)
                {
                    //10
                    points += 10;
                }
                else if (asciiEq == 66 || asciiEq == 70 || asciiEq == 71 || asciiEq == 75 || asciiEq == 86 ||
                            asciiEq == 87 || asciiEq == 89)
                {
                    //7
                    points += 7;

                }
                else if (asciiEq == 65 || asciiEq == 67 || asciiEq == 72 || asciiEq == 76 || asciiEq == 77 ||
                     asciiEq == 80 || asciiEq == 83 || asciiEq == 85)
                {
                    //5
                    points += 5;
                }
                else
                {
                    //3
                    points += 3;
                }

                //bonus for every repeated occurence 20 points
                //not exactly how i planned but will do for us

                if ((Array.IndexOf(arrBonus, asciiEq)) != -1)
                {
                    bonus += 20;

                }
                arrBonus[i] = asciiEq;

                //the message box shows th working here
                MessageBox.Show(str[i].ToString() + "   " + points.ToString());
            }
            // shows the total
            MessageBox.Show("bonus : " + bonus.ToString());
            MessageBox.Show("total : " + (points * lenWord + bonus).ToString());


            //returning the score
            return points * lenWord + bonus;
        }
    }
}