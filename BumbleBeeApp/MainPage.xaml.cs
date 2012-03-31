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
using System.Threading;


namespace BumbleBeeApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        BumbleGame theGame;
        // Constructor
        //coordinates initital and final
        private double xInt, yInt, xFin, yFin;
        //counter for placing the alphabets
        byte potNumber = 0;

        public MainPage()
        {
            InitializeComponent();
            InitializeGui();

        }

        //to set up the initial user interface
        public void InitializeGui()
        {
            // TODO get the users name and place it in the appropriate place MessageBox.Show("Enter Your Name");
            theGame = new BumbleGame("Nikhil");
            //To correctly place elements in the hives
            for (int i = 0; i < 15; ++i)
            {
                    Alphabet alpha = new Alphabet(BumbleDictionary.RandomAlphabetGenerator());
                    theGame.wordCloud.Add(alpha);
                    PlaceAlphabetOnScreen(ref alpha._img, Alphabet.hiveIndices, i);
            }
        }

        public void PlaceAlphabetOnScreen(ref Image alpha, int[,] lookUp, int hiveIndex)
        {
            //setup the action listeners
            var gestureListener = GestureService.GetGestureListener(alpha);
            gestureListener.DragDelta += OnDragDelta;
            gestureListener.GestureCompleted += OnGestureCompleted;
            gestureListener.GestureBegin += OnGestureBegin;
            ContentPanel.Children.Add(alpha);
            //setup the animation
            Storyboard sb = CreateAndApplyStoryboard(alpha, lookUp, hiveIndex);
            sb.Begin();
        }

        //Places the alphabet appropriately in either the hives or honey pots
        public Storyboard CreateAndApplyStoryboard(UIElement targetElement, int [,] lookUp,int elementIndex)
        {
            Storyboard sb = new Storyboard();

            DoubleAnimation animationX = new DoubleAnimation
            {
                From = 0,
                To = lookUp[elementIndex,0],
                Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                EasingFunction = new ExponentialEase()
            };

            DoubleAnimation animationY = new DoubleAnimation
            {
                From = 0,
                To = lookUp[elementIndex,1],
                Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                EasingFunction = new ExponentialEase()
            };

            Storyboard.SetTarget(animationX, targetElement);
            Storyboard.SetTarget(animationY, targetElement);

            Storyboard.SetTargetProperty(animationX, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
            Storyboard.SetTargetProperty(animationY, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

            sb.Children.Add(animationX);
            sb.Children.Add(animationY);
            return sb;
        }

        private void OnGestureBegin(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {
            //  Image image = sender as Image;
            //taking initial coordinates
            xInt = e.GetPosition(null).X;
            yInt = e.GetPosition(null).Y;

        }

        private void OnDragDelta(object sender, DragDeltaGestureEventArgs e)
        {
            Image image = sender as Image;
            TranslateTransform transform = image.RenderTransform as TranslateTransform;

            // Move the image
            transform.X += e.HorizontalChange;
            transform.Y += e.VerticalChange;

            //final coordinates
            xFin = e.GetPosition(null).X;
            yFin = e.GetPosition(null).Y;

        }

        private void OnGestureCompleted(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {
            Image image = sender as Image;
            TranslateTransform transform = image.RenderTransform as TranslateTransform;
            
            //printing coordinates
            //MessageBox.Show((xInt.ToString()) + "\t" + (yInt.ToString()));

           
            // Move the image
            
            //if the image was not moved to pot zone
            //but it was left some where in the cloud
            //((e.GetPosition(null).Y) != 480) it rectifies the problem of double click
            //
            if (((e.GetPosition(null).X) < 640) && ((e.GetPosition(null).Y) > 385) && ((e.GetPosition(null).Y) != 480) )
            {
                //returns the value of the alphabet
                //place it on Honey Pot
                theGame.userWord.Add(image);
                //I'm working on this part
                //still trying to refine it some more...

                if (!(xInt < 640 && yInt > 385))
                {
                    transform.X -= (xFin - 30);
                    transform.X += Alphabet.honeyPot[potNumber, 0];
                    transform.Y -= (yFin - 20);
                    transform.Y += Alphabet.honeyPot[potNumber, 1];
                    potNumber++;
                }
                else
                {
                    if (xFin < xInt)
                    {
                        transform.X += -(xFin - xInt + 30);
                    }
                    if (xFin > xInt)
                    {
                        transform.X += -(xFin - xInt - 30);
                    }
                    if (yFin < yInt)
                    {
                        transform.Y += -(yFin - yInt + 30);
                    }
                    if (yFin > yInt)
                    {
                        transform.Y += -(yFin - yInt - 30);
                    }
                }

            }
            else
            {
                //TODO return it back to its original position

                //to be done in two parts
                // 1) for X-axis
                // 2) for Y-axis

                //X-axis
                //if the move along X axis is greater than 32(considering the size of the image is 35) pixels
                //((e.GetPosition(null).Y) != 480) it rectifies the problem of double click
                if (Math.Abs((xFin - xInt)) >= 32 && ((e.GetPosition(null).Y) != 480) )
                {
                    if (xFin < xInt)
                    {
                        transform.X += -(xFin - xInt + 30 );
                    }
                    if (xFin > xInt)
                    {
                        transform.X += -(xFin - xInt - 30 );
                    }
                }
                else if ((xFin - xInt) == 0)
                { 
                    //DONOTHING
                }

                //Y-axis
                //if the move along Y axis is greater than 32 pixels
                //((e.GetPosition(null).Y) != 480) it rectifies the problem of double click
                if (Math.Abs((yFin - yInt)) >= 32 && ((e.GetPosition(null).Y) != 480))
                {
                    if (yFin < yInt)
                    {
                        transform.Y += -(yFin - yInt + 25);
                    }
                    if (yFin > yInt)
                    {
                        transform.Y += -(yFin - yInt - 20);
                    }
                }
                else if ((yFin - yInt) == 0)
                {
                    //DONOTHING
                }

            }

            
        }

        private void image25_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string wordToCheck = theGame.GetUserWord();
            
            if (BumbleDictionary.IsValidWord(wordToCheck))
            {
                theGame.UserScore += BumbleDictionary.WordScore(wordToCheck);
                MessageBox.Show(wordToCheck + " is a valid word with a score of " + BumbleDictionary.WordScore(wordToCheck));
            }
            else
            {
                MessageBox.Show(wordToCheck + " isn't valid");
                foreach (Image alpha in theGame.userWord)
                {
                    //Deletes the imags from the screen
                    alpha.Source = null;
                }
                theGame.userWord.Clear();
            }
        }
    }
}