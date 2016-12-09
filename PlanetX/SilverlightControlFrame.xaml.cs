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
    public partial class SilverlightControlFrame : UserControl
    {
        public SilverlightControlFrame()
        {
            InitializeComponent();

            StoryboardFrame.Begin();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            LayoutRoot.Width = this.Width;
            LayoutRoot.Height = this.Height;

            RectangleFrame.Width = this.Width;
            RectangleFrame.Height = this.Height;
        }
    }
}
