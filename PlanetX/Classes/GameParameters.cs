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

namespace PlanetX.Classes
{
    public class GameParameters
    {
        // Size of the galaxy, rows and columns
        public const int GALAXY_ROWS = 4;
        public const int GALAXY_COLMUNS = 4;

        // 
        public const double PLANET_RADIUS = 300.0;

        // Starting values
        public const int START_MAXPROD = SilverlightControlPlanet.MAX_MAXPROD;
        public const int START_SHIPNUM = 100;
        public const int START_PRODUCTION = SilverlightControlPlanet.MAX_PRODUCTION;

        // Production modifier
        public const int SPEED_PRODUCTION = 180;

        // Initial speed of the fleets
        public const double INIT_FLEET_SPEED = 2.2;

        // Hard level settings
        
        // More enemy planets
        public const int HARD_LEVEL_PLANET_NUM = 3;

        // Faster enemy fleets
        public const double HARD_LEVEL_SPEED_MODIFIER = 1.2;
    }
}
