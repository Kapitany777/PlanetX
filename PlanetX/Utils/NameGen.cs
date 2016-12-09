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
using System.Text;

namespace PlanetX.Utils
{
    public class NameGen
    {
        private string Consonants = "bcdfghjklmnpqrstvwxyz";
        private string Vocals = "aeiou";

        private string RndConsonants = "bcdfghjklmnprstvz";
        private string RndVocals = "aeiou";

        private string[] PlanetNames =
        { "Mercury", "Venus", "Earth", "Mars", "Ceres", "Jupiter", "Saturn", "Uranus", "Neptune", "Pluto", "Charon",
          "Moon", "Deimos", "Phobos", "Ganymede", "Callisto", "Europa", "Titan", "Enceladus", "Titania", "Oberon",
          "Umbriel", "Ariel", "Miranda", "Triton", "Nemesis", "Vega", "Eris", "Kepler", "Newton", "Galilei",
          "Copernicus", "Halley", "Einstein", "Braun", "Aldrin", "Shepard", "Ceti", "Centaur",
          "Alpha", "Beta", "Gamma", "Delta", "Epsilon", "Tau", "Omega" };

        private Random rnd;

        public NameGen()
        {
            rnd = new Random();
        }

        public string GetNextName()
        {
            StringBuilder sb = new StringBuilder();

            if (rnd.Next(100) < 50)
            {
                sb.Append(Consonants[rnd.Next(Consonants.Length)].ToString().ToUpper());
            }
            else
            {
                sb.Append(Vocals[rnd.Next(Vocals.Length)].ToString().ToUpper());
            }

            for (int i = 0; i < rnd.Next(3) + 2; i++)
            {
                sb.Append(Consonants[rnd.Next(Consonants.Length)]);
                sb.Append(Vocals[rnd.Next(Vocals.Length)]);
            }

            return sb.ToString();
        }

        private bool IsConsonant(char c)
        {
            return Consonants.Contains(c.ToString());
        }

        private char GetRandomConsonant()
        {
            return RndConsonants[rnd.Next(RndConsonants.Length)];
        }

        private bool IsVocal(char c)
        {
            return Vocals.Contains(c.ToString());
        }

        private char GetRandomVocal()
        {
            return RndVocals[rnd.Next(RndVocals.Length)];
        }

        public string GetPlanetName()
        {
            StringBuilder sb = new StringBuilder();
            string planetName = PlanetNames[rnd.Next(PlanetNames.Length)];

            sb.Append(planetName[0]);

            for (int i = 1; i < planetName.Length; i++)
            {
                char c = planetName[i];

                if (rnd.Next(100) < 40)
                {
                    if (IsConsonant(c))
                    {
                        c = GetRandomConsonant();
                    }
                    else
                    {
                        c = GetRandomVocal();
                    }
                }

                sb.Append(c);
            }

            return sb.ToString();
        }

    }
}
