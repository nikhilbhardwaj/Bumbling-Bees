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
        public static int[,] hiveIndices = new int[,] {
                                        {200,362},{250,370},{213,410} , {400,330} , {380,282} , {437, 295} , {325, 405} , {379,413} ,{343,453} , 
                                        {450,382},{510,395},{468,430} , {585,330} , {645,340} , {603,375}
        };

        //i m putting the coordinates for the honey pots here as well
        public static int[,] honeyPot = new int[,]{
                                        {5,450}, {65,450} , {125, 450} , {185,450} , {245, 450} , {305, 450} ,
                                        {365, 450} , {425, 450} , {485, 450} , {545, 450}
        };

        public Alphabet(char randomChar)
        {
            CurrentAlphabet = randomChar;
            _img = new Image();
            _img.Tag = CurrentAlphabet;
            ImageSourceConverter convertor = new ImageSourceConverter();
            _img.Source = (ImageSource)convertor.ConvertFromString(basePath + randomChar + ".png");
            _img.Width = _img.Height = 35;
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
