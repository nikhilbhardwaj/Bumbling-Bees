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
using System.ComponentModel;

namespace BumbleBeeApp
{
    public class BumbleGame : INotifyPropertyChanged
    {
        public List<Alphabet> wordCloud;
        public List<Image> userWord;
        private int _userScore;
        public int CurrentLevel { get; set; }
        public string UserScore {
            get
            {
                return _userScore.ToString();
            }
            set
            {
                //Implementation for the Changed notification
                _userScore = Convert.ToInt32(value);
                NotifyPropertyChanged("UserScore");
            }
        }
        public string UserName;

        //returns the word that the user has generated
        public string GetUserWord()
        {
            //Should be replaced with some equivalent of the stringbuffer for c#
            string tmpWord = "";
            foreach(Image alpha in userWord)
            {
                tmpWord += alpha.Tag.ToString();
            }
            return tmpWord;
        }

        public void IncrementScore(int val)
        {
            _userScore += val;
            NotifyPropertyChanged("UserScore");
        }
        //Default constructor to create a new instance of the game
        public BumbleGame(string username)
        {
            wordCloud = new List<Alphabet>(15);
            userWord = new List<Image>();
            CurrentLevel = 1;
            UserScore = "0";
            UserName = username;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
