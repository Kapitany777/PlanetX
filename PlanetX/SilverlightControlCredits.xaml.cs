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

namespace PlanetX
{
    public partial class SilverlightControlCredits : UserControl
    {
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

        public SilverlightControlCredits()
        {
            InitializeComponent();
        }

        private void StoryboardCreditsHide_Completed(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
