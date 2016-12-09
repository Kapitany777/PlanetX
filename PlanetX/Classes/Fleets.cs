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
using System.Collections.Generic;

namespace PlanetX.Classes
{
    public class Fleets
    {
        private List<SilverlightControlFleet> fleets = new List<SilverlightControlFleet>();

        public Fleets()
        {

        }

        public void Add(SilverlightControlFleet fleet)
        {
            fleets.Add(fleet);
        }

        public void Update()
        {
            // Move fleets
            foreach (SilverlightControlFleet fleet in fleets)
            {
                if (fleet.IsActive)
                {
                    fleet.Update();
                }
            }

            // Remove inactive fleets
            for (int i = fleets.Count - 1; i >= 0; i--)
            {
                if (!fleets[i].IsActive)
                {
                    fleets.RemoveAt(i);
                }
            }
        }

        public int ShipCount(PlanetOwner owner)
        {
            int sum = 0;

            foreach (SilverlightControlFleet fleet in fleets)
            {
                if (fleet.Owner == owner && fleet.IsActive)
                    sum += fleet.ShipNum;
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

        public int FleetCount()
        {
            return fleets.Count;
        }
    }
}
