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
using Microsoft.Phone.Controls;

namespace BumbleBeeApp
{
    //This contains an Image and knows about the alphabet it holds
    //So we can interact with it like an Image on the screen, i.e. Drag and Drop etc
    //and also programatically query it for information we need
    public class Alphabet
    {
        public Image _img;
        const string basePath = @"Images\Letter ";
        static int[,] hiveIndices = new int[,] {
                                        {200,200},{200,250},{200,200}
        };

        public Alphabet(char randomChar)
        {
            CurrentAlphabet = randomChar;
            _img = new Image();
            ImageSourceConverter convertor = new ImageSourceConverter();
            _img.Source = (ImageSource)convertor.ConvertFromString(basePath + randomChar + ".png");
            _img.Width = _img.Height = 50;
            //TODO Change the default postition
            _img.Margin = new Thickness(-200,-400,200,400);
            _img.RenderTransform = new TranslateTransform(); 
        }

        public char CurrentAlphabet { get; set; }
        //More properties will be added when I can think of something useful, do add something you see fit

        //These two methods are meant to place an element into position
        public static int HiveXCoordinate(int hiveIndex)
        {
            return hiveIndices[hiveIndex, 0];
        }

        public static int HiveYCoordinate(int hiveIndex)
        {
            return hiveIndices[hiveIndex, 1];
        }
    }
}
