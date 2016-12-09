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
using System.ComponentModel;

namespace PlanetX.Classes
{
    public class GameData : INotifyPropertyChanged
    {
        private int fleetPercent;

        public int FleetPercent
        {
            get { return fleetPercent; }

            set
            {
                if (fleetPercent != value)
                {
                    fleetPercent = value;
                    OnPropertyChanged("FleetPercent");
                }
            }
        }

        private int playerShips;

        public int PlayerShips
        {
            get { return playerShips; }

            set
            {
                if (playerShips != value)
                {
                    playerShips = value;
                    OnPropertyChanged("PlayerShips");
                }
            }
        }

        private int enemyShips;

        public int EnemyShips
        {
            get { return enemyShips; }

            set
            {
                if (enemyShips != value)
                {
                    enemyShips = value;
                    OnPropertyChanged("EnemyShips");
                }
            }
        }

        // When a property changes
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
