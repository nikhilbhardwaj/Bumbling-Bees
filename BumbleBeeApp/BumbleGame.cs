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
using System.Collections.Generic;

namespace BumbleBeeApp
{
    public class BumbleGame
    {
        private List<Alphabet> wordCloud, userWord;
        public int CurrentLevel { get; set; }
        public int UserScore { get; set; }
        public string UserName;

        //returns the word that the user has generated
        public string GetUserWord()
        {
            //Should be replaced with some equivalent of the stringbuffer for c#
            string tmpWord = "";
            foreach(Alphabet alpha in userWord)
            {
                tmpWord += alpha.CurrentAlphabet;
            }
            return tmpWord;
        }

        //Default constructor to create a new instance of the game
        public BumbleGame(string username)
        {
            wordCloud = new List<Alphabet>(15);
            userWord = new List<Alphabet>(4);
            CurrentLevel = 1;
            UserScore = 0;
            UserName = username;
        }
    }
}
