using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.IO.IsolatedStorage;

namespace BumbleBeeApp
{
    //This class will contain all the word generation and validation methods
    //No objects would be instantiated and all methods should be static
    public class BumbleDictionary
    {
        //Method to examine if the given word is a dictionary word
        //Uses the IsolatedStorage for reading the file
        public static bool IsValidWord(string word)
        {
            IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile("raw_dictionary.txt", FileMode.Open, FileAccess.Read);
            string line;
            using (StreamReader reader = new StreamReader(fileStream))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Equals(word, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //Methods to generate random alphabets
        public static char RandomAlphabetGenerator(byte numberOfAlphabets = 127)   //this will generate any number of random alphabets
        {
            byte b = 0, c;
            byte[] arr = new byte[3];
            int i = 0;
            string set = ""; 

            // it can be used to generate one or many at the same time 
            // taking into consideration that the same algorithm will be used for generating,
            // the first set of alphabets during the start of game, which may be any number
            // and as our game generates a set of alphabets to replace the ones used for making the previous word, thought it 'll be better 
            // to get the string in one go and then break it into pieces to get them individually later


            c = AsciiGen();

            while (b < numberOfAlphabets)
            {

                // following conditional check ensures that repeatation of alphabets is controlled
                // for eg. if once A is generated it 'll not allow A in next three generations
                // though after that it can come again

                if ((Array.IndexOf(arr, c)) < 0)
                {
                    arr[i] = c;
                    set += (char)arr[i];
                    i++;
                    b++;
                }
                else
                {
                    c = AsciiGen();
                }

                if (i >= 3)
                {
                    i = 0;
                }
            }
            //Modified to return only one character
            return set[64];
        }

        private static byte AsciiGen()
        {
            int i = new Random().Next(99);   //generates a random +ve integer less than 99

            //based on cumilitive frequencies it will select and
            //returns a byte value which is the ascii equivalent of the alphabet
            if (i >= 0 && i <= 9)
                return 65;
            else if (i > 9 && i <= 11)
                return 66;
            else if (i > 11 && i <= 13)
                return 67;
            else if (i > 13 && i <= 17)
                return 68;
            else if (i > 17 && i <= 29)
                return 69;
            else if (i > 29 && i <= 31)
                return 70;
            else if (i > 31 && i <= 34)
                return 71;
            else if (i > 34 && i <= 36)
                return 72;
            else if (i > 36 && i <= 45)
                return 73;
            else if (i > 45 && i <= 46)
                return 74;
            else if (i > 46 && i <= 47)
                return 75;
            else if (i > 47 && i <= 51)
                return 76;
            else if (i > 51 && i <= 53)
                return 77;
            else if (i > 53 && i <= 59)
                return 78;
            else if (i > 59 && i <= 67)
                return 79;
            else if (i > 67 && i <= 69)
                return 80;
            else if (i > 69 && i <= 70)
                return 81;
            else if (i > 70 && i <= 76)
                return 82;
            else if (i > 76 && i <= 80)
                return 83;
            else if (i > 80 && i <= 86)
                return 84;
            else if (i > 86 && i <= 90)
                return 85;
            else if (i > 90 && i <= 92)
                return 86;
            else if (i > 92 && i <= 94)
                return 87;
            else if (i > 94 && i <= 95)
                return 88;
            else if (i > 95 && i <= 97)
                return 89;
            else              //i == 98
                return 90;
        }

        //Methods to compute the word score should follow
    }
}
