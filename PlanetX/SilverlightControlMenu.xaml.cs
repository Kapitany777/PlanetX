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
using PlanetX.Classes;

namespace PlanetX
{
    public partial class SilverlightControlMenu : UserControl
    {
        public Difficulty GameDifficulty { get; set; }

        private MouseButtonEventHandler eventNewGame;

        public MouseButtonEventHandler EventNewGame
        {
            get { return eventNewGame; }

            set
            {
                eventNewGame = value;
                MenuNewGame.MouseLeftButtonDown += value;
            }
        }

        private MouseButtonEventHandler eventReturnToGame;

        public MouseButtonEventHandler EventReturnToGame
        {
            get { return eventReturnToGame; }

            set
            {
                eventReturnToGame = value;
                MenuReturnToGame.MouseLeftButtonDown += value;
            }
        }

        public SilverlightControlMenu()
        {
            InitializeComponent();

            GameDifficulty = Difficulty.Easy;
        }

        private void StoryboardMenuHide_Completed(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        public void SetDifficultyColors()
        {
            MenuEasy.Foreground = new SolidColorBrush(Colors.White);
            MenuNormal.Foreground = new SolidColorBrush(Colors.White);
            MenuHard.Foreground = new SolidColorBrush(Colors.White);

            switch (GameDifficulty)
            {
                case Difficulty.Easy:
                    MenuEasy.Foreground = new SolidColorBrush(Colors.Yellow);
                    break;

                case Difficulty.Normal:
                    MenuNormal.Foreground = new SolidColorBrush(Colors.Yellow);
                    break;

                case Difficulty.Hard:
                    MenuHard.Foreground = new SolidColorBrush(Colors.Yellow);
                    break;
            }
        }

        private void MenuEasy_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GameDifficulty = Difficulty.Easy;

            EventNewGame(sender, e);
        }

        private void MenuNormal_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GameDifficulty = Difficulty.Normal;

            EventNewGame(sender, e);
        }

        private void MenuHard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GameDifficulty = Difficulty.Hard;

            EventNewGame(sender, e);
        }
    }
}
