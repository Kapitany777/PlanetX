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
using System.ComponentModel;
using PlanetX.Classes;
using PlanetX.Utils;

namespace PlanetX
{
    public partial class SilverlightControlFleet : UserControl
    {
        public int ShipNum { get; set; }
        public bool IsActive { get; set; }

        private PlanetOwner owner;

        [Category("Fleet")]
        public PlanetOwner Owner
        {
            get
            {
                return owner;
            }
        }

        [Category("Fleet")]
        public double Speed { get; set; }
        
        private SilverlightControlPlanet start;
        private SilverlightControlPlanet destination;
        
        private double destx;
        private double desty;

        private double px;
        private double py;

        private double xk;
        private double yk;

        private BitmapImage ship_player = new BitmapImage(new Uri("/PlanetX;component/Images/ship_player.png", UriKind.Relative));
        private BitmapImage ship_enemy = new BitmapImage(new Uri("/PlanetX;component/Images/ship_enemy.png", UriKind.Relative));

        public SilverlightControlFleet()
        {
            InitializeComponent();

            ShipNum = 0;
            IsActive = false;
            Speed = GameParameters.INIT_FLEET_SPEED;
        }

        public void SetPlanets(SilverlightControlPlanet startplanet, SilverlightControlPlanet destplanet)
        {
            start = startplanet;
            destination = destplanet;

            double startx = (double)start.GetValue(Canvas.LeftProperty) + start.Width / 2.0 - this.Width / 2.0;
            double starty = (double)start.GetValue(Canvas.TopProperty) + start.Height / 2.0 - this.Height / 2.0;

            destx = (double)destination.GetValue(Canvas.LeftProperty) + destination.Width / 2.0 - this.Width / 2.0;
            desty = (double)destination.GetValue(Canvas.TopProperty) + destination.Height / 2.0 - this.Width / 2.0;

            xk = startx;
            yk = starty;

            double dx = destx - startx;
            double dy = desty - starty;
            double d = PlanetMath.Length(dx, dy);

            px = Speed * (dx / d);
            py = Speed * (dy / d);

            owner = start.Owner;

            if (owner == PlanetOwner.Player)
                ImageFleet.Source = ship_player;
            else
                ImageFleet.Source = ship_enemy;

            FleetRotate.Angle = PlanetMath.RotationAngle(startx, starty, destx, desty);

            IsActive = true;
        }

        public void Update()
        {
            xk += px;
            yk += py;

            this.SetValue(Canvas.LeftProperty, xk);
            this.SetValue(Canvas.TopProperty, yk);
            
            if (PlanetMath.Distance(xk, yk, destx, desty) < 5.0)
            {
                IsActive = false;
                Visibility = Visibility.Collapsed;

                Landing();
            }
        }

        private void Landing()
        {
            if (destination.Owner == this.Owner)
            {
                destination.ShipNum += this.ShipNum;
            }
            else
            {
                if (destination.ShipNum <= this.ShipNum)
                {
                    destination.Owner = this.Owner;
                    destination.ShipNum = this.ShipNum - destination.ShipNum;
                }
                else
                {
                    destination.ShipNum -= this.ShipNum;
                }
            }
        }
    }
}
