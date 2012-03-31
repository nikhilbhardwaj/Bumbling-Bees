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
        public MainPage()
        {
            InitializeComponent();
            InitializeGui();
        }

        //to set up the initial user interface
        public void InitializeGui()
        {
            MessageBox.Show("Enter Your Name");
            theGame = new BumbleGame("Nikhil");
            //To correctly place elements in the hives
            for (int i = 0; i < 15; ++i)
            {
                    Alphabet alpha = new Alphabet(BumbleDictionary.RandomAlphabetGenerator());
                    theGame.wordCloud.Add(alpha);
                    PlaceAlphabetOnScreen(ref alpha, i);
                    Thread.Sleep(1000);
            }
        }

        public void PlaceAlphabetOnScreen(ref Alphabet alpha, int hiveIndex)
        {
            //setup the action listeners
            var gestureListener = GestureService.GetGestureListener(alpha._img);
            gestureListener.DragDelta += OnDragDelta;
            gestureListener.GestureCompleted += OnGestureCompleted;
            gestureListener.GestureBegin += OnGestureBegin;
            ContentPanel.Children.Add(alpha._img); 
            //setup the animation
            Storyboard sb = CreateAndApplyStoryboard(alpha._img, hiveIndex);
            sb.Begin();
        }

        //Places the alphabet appropriately in either the hive or the honey pots
        public Storyboard CreateAndApplyStoryboard(UIElement targetElement, int elementIndex)
        {
            Storyboard sb = new Storyboard();

            DoubleAnimation animationX = new DoubleAnimation
            {
                From = 0,
                To = Alphabet.HiveXCoordinate(elementIndex),
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                EasingFunction = new ExponentialEase()
            };

            DoubleAnimation animationY = new DoubleAnimation
            {
                From = 0,
                To = Alphabet.HiveYCoordinate(elementIndex),
                Duration = new Duration(TimeSpan.FromSeconds(1)),
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
        }

        private void OnDragDelta(object sender, DragDeltaGestureEventArgs e)
        {
            Image image = sender as Image;
            TranslateTransform transform = image.RenderTransform as TranslateTransform;

            // Move the image
            transform.X += e.HorizontalChange;
            transform.Y += e.VerticalChange;
        }

        private void OnGestureCompleted(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {
            Image image = sender as Image;
            TranslateTransform transform = image.RenderTransform as TranslateTransform;

        }
    }
}