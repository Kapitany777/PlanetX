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
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using PlanetX.Classes;
using PlanetX.Utils;

namespace PlanetX
{
    public partial class MainPage : UserControl
    {
        private GameData gameData;

        private Difficulty GameDifficulty = Difficulty.Normal;

        private delegate void TEnemy_AI();
        TEnemy_AI Enemy_AI;

        private Galaxy galaxy;
        private Fleets fleets;

        private SilverlightControlPlanet currentPlanet;

        private DispatcherTimer timerGame;

        private DispatcherTimer timerTime;
        DateTime gameTime;

        private Random rnd = new Random();

        private int edgeShowType = 1;

        private BitmapImage face_player = new BitmapImage(new Uri("/PlanetX;component/Images/face_player.png", UriKind.Relative));
        private BitmapImage face_enemy = new BitmapImage(new Uri("/PlanetX;component/Images/face_enemy.png", UriKind.Relative));
        private BitmapImage face_neutral = new BitmapImage(new Uri("/PlanetX;component/Images/face_neutral.png", UriKind.Relative));

        private BitmapImage sound_on = new BitmapImage(new Uri("/PlanetX;component/Images/sound_on_32_inv.png", UriKind.Relative));
        private BitmapImage mute = new BitmapImage(new Uri("/PlanetX;component/Images/mute_32_inv.png", UriKind.Relative));

