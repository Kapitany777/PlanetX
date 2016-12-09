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
using PlanetX.Utils;
using System.Collections.Generic;

namespace PlanetX.Classes
{
    public class Galaxy
    {
        private Canvas canvas;
        private Random rnd = new Random();
        private NameGen namegen = new NameGen();

        public List<SilverlightControlPlanet> Planets = new List<SilverlightControlPlanet>();

        public MouseButtonEventHandler MouseLeftButtonDown;
        public MouseButtonEventHandler MouseRightButtonDown;

        public int Rows { get; set; }
        public int Columns { get; set; }

        public Galaxy(Canvas canvas)
        {
            this.canvas = canvas;

            Rows = GameParameters.GALAXY_ROWS;
            Columns = GameParameters.GALAXY_COLMUNS;
        }

        public void Generate()
        {
            double x = canvas.Width / Columns;
            double y = canvas.Height / Rows;

            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    SilverlightControlPlanet p = new SilverlightControlPlanet();

                    p.PlanetName = namegen.GetPlanetName();
                    p.Owner = PlanetOwner.Neutral;
                    p.ShipNum = rnd.Next(100) + 10;
                    p.MaxProd = rnd.Next(SilverlightControlPlanet.MAX_MAXPROD) + SilverlightControlPlanet.MIN_MAXPROD;
                    p.Production = rnd.Next(SilverlightControlPlanet.MAX_PRODUCTION) + SilverlightControlPlanet.MIN_PRODUCTION;

                    p.SetValue(Canvas.LeftProperty, i * x + rnd.Next((int)(x - p.Width - 10)) + 5);
                    p.SetValue(Canvas.TopProperty, j * y + rnd.Next((int)(y - p.Height - 10)) + 5);

                    double pv = Math.Max(p.PlanetValue(), 60.0) / 100.0;

                    p.Width *= pv;
                    p.Height *= pv;
                    
                    if (MouseLeftButtonDown != null)
                        p.MouseLeftButtonDown += MouseLeftButtonDown;

                    if (MouseRightButtonDown != null)
                        p.MouseRightButtonDown += MouseRightButtonDown;

                    Planets.Add(p);

                    canvas.Children.Add(p);
                }
            }

            // Initializing the player's planet
            Planets[0].Owner = PlanetOwner.Player;
            Planets[0].MaxProd = GameParameters.START_MAXPROD;
            Planets[0].ShipNum = GameParameters.START_SHIPNUM;
            Planets[0].Production = GameParameters.START_PRODUCTION;
            Planets[0].Width = SilverlightControlPlanet.PLANET_WIDTH;
            Planets[0].Height = SilverlightControlPlanet.PLANET_HEIGHT;

            // Initializing the enemy's planet
            Planets[Planets.Count - 1].Owner = PlanetOwner.Enemy;
            Planets[Planets.Count - 1].MaxProd = GameParameters.START_MAXPROD;
            Planets[Planets.Count - 1].ShipNum = GameParameters.START_SHIPNUM;
            Planets[Planets.Count - 1].Production = GameParameters.START_PRODUCTION;
            Planets[Planets.Count - 1].Width = SilverlightControlPlanet.PLANET_WIDTH;
            Planets[Planets.Count - 1].Height = SilverlightControlPlanet.PLANET_HEIGHT;
        }

        public void AddEnemyPlanets(int num)
        {
            SilverlightControlPlanet p;

            int i = 1;

            while (i < num)
            {
                p = this.GetRandomPlanet();

                if (p != this.GetFirstPlanet() && p.Owner != PlanetOwner.Enemy)
                {
                    p.Owner = PlanetOwner.Enemy;
                    i++;
                }
            }
        }

        public SilverlightControlPlanet GetFirstPlanet()
        {
            return Planets[0];
        }

        public SilverlightControlPlanet GetLastPlanet()
        {
            return Planets[Planets.Count - 1];
        }

        public SilverlightControlPlanet GetRandomPlanet()
        {
            return Planets[rnd.Next(Planets.Count)];
        }

        public void Update()
        {
            foreach (SilverlightControlPlanet planet in Planets)
            {
                planet.Update();
            }
        }

        public int ShipCount(PlanetOwner owner)
        {
            int sum = 0;

            foreach (SilverlightControlPlanet planet in Planets)
            {
                if (planet.Owner == owner)
                    sum += planet.ShipNum;
            }

            return sum;
        }

        public int PlayerShips()
        {
            return ShipCount(PlanetOwner.Player);
        }

        public int EnemyShips()
        {
            return ShipCount(PlanetOwner.Enemy);
        }

        public int PlanetCount(PlanetOwner owner)
        {
            int sum = 0;

            foreach (SilverlightControlPlanet planet in Planets)
            {
                if (planet.Owner == owner)
                    sum++;
            }

            return sum;
        }

        public int PlayerPlanets()
        {
            return PlanetCount(PlanetOwner.Player);
        }

        public int EnemyPlanets()
        {
            return PlanetCount(PlanetOwner.Enemy);
        }
    }
}
