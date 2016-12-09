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
    public partial class SilverlightControlRadius : UserControl
    {
        public SilverlightControlRadius()
        {
            InitializeComponent();

            this.Width = GameParameters.PLANET_RADIUS * 2;
            this.Height = GameParameters.PLANET_RADIUS * 2;

            EllipseRadius.Width = GameParameters.PLANET_RADIUS * 2;
            EllipseRadius.Height = GameParameters.PLANET_RADIUS * 2;
        }
    }
}