        public MainPage()
        {
            InitializeComponent();

            silverlightControlMenu.SetValue(Canvas.LeftProperty, (CanvasGame.Width - silverlightControlMenu.Width) / 2.0);
            silverlightControlMenu.SetValue(Canvas.TopProperty, (CanvasGame.Height - silverlightControlMenu.Height) / 2.0);

            silverlightControlMenu.EventNewGame = NewGame_MouseLeftButtonDown;
            silverlightControlMenu.EventReturnToGame = ReturnToGame_MouseLeftButtonDown;

            silverlightControlCredits.SetValue(Canvas.LeftProperty, (CanvasGame.Width - silverlightControlCredits.Width) / 2.0);
            silverlightControlCredits.SetValue(Canvas.TopProperty, (CanvasGame.Height - silverlightControlCredits.Height) / 2.0);

            silverlightControlCredits.EventOk = CreditsOk_Click;

            silverlightControlGameOver.SetValue(Canvas.LeftProperty, (CanvasGame.Width - silverlightControlGameOver.Width) / 2.0);
            silverlightControlGameOver.SetValue(Canvas.TopProperty, (CanvasGame.Height - silverlightControlGameOver.Height) / 2.0);

            silverlightControlGameOver.EventOk = GameOverOk_Click;

            timerGame = new DispatcherTimer();
            timerGame.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timerGame.Tick += new EventHandler(timer_Tick);

            timerTime = new DispatcherTimer();
            timerTime.Interval = new TimeSpan(0, 0, 1);
            timerTime.Tick += new EventHandler(timerTime_Tick);

            try
            {
                gameTime = new DateTime(2011, 01, 01, 0, 0, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void timerTime_Tick(object sender, EventArgs e)
        {
            gameTime = gameTime.AddSeconds(1.0);
            
            TextBlockTime.Text = gameTime.ToString("HH:mm:ss");
        }

        private void RemovePlanets()
        {
            // A megjelenitett bolygok torlese a gridbol
            List<UIElement> rlist = new List<UIElement>();

            foreach (UIElement item in CanvasGame.Children)
            {
                if (item is SilverlightControlPlanet)
                {
                    rlist.Add(item);
                }
            }

            foreach (UIElement item in rlist)
            {
                CanvasGame.Children.Remove(item);
            }
        }

        private void RemoveFleets()
        {
            // A megjelenitett flottak torlese a gridbol
            List<UIElement> rlist = new List<UIElement>();

            foreach (UIElement item in CanvasGame.Children)
            {
                if (item is SilverlightControlFleet)
                {
                    rlist.Add(item);
                }
            }

            foreach (UIElement item in rlist)
            {
                CanvasGame.Children.Remove(item);
            }
        }

        private void RemoveEdges()
        {
            // A megjelenitett utvonalak torlese a gridbol
            List<UIElement> rlist = new List<UIElement>();

            foreach (UIElement item in CanvasGame.Children)
            {
                if (item is Line)
                {
                    rlist.Add(item);
                }
            }

            foreach (UIElement item in rlist)
            {
                CanvasGame.Children.Remove(item);
            }
        }

        private void RemoveGameElements()
        {
            RemoveFleets();
            RemoveEdges();
            RemovePlanets();
        }

        private void CreateEdges()
        {
            for (int i = 0; i < galaxy.Planets.Count; i++)
            {
                SilverlightControlPlanet planet1 = galaxy.Planets[i];

                for (int j = i + 1; j < galaxy.Planets.Count; j++)
                {
                    SilverlightControlPlanet planet2 = galaxy.Planets[j];

                    if (planet1.Distance(planet2) <= GameParameters.PLANET_RADIUS)
                    {
                        Line l = new Line();

                        l.X1 = planet1.CenterX;
                        l.Y1 = planet1.CenterY;

                        l.X2 = planet2.CenterX;
                        l.Y2 = planet2.CenterY;

                        l.Stroke = new SolidColorBrush(Colors.White);
                        l.StrokeThickness = 1.0;

                        l.StrokeDashArray = new DoubleCollection() { 1, 5 };

                        CanvasGame.Children.Add(l);
                    }
                }
            }
        }

        private void SetEdges(SilverlightControlPlanet planet)
        {
            foreach (UIElement item in CanvasGame.Children)
            {
                if (item is Line)
                {
                    try
                    {
                        Line l = item as Line;

                        if ((l.X1 == planet.CenterX && l.Y1 == planet.CenterY) ||
                            (l.X2 == planet.CenterX && l.Y2 == planet.CenterY))
                        {
                            // Kikenyszeritjuk az ujrarajzolast
                            l.Visibility = Visibility.Collapsed;

                            if (edgeShowType != 0)
                                l.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            l.Visibility = Visibility.Collapsed;

                            if (edgeShowType == 2)
                                l.Visibility = Visibility.Visible;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        void NewGame()
        {
            if (timerGame != null)
                timerGame.Stop();

            if (timerTime != null)
                timerTime.Stop();

            RemoveGameElements();

            gameData = new GameData();
            gameData.FleetPercent = 50;

            galaxy = new Galaxy(CanvasGame);
            galaxy.MouseLeftButtonDown = silverlightControlPlanet_MouseLeftButtonDown;
            galaxy.MouseRightButtonDown = silverlightControlPlanet_MouseRightButtonDown;

            galaxy.Generate();

            // Hard level: more enemy planets
            if (GameDifficulty == Difficulty.Hard)
            {
                galaxy.AddEnemyPlanets(GameParameters.HARD_LEVEL_PLANET_NUM);
            }

            // Ez a SetEdges ele kell!
            currentPlanet = galaxy.GetFirstPlanet();

            CreateEdges();
            SetEdges(currentPlanet);

            fleets = new Fleets();

            SetFrame(currentPlanet);
            SetRadius(currentPlanet);
            SetFace(currentPlanet);
            silverlightControlInfoPanel.DataContext = currentPlanet;

            silverlightControlGameInfo.DataContext = gameData;

            gameTime = new DateTime(2011, 01, 01, 0, 0, 0);
            TextBlockTime.Text = gameTime.ToString("HH:mm:ss");

            silverlightControlFrame.Visibility = Visibility.Visible;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            galaxy.Update();
            fleets.Update();

            SetRadius(currentPlanet);
            SetFace(currentPlanet);

            Enemy_AI();

            gameData.PlayerShips = galaxy.PlayerShips() + fleets.PlayerShips();
            gameData.EnemyShips = galaxy.EnemyShips() + fleets.EnemyShips();

            if (galaxy.EnemyPlanets() == 0 && gameData.EnemyShips == 0)
            {
                timerGame.Stop();
                timerTime.Stop();
                RemoveFleets();

                silverlightControlGameOver.Winner = PlanetOwner.Player;
                ShowGameOver();
            }
            else if (galaxy.PlayerPlanets() == 0 && gameData.PlayerShips == 0)
            {
                timerGame.Stop();
                timerTime.Stop();
                RemoveFleets();

                silverlightControlGameOver.Winner = PlanetOwner.Enemy;
                ShowGameOver();
            }
        }

        private bool IsGameFinished()
        {
            return (galaxy.EnemyPlanets() == 0 && gameData.EnemyShips == 0) ||
                   (galaxy.PlayerPlanets() == 0 && gameData.PlayerShips == 0);
        }

        private void CanvasGame_Loaded(object sender, RoutedEventArgs e)
        {
            ShowMenu();
        }

        private void SetFrame(SilverlightControlPlanet planet)
        {
            double x = (double)planet.GetValue(Canvas.LeftProperty) - 2;
            silverlightControlFrame.SetValue(Canvas.LeftProperty, x);

            double y = (double)planet.GetValue(Canvas.TopProperty) - 2;
            silverlightControlFrame.SetValue(Canvas.TopProperty, y);

            silverlightControlFrame.Width = planet.Width + 5;
            silverlightControlFrame.Height = planet.Height + 5;
        }

        private void SetRadius(SilverlightControlPlanet planet)
        {
            if (planet.Owner == PlanetOwner.Player)
            {
                double x = (double)planet.GetValue(Canvas.LeftProperty) + planet.Width / 2.0 - silverlightControlRadius.Width / 2.0;
                silverlightControlRadius.SetValue(Canvas.LeftProperty, x);

                double y = (double)planet.GetValue(Canvas.TopProperty) + planet.Height / 2.0 - silverlightControlRadius.Height / 2.0;
                silverlightControlRadius.SetValue(Canvas.TopProperty, y);

                silverlightControlRadius.Visibility = Visibility.Visible;
            }
            else
            {
                silverlightControlRadius.Visibility = Visibility.Collapsed;
            }
        }

        private void SetFace(SilverlightControlPlanet planet)
        {
            switch (planet.Owner)
            {
                case PlanetOwner.Player:
                    ImageFace.Source = face_player;
                    break;

                case PlanetOwner.Enemy:
                    ImageFace.Source = face_enemy;
                    break;

                case PlanetOwner.Neutral:
                    ImageFace.Source = face_neutral;
                    break;
            }
        }

        private void SetControls(bool isEnabled)
        {
            foreach (var item in CanvasGame.Children)
            {
                if (item is SilverlightControlFleet || item is SilverlightControlPlanet)
                {
                    (item as Control).IsEnabled = isEnabled;
                }
            }
        }

        private void EnableControls()
        {
            SetControls(true);
        }

        private void DisableControls()
        {
            SetControls(false);
        }

        private void silverlightControlPlanet_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentPlanet = sender as SilverlightControlPlanet;
            SetFrame(currentPlanet);
            SetRadius(currentPlanet);
            SetFace(currentPlanet);
            SetEdges(currentPlanet);

            silverlightControlInfoPanel.DataContext = sender;

            SoundClick.Position = TimeSpan.Zero;
            SoundClick.Play();
        }

        private void SendFleet(SilverlightControlPlanet planetFrom, SilverlightControlPlanet planetTo, int ships)
        {
            if (planetFrom.ShipNum > 0)
            {
                int send_ships = Math.Min(ships, planetFrom.ShipNum);

                SilverlightControlFleet fleet = new SilverlightControlFleet();

                fleet.SetPlanets(planetFrom, planetTo);
                fleet.ShipNum = send_ships;

                if (GameDifficulty == Difficulty.Hard && planetFrom.Owner == PlanetOwner.Enemy)
                {
                    fleet.Speed *= GameParameters.HARD_LEVEL_SPEED_MODIFIER;
                }

                fleet.SetValue(Canvas.LeftProperty, planetFrom.GetValue(Canvas.LeftProperty));
                fleet.SetValue(Canvas.TopProperty, planetFrom.GetValue(Canvas.TopProperty));

                fleets.Add(fleet);

                CanvasGame.Children.Add(fleet);

                planetFrom.ShipNum -= send_ships;
            }
        }

        private void silverlightControlPlanet_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsGameFinished())
                return;

            if (currentPlanet.Owner == PlanetOwner.Player)
            {
                SilverlightControlPlanet dest = sender as SilverlightControlPlanet;

                if (currentPlanet.Distance(dest) <= GameParameters.PLANET_RADIUS)
                {
                    int ships = (int)(currentPlanet.ShipNum * gameData.FleetPercent / 100.0);

                    SendFleet(currentPlanet, dest, ships);

                    SoundStart.Position = TimeSpan.Zero;
                    SoundStart.Play();
                }
                else
                {
                    SoundTooFar.Position = TimeSpan.Zero;
                    SoundTooFar.Play();
                }
            }

            e.Handled = true;
        }

        private void LayoutRoot_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void GameMusic_MediaEnded(object sender, RoutedEventArgs e)
        {
            (sender as MediaElement).Position = TimeSpan.Zero;
            (sender as MediaElement).Play();
        }

        private void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (gameData.FleetPercent <= 90)
                    gameData.FleetPercent += 10;
            }
            else
            {
                if (gameData.FleetPercent >= 20)
                    gameData.FleetPercent -= 10;
            }

            e.Handled = true;
        }

        private void ShowMenu()
        {
            timerGame.Stop();
            timerTime.Stop();

            DisableControls();

            silverlightControlMenu.GameDifficulty = GameDifficulty;
            silverlightControlMenu.SetDifficultyColors();
            silverlightControlMenu.Visibility = Visibility.Visible;
            silverlightControlMenu.StoryboardMenuShow.Begin();
        }

        private void HideMenu()
        {
            silverlightControlMenu.StoryboardMenuHide.Begin();

            GameDifficulty = silverlightControlMenu.GameDifficulty;

            SetGameDifficulty();

            EnableControls();
        }

        private void SetGameDifficulty()
        {
            switch (GameDifficulty)
            {
                case Difficulty.Easy:
                    Enemy_AI = Enemy_AI_Easy;
                    break;

                case Difficulty.Normal:
                    Enemy_AI = Enemy_AI_Normal;
                    break;

                case Difficulty.Hard:
                    Enemy_AI = Enemy_AI_Hard;
                    break;
            }
        }

        private void TextBlockMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (silverlightControlCredits.Visibility == Visibility.Collapsed)
                ShowMenu();
        }

        private void NewGame_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HideMenu();

            NewGame();

            timerGame.Start();
            timerTime.Start();
        }

        private void ReturnToGame_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HideMenu();

            if (!IsGameFinished())
            {
                timerGame.Start();
                timerTime.Start();
            }
        }

        private void CreditsOk_Click(object sender, RoutedEventArgs e)
        {
            HideCredits();

            if (!IsGameFinished())
            {
                timerGame.Start();
                timerTime.Start();
            }
        }

        private void ShowCredits()
        {
            timerGame.Stop();
            timerTime.Stop();

            DisableControls();

            silverlightControlCredits.Visibility = Visibility.Visible;

            try
            {
                silverlightControlCredits.StoryboardCreditsShow.Begin();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void HideCredits()
        {
            silverlightControlCredits.StoryboardCreditsHide.Begin();

            EnableControls();
        }

        private void TextBlockCredits_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (silverlightControlMenu.Visibility == Visibility.Collapsed)
                ShowCredits();
        }

        private void TextBlockEdges_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            edgeShowType++;

            if (edgeShowType > 2)
                edgeShowType = 0;

            SetEdges(currentPlanet);
        }

        private void GameOverOk_Click(object sender, RoutedEventArgs e)
        {
            silverlightControlGameOver.StoryboardGameOverHide.Begin();

            EnableControls();
        }

        private void ShowGameOver()
        {
            DisableControls();

            silverlightControlGameOver.Visibility = Visibility.Visible;
            silverlightControlGameOver.StoryboardGameOverShow.Begin();
        }

        private void ImageSoundIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (GameMusic.CurrentState == MediaElementState.Stopped)
            {
                ImageSoundIcon.Source = sound_on;
                GameMusic.Play();
            }
            else
            {
                ImageSoundIcon.Source = mute;
                GameMusic.Stop();
            }
        }

        // Easy level Enemy AI
        private void Enemy_AI_Easy()
        {
            foreach (SilverlightControlPlanet planet1 in galaxy.Planets)
            {
                // Ha nem ellenseges bolygo, akkor a ciklus elejere ugrunk
                if (planet1.Owner != PlanetOwner.Enemy)
                    continue;

                // Celpont keresese
                bool found = false;

                // Keresunk egy semleges bolygot
                foreach (SilverlightControlPlanet planet2 in galaxy.Planets)
                {
                    if (planet2.Owner == PlanetOwner.Neutral)
                    {
                        if (rnd.Next(100) < 5 &&
                             planet1.ShipNum >= 40 &&
                             planet1.Distance(planet2) <= GameParameters.PLANET_RADIUS)
                        {
                            SendFleet(planet1, planet2, rnd.Next(25) + 15);
                            found = true;
                        }
                    }
                }

                if (found)
                    continue;

                // Keresunk egy ellenseges bolygot
                foreach (SilverlightControlPlanet planet2 in galaxy.Planets)
                {
                    if (planet2.Owner == PlanetOwner.Player)
                    {
                        if (rnd.Next(100) < 5 &&
                             planet1.ShipNum >= 40 &&
                             planet1.Distance(planet2) <= GameParameters.PLANET_RADIUS)
                        {
                            SendFleet(planet1, planet2, rnd.Next(25) + 15);
                            found = true;
                        }
                    }
                }

                if (found)
                    continue;

                // Egyebkent erositest kuldunk egy sajat bolygora
                foreach (SilverlightControlPlanet planet2 in galaxy.Planets)
                {
                    if (planet2.Owner == PlanetOwner.Enemy && planet2 != planet1)
                    {
                        if (rnd.Next(100) < 5 &&
                            planet1.ShipNum >= 60 &&
                            planet1.Distance(planet2) <= GameParameters.PLANET_RADIUS)
                        {
                            SendFleet(planet1, planet2, rnd.Next(40) + 15);
                            found = true;
                        }
                    }
                }

            }
        }

        // Normal level Enemy AI
        private void Enemy_AI_Normal()
        {
            foreach (SilverlightControlPlanet planet1 in galaxy.Planets)
            {
                // Ha nem ellenseges bolygo, akkor a ciklus elejere ugrunk
                if (planet1.Owner != PlanetOwner.Enemy)
                    continue;

                // Celpont keresese
                bool found = false;

                // Keresunk egy semleges bolygot
                foreach (SilverlightControlPlanet planet2 in galaxy.Planets)
                {
                    if (planet2.Owner == PlanetOwner.Neutral)
                    {
                        if (rnd.Next(100) < 20 &&
                             planet1.ShipNum >= 40 &&
                             planet1.Distance(planet2) <= GameParameters.PLANET_RADIUS)
                        {
                            SendFleet(planet1, planet2, rnd.Next(25) + 15);
                            found = true;
                        }
                    }
                }

                if (found)
                    continue;

                // Keresunk egy ellenseges bolygot
                foreach (SilverlightControlPlanet planet2 in galaxy.Planets)
                {
                    if (planet2.Owner == PlanetOwner.Player)
                    {
                        if (rnd.Next(100) < 15 &&
                             planet1.ShipNum >= 40 &&
                             planet1.Distance(planet2) <= GameParameters.PLANET_RADIUS)
                        {
                            SendFleet(planet1, planet2, rnd.Next(25) + 15);
                            found = true;
                        }
                    }
                }

                if (found)
                    continue;

                // Egyebkent erositest kuldunk egy sajat bolygora
                foreach (SilverlightControlPlanet planet2 in galaxy.Planets)
                {
                    if (planet2.Owner == PlanetOwner.Enemy && planet2 != planet1)
                    {
                        if (rnd.Next(100) < 20 &&
                            planet1.ShipNum >= 60 &&
                            planet1.Distance(planet2) <= GameParameters.PLANET_RADIUS)
                        {
                            SendFleet(planet1, planet2, rnd.Next(40) + 15);
                            found = true;
                        }
                    }
                }

            }
        }

        // Hard level Enemy AI
        private void Enemy_AI_Hard()
        {
            foreach (SilverlightControlPlanet planet1 in galaxy.Planets)
            {
                // Ha nem ellenseges bolygo, akkor a ciklus elejere ugrunk
                if (planet1.Owner != PlanetOwner.Enemy)
                    continue;

                // Celpont keresese
                bool found = false;

                // Keresunk egy semleges bolygot
                foreach (SilverlightControlPlanet planet2 in galaxy.Planets)
                {
                    if (planet2.Owner == PlanetOwner.Neutral)
                    {
                        if (rnd.Next(100) < 40 &&
                             planet1.ShipNum >= 40 &&
                             planet1.Distance(planet2) <= GameParameters.PLANET_RADIUS)
                        {
                            SendFleet(planet1, planet2, rnd.Next(25) + 15);
                            found = true;
                        }
                    }
                }

                if (found)
                    continue;

                // Keresunk egy ellenseges bolygot
                foreach (SilverlightControlPlanet planet2 in galaxy.Planets)
                {
                    if (planet2.Owner == PlanetOwner.Player)
                    {
                        if (rnd.Next(100) < 35 &&
                             planet1.ShipNum >= 40 &&
                             planet1.Distance(planet2) <= GameParameters.PLANET_RADIUS)
                        {
                            SendFleet(planet1, planet2, rnd.Next(25) + 15);
                            found = true;
                        }
                    }
                }

                if (found)
                    continue;

                // Egyebkent erositest kuldunk egy sajat bolygora
                foreach (SilverlightControlPlanet planet2 in galaxy.Planets)
                {
                    if (planet2.Owner == PlanetOwner.Enemy && planet2 != planet1)
                    {
                        if (rnd.Next(100) < 20 &&
                            planet1.ShipNum >= 60 &&
                            planet1.Distance(planet2) <= GameParameters.PLANET_RADIUS)
                        {
                            SendFleet(planet1, planet2, rnd.Next(40) + 15);
                            found = true;
                        }
                    }
                }

            }
        }


        // End of class
    }
}
