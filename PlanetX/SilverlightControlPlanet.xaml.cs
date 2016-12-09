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
using System.ComponentModel;
using PlanetX.Classes;
using PlanetX.Utils;

namespace PlanetX
{
    public partial class SilverlightControlPlanet : UserControl, INotifyPropertyChanged
    {
        public const int MIN_MAXPROD = 20;
        public const int MAX_MAXPROD = 200;

        public const int MIN_PRODUCTION = 10;
        public const int MAX_PRODUCTION = 100;

        public const double PLANET_WIDTH = 50.0;
        public const double PLANET_HEIGHT = 50.0;

        // Production counter
        private int prodcount;

        // The planet's name
        private string planetName;

        [Category("Planet")]
        public string PlanetName
        {
            get { return planetName; }

            set
            {
                if (planetName != value)
                {
                    planetName = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        // The planet's owner
        private PlanetOwner owner;

        [Category("Planet")]
        public PlanetOwner Owner
        {
            get { return owner; }

            set
            {
                if (owner != value)
                {
                    owner = value;
                    OnPropertyChanged("Owner");

                    switch (owner)
                    {
                        case PlanetOwner.Player:
                            LayoutRoot.Background = this.Resources["ImagePlanetPlayer"] as ImageBrush;
                            break;

                        case PlanetOwner.Enemy:
                            LayoutRoot.Background = this.Resources["ImagePlanetEnemy"] as ImageBrush;
                            break;

                        case PlanetOwner.Neutral:
                            LayoutRoot.Background = this.Resources["ImagePlanetNeutral"] as ImageBrush;
                            break;
                    }
                }
            }
        }

        // The actual number of the ships
        private int shipNum;

        [Category("Planet")]
        public int ShipNum
        {
            get { return shipNum; }

            set
            {
                if (shipNum != value)
                {
                    shipNum = value;
                    OnPropertyChanged("ShipNum");
                }
            }
        }

        // The maximum production of the ships
        private int maxProd;

        [Category("Planet")]
        public int MaxProd
        {
            get { return maxProd; }

            set
            {
                if (maxProd != value)
                {
                    maxProd = Math.Min(value, MAX_MAXPROD);
                    OnPropertyChanged("MaxShip");
                }
            }
        }

        // The X coordinate of the planet's center
        public double CenterX
        {
            get
            {
                return (double)this.GetValue(Canvas.LeftProperty) + this.Width / 2.0;
            }
        }

        // The Y coordinate of the planet's center
        public double CenterY
        {
            get
            {
                return (double)this.GetValue(Canvas.TopProperty) + this.Height / 2.0;
            }
        }

        // The level of the production
        private int production;

        [Category("Planet")]
        public int Production
        {
            get { return production; }

            set
            {
                if (production != value)
                {
                    production = Math.Min(value, MAX_PRODUCTION);
                    OnPropertyChanged("Production");
                }
            }
        }

        // Constructor
        public SilverlightControlPlanet()
        {
            InitializeComponent();

            LayoutRoot.DataContext = this;

            this.Width = PLANET_WIDTH;
            this.Height = PLANET_HEIGHT;

            PlanetName = "Planet";
            Owner = PlanetOwner.Neutral;
            ShipNum = 10;
            MaxProd = 100;
            Production = 100;

            prodcount = 0;
        }

        // Updating the planet
        public void Update()
        {
            if (Owner == PlanetOwner.Player || Owner == PlanetOwner.Enemy)
            {
                prodcount += Production;

                if (prodcount >= GameParameters.SPEED_PRODUCTION)
                {
                    if (ShipNum < MaxProd)
                    {
                        ShipNum++;
                    }

                    prodcount -= GameParameters.SPEED_PRODUCTION;
                }
            }
        }

        // The planet's value for the AI
        public int PlanetValue()
        {
            return this.Production;
        }

        // Distance of two planets
        public double Distance(SilverlightControlPlanet planet)
        {
            double x1 = (double)this.GetValue(Canvas.LeftProperty);
            double y1 = (double)this.GetValue(Canvas.TopProperty);

            double x2 = (double)planet.GetValue(Canvas.LeftProperty);
            double y2 = (double)planet.GetValue(Canvas.TopProperty);

            return PlanetMath.Distance(x1, y1, x2, y2);
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
