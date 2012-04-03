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
using Coding4Fun.Phone.Controls;


namespace BumbleBeeApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        BumbleGame theGame;
        // Constructor
        //coordinates initital and final
        private double xInt, yInt, xFin, yFin,xLast;
        //private double yLast;
        //counter for placing the alphabets
        byte potNumber = 0;
        int index = 0;
        List<int> lstIndices = new List<int>();

        public MainPage()
        {
            InitializeComponent();
            InitializeGui();
            DataContext = theGame;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        //Called when the main page is loaded
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //To get the user name
            InputPrompt input = new InputPrompt();
            input.Completed += input_Completed;
            input.Title = "Who are you Player?";
            input.Message = "Please enter your name to continue.";
            input.Show();
        }
        //to set up the initial user interface
        public void InitializeGui()
        {
            // TODO get the users name and place it in the appropriate place MessageBox.Show("Enter Your Name");
            theGame = new BumbleGame("?");
            //To correctly place elements in the hives
            for (int i = 0; i < 15; ++i)
            {
                Alphabet alpha = new Alphabet(BumbleDictionary.RandomAlphabetGenerator());
                theGame.wordCloud.Add(alpha);
                PlaceAlphabetOnScreen(ref alpha._img, Alphabet.hiveIndices, i, 1);
            }
        }

        public void PlaceAlphabetOnScreen(ref Image alpha, int[,] lookUp, int hiveIndex, int millis)
        {
            //setup the action listeners
            var gestureListener = GestureService.GetGestureListener(alpha);
            gestureListener.DragDelta += OnDragDelta;
            gestureListener.GestureCompleted += OnGestureCompleted;
            gestureListener.GestureBegin += OnGestureBegin;
            ContentPanel.Children.Add(alpha);
            //setup the animation
            Storyboard sb = CreateAndApplyStoryboard(alpha, lookUp, hiveIndex, millis);
            sb.Begin();
        }

        //Places the alphabet appropriately in either the hives or honey pots
        public Storyboard CreateAndApplyStoryboard(UIElement targetElement, int[,] lookUp, int elementIndex, int millis)
        {
            Storyboard sb = new Storyboard();

            DoubleAnimation animationX = new DoubleAnimation
            {
                From = 0,
                To = lookUp[elementIndex, 0],
                Duration = new Duration(TimeSpan.FromMilliseconds(millis)),
                EasingFunction = new ExponentialEase()
            };

            DoubleAnimation animationY = new DoubleAnimation
            {
                From = 0,
                To = lookUp[elementIndex, 1],
                Duration = new Duration(TimeSpan.FromMilliseconds(millis)),
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

        //The gesture handling events
        private void OnGestureBegin(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {
            //  Image image = sender as Image;
            //taking initial coordinates
            xInt = e.GetPosition(null).X;
            yInt = e.GetPosition(null).Y;
            index = GetIndex(xInt, yInt);
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

            // Move the image
            //if the image was not moved to pot zone
            //but it was left some where in the cloud
            //((e.GetPosition(null).Y) != 480) it rectifies the problem of double click
            //MessageBox.Show((e.GetPosition(null).Y > 385).ToString());

            if (e.GetPosition(null).X != xLast)
            {
                if (((e.GetPosition(null).X) < 640) && ((e.GetPosition(null).Y) > 385) && ((e.GetPosition(null).Y) != 480))
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

                        //added to list only if its a valid drag
                        lstIndices.Add(index);
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
                    if (Math.Abs((xFin - xInt)) >= 32 && ((e.GetPosition(null).Y) != 480))
                    {
                        if (xFin < xInt)
                        {
                            transform.X += -(xFin - xInt + 30);
                        }
                        if (xFin > xInt)
                        {
                            transform.X += -(xFin - xInt - 30);
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

                xLast = xFin;
                //yLast = yFin;
            }
        }

        private void image25_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string wordToCheck = theGame.GetUserWord();

            if (wordToCheck.Length >= 3 && BumbleDictionary.IsValidWord(wordToCheck))
            {
                //show tha animation
                this.right.Begin();
                //Increment the score on the screen
                theGame.UserScore += BumbleDictionary.WordScore(wordToCheck);
                //To delete the alphabets from the screen and generate the new ones
                foreach (Image alpha in theGame.userWord)
                {
                    //Create and Send new alphabet to the hives
                    Alphabet tmpAlpha = new Alphabet(BumbleDictionary.RandomAlphabetGenerator());
                    PlaceAlphabetOnScreen(ref tmpAlpha._img, Alphabet.hiveIndices, lstIndices.First(), 1000);
                    //Deletes the imags from the screen
                    alpha.Source = null;
                    lstIndices.RemoveAt(0);
                    --potNumber;
                }
                theGame.userWord.Clear();
            }
            else
            {
                //If we're here then the word entered by the user isn't valid
                foreach (Image alpha in theGame.userWord)
                {
                    //show the animation
                    this.wrong.Begin();
                    //Send the alphabet to the hives again
                    Alphabet tmpAlpha = new Alphabet(Convert.ToChar(alpha.Tag));
                    PlaceAlphabetOnScreen(ref tmpAlpha._img, Alphabet.hiveIndices, lstIndices.First(), 1000);
                    //Deletes the imags from the screen
                    alpha.Source = null;
                    lstIndices.RemoveAt(0);
                    --potNumber;
                }
                theGame.userWord.Clear();
            }
        }

        //Method to find the index of the empty hives
        private int GetIndex(double x, double y)
        {

            //{200,362},{250,370},{213,410} , {400,330} , {380,282} , {437, 295} , {325, 405} , {379,413} ,{343,453} , 
            //{450,382},{510,395},{468,430} , {585,330} , {645,340} , {603,375}



            //the indices with their locations are
            //size of indice  width = 60 , && height = 50

            //order changed to match with indices of alphabet.cs hives

            if (x >= 144 && x < 204 && y >= 180 && y < 230)
            {
                return 0;
            }// 0 (144,180)
            else if (x >= 205 && x < 245 && y >= 180 && y < 230)
            {
                return 1;
            }// 1 (205,180)
            else if (x >= 174 && x < 234 && y >= 230 && y < 280)
            {
                return 2;
            }// 2 (174,230)
            else if (x >= 272 && x < 332 && y >= 223 && y < 273)
            {
                return 6;
            }// 3 (272,223) changed to 6
            else if (x >= 332 && x < 392 && y >= 223 && y < 273)
            {
                return 7;
            }// 4 (332,223) changed to 7
            else if (x >= 302 && x < 363 && y >= 273 && y < 323)
            {
                return 8;
            }// 5 (302,273) changed to 8
            else if (x >= 329 && x < 389 && y >= 104 && y < 154)
            {
                return 4;
            }// 6 (329,104) changed to 4
            else if (x >= 389 && x < 449 && y >= 104 && y < 154)
            {
                return 5;
            }// 7 (389,104) changed to 5
            else if (x >= 359 && x < 419 && y >= 154 && y < 205)
            {
                return 3;
            }// 8 (359,154)  changed to 3
            else if (x >= 372 && x < 432 && y >= 201 && y < 251)
            {
                return 9;
            }// 9 (372,201)
            else if (x >= 432 && x < 492 && y >= 201 && y < 251)
            {
                return 10;
            }// 10 (432,201)
            else if (x >= 402 && x < 462 && y >= 251 && y < 302)
            {
                return 11;
            }// 11 (402,251)
            else if (x >= 504 && x < 564 && y >= 148 && y < 198)
            {
                return 12;
            }// 12 (504,148)
            else if (x >= 564 && x < 624 && y >= 148 && y < 198)
            {
                return 13;
            }// 13 (564,148)
            else if (x >= 534 && x < 594 && y >= 198 && y < 249)
            {
                return 14;
            }// 14 (534,198)
            else
                return -99;
        }


        private void image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            // TODO: Add event handler implementation here. I am sorry bout this one, it got
            //accidentally auto-generated
        }



        //Handling the input for the username
        private void input_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            //add some code here   
            InputPrompt input = sender as InputPrompt;
            theGame.UserName = input.Value;
            input.Visibility = System.Windows.Visibility.Collapsed;
        }

    }
}