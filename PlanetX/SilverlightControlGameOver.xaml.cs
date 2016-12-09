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
using System.Windows.Media.Imaging;
using PlanetX.Classes;

namespace PlanetX
{
    public partial class SilverlightControlGameOver : UserControl
    {
        private PlanetOwner winner;

        public PlanetOwner Winner
        {
            get { return winner; }
            
            set
            {
                if (value != winner)
                {
                    winner = value;

                    switch (winner)
                    {
                        case PlanetOwner.Player:
                            TextBlockGameOver.Text = "Victory!";
                            ImageFace.Source = face_player;
                            break;

                        case PlanetOwner.Enemy:
                            TextBlockGameOver.Text = "Game over!";
                            ImageFace.Source = face_enemy;
                            break;
                    }
                }
            }
        }
                
        private RoutedEventHandler eventOk;

        public RoutedEventHandler EventOk
        {
            get { return eventOk; }

            set
            {
                eventOk = value;
                ButtonOk.Click += value;
            }
        }
        
        private BitmapImage face_player = new BitmapImage(new Uri("/PlanetX;component/Images/face_player.png", UriKind.Relative));
        private BitmapImage face_enemy = new BitmapImage(new Uri("/PlanetX;component/Images/face_enemy.png", UriKind.Relative));

        public SilverlightControlGameOver()
        {
            InitializeComponent();

            winner = PlanetOwner.Neutral;
        }

        private void StoryboardGameOverHide_Completed(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
