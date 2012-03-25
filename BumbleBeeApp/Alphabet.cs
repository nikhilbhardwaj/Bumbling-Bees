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
        static string basePath = @"Images\";
        //line below gives some error
        //static const string basePath = @"Images\Letter "; 

        public Alphabet(char randomChar)
        {
            CurrentAlphabet = randomChar;
            _img = new Image();
            ImageSourceConverter convertor = new ImageSourceConverter();
            _img.Source = (ImageSource)convertor.ConvertFromString(basePath + randomChar + ".png");
            _img.Width = _img.Height = 50;
            //TODO Change the default postition
            _img.Margin = new Thickness(23, 17, 509, 291);
            _img.RenderTransform = new TranslateTransform();
            
        }

        public char CurrentAlphabet { get; set; }
        //More properties will be added when I can think of something useful, do add something you see fit
    }
}
