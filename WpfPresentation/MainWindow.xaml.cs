using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LogicLayer;
using DataObjects;
using System.Text.RegularExpressions;

namespace WpfPresentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User _user = null;

        private List<PlanetVM> _planets = null;

        private PlanetManager _planetManager = new PlanetManager();
        private Window _window = null;

        private bool _addPlanetStatus = false;
        private bool _removePlanetStatus = false;
        private bool _movePlanet = false;
        private bool _fireStatus = false;

        private PlanetVM _planetToBeDestroyed = null;
        private string _planetName = null;
        private PlanetVM _planetToBeMoved = null;
        private bool _planetHasBeenSelected = false;

        private Grid grid = null;
        private Button button = null;
        private Window2 _planetInfo = null;


        public MainWindow(User user)
        {
            _user = user;
            InitializeComponent();

        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            this.Close();
            window1.Show();
        }

        private void _refreshSearchResults()
        {
            lblResults.Content = "Results:";
            try
            {
                _planets = _planetManager.RetrievePlanetVMsByPlanetID(txtSearch.Text);
                datSearch.ItemsSource = _planets;
                if (txtSearch.Text == "")
                {
                    datSearch.Columns.RemoveAt(0);
                    datSearch.Columns.RemoveAt(0);
                    datSearch.Columns.RemoveAt(0);
                    datSearch.Columns.RemoveAt(0);
                    datSearch.Columns.RemoveAt(0);
                    datSearch.Columns.RemoveAt(0);
                    datSearch.Columns.RemoveAt(0);
                    datSearch.Columns.RemoveAt(0);
                    datSearch.Columns.RemoveAt(0);
                    datSearch.Columns.RemoveAt(0);
                    datSearch.Columns.RemoveAt(0);
                    lblResults.Visibility = Visibility.Hidden;
                    return;
                }
                datSearch.Columns.RemoveAt(0);
                datSearch.Columns.RemoveAt(0);
                datSearch.Columns.RemoveAt(0);
                datSearch.Columns.RemoveAt(0);
                datSearch.Columns.RemoveAt(0);
                datSearch.Columns.RemoveAt(1);
                datSearch.Columns.RemoveAt(1);
                datSearch.Columns.RemoveAt(1);
                datSearch.Columns.RemoveAt(1);
                datSearch.Columns.RemoveAt(1);

                lblResults.Visibility = Visibility.Visible;
                lblResults.Content += " " + datSearch.Items.Count;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            _refreshSearchResults();
        }

        private void txtSearch_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                btnSearch_Click(sender, e);
            }
        }

        private void btnDatSearch_Click(object sender, RoutedEventArgs e)
        {

            var selectedPlanet = (PlanetVM)datSearch.SelectedItem;
            try
            {
                
                _window = Window.GetWindow(this);
                _planetInfo = new Window2(selectedPlanet, _window, _user);
                _planetInfo.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }

        // close application button
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnMainMaximize_Click(object sender, RoutedEventArgs e)
        {
            if(WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else {
                WindowState = WindowState.Maximized;
            }
        }

        private void btnMainMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void lblWindowBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();

            }
        }

        private void lblWindowBar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void imgMap_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(_addPlanetStatus)
            {
                brdMessage.Visibility = Visibility.Hidden;
                _window = Window.GetWindow(this);
                CreatePlanet createPlanet = new CreatePlanet(_window, Math.Round(e.GetPosition(canvas).X, 6) - 100, Math.Round(e.GetPosition(canvas).Y, 6) - 22.5);
                btnCancel.Visibility = Visibility.Hidden;
                createPlanet.ShowDialog();
                Window_Loaded(sender, e);
                _addPlanetStatus = false;
                brdMessage.Visibility = Visibility.Hidden;
                btnMessageClose.Visibility = Visibility.Hidden;
            }
            if (_movePlanet && _planetHasBeenSelected)
            {
                movePlanetOnMap(Math.Round(e.GetPosition(canvas).X, 6) - 100, Math.Round(e.GetPosition(canvas).Y, 6) - 22.5);
                Window_Loaded(sender, e);
                _movePlanet = false;
            }
            else
            {
  /*              brdMessage.Visibility = Visibility.Hidden;
                btnCancel.Visibility = Visibility.Hidden;*/
            }
        }
        
        private void _removeDeletedPlanetFromMap(string planetID)
        {
            List<Grid> remove = new List<Grid>();
            foreach (var children in canvas.Children)
            {
                if ((children.GetType() == typeof(Grid)))
                {
                    if ((children as Grid).Name == planetID)
                        remove.Add(children as Grid);
                }
            }
            foreach (var ch in remove)
            {
                canvas.Children.Remove(ch as Grid);
            }
            _refreshSearchResults();
        }
        private void _removePlanetFromMap(PlanetVM planetID)
        {
            try
            {
                _planetManager.DeletePlanetByPlanetID(planetID.PlanetID);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
            planetID.PlanetID = createOrFindPlanetElementName(planetID);
            _removeDeletedPlanetFromMap(planetID.PlanetID);
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            brdMessage.Visibility = Visibility.Visible;
            btnMessageClose.Visibility = Visibility.Hidden;
            lblMessage.Content = "Choose a location to add a planet";
            btnCancel.Visibility = Visibility.Visible;
            _movePlanet = false;
            if(_addPlanetStatus)
            {
                btnCancel.Visibility = Visibility.Hidden;
                brdMessage.Visibility = Visibility.Hidden;
                _addPlanetStatus = false;
                return;
            }
            _addPlanetStatus = true;
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (_removePlanetStatus)
            {
                _removePlanetStatus = false;
                btnCancel.Visibility = Visibility.Hidden;
                brdMessage.Visibility = Visibility.Hidden;
                btnFire.Visibility = Visibility.Hidden;
                return;
            }
            _movePlanet = false;
            _addPlanetStatus = false;
            _removePlanetStatus = true;
            _planetHasBeenSelected = false;
            btnCancel.Visibility = Visibility.Visible;
            brdMessage.Visibility = Visibility.Visible;
            btnMessageClose.Visibility = Visibility.Hidden;
            lblMessage.Content = "Select a planet to destroy";
        }

        private void _addPlanetOnStarMap(PlanetVM planetName)
        {

            grid = new Grid();
            grid.Width = 200;
            grid.Height = 45;
            grid.HorizontalAlignment = HorizontalAlignment.Center;

            TextBlock textBlock = new TextBlock();
            textBlock.FontSize = 20;
            textBlock.Foreground = Brushes.MintCream;
            textBlock.Background = Brushes.Transparent;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.Margin = new Thickness(0, -10, 0, 0);
            textBlock.Text = planetName.PlanetID;

            button = new Button();
            button.Cursor = Cursors.Hand;
            button.HorizontalAlignment = HorizontalAlignment.Center;
            button.Width = 20;
            button.Height = 20;
            button.Style = (Style)FindResource("RoundCorner");
            button.Padding = new Thickness(10);

            grid.Children.Add(textBlock);
            grid.Children.Add(button);

            button.Name = createOrFindPlanetElementName((PlanetVM)planetName);
            grid.Name = createOrFindPlanetElementName((PlanetVM)planetName);


            button.Click += (s, e) => {
                string planetRemoved = planetName.PlanetID;

                if (_removePlanetStatus)
                {

                    if (_fireStatus == false)
                    {
                        btnFire.Visibility = Visibility.Visible;
                    }
                    lblMessage.Content = planetRemoved + " is in your sights. You may fire when ready";
                    
                    _planetToBeDestroyed = planetName;
                    _planetName = planetRemoved;
                    return;
                }
                if (_movePlanet)
                {
                    lblMessage.Content = planetRemoved + " is selected, choose its new location.";
                    _planetToBeMoved = planetName;
                    _planetHasBeenSelected = true;

                    return;
                }
                brdMessage.Visibility = Visibility.Hidden;
                _window = Window.GetWindow(this);
                PlanetVM selectedPlanet = _planetManager.RetrievePlanetVMByPlanetID(planetName.PlanetID);
                _planetInfo = new Window2((PlanetVM)selectedPlanet, _window, _user);
                _planetInfo.ShowDialog();
            };

            canvas.Children.Add(grid);
            Canvas.SetLeft(grid, (double)planetName.PlanetCoordinateX);
            Canvas.SetTop(grid, (double)planetName.PlanetCoordinateY);
        }
        private void destroyPlanet(PlanetVM planetName, string planetRemoved)
        {
            _removePlanetFromMap(planetName);
            btnMessageClose.Visibility = Visibility.Visible;
            lblMessage.Content = "The Empire has destroyed " + planetRemoved;
            _removePlanetStatus = false;
        }

        private string createOrFindPlanetElementName(PlanetVM planetName)
        {
            string propName = Regex.Replace(planetName.PlanetID.ToString(), "[^0-9a-zA-Z]+", "");
            propName = Regex.Replace(propName, @"[\d-]", "");
            return propName;
        }

        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(String role in _user.Roles)
            {
                if(role == "Admin")
                {

                }
                if(role == "Trusted User")
                {
                    
                }
                if(role == "New User")
                {
                    brdEdit.Visibility = Visibility.Hidden;
                    btnAdd.Visibility = Visibility.Hidden;
                    btnRemove.Visibility = Visibility.Hidden;
                }
            }
            _planets = _planetManager.RetrievePlanetVMsByPlanetID("");
            foreach (PlanetVM planet in _planets)
            {
                _addPlanetOnStarMap(planet);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (_removePlanetStatus)
            {
                if (e.Key == Key.Escape)
                {
                    _removePlanetStatus = false;
                }
            }
        }

        private void btnMessageClose_Click(object sender, RoutedEventArgs e)
        {
            brdMessage.Visibility = Visibility.Hidden;
            btnMessageClose.Visibility = Visibility.Hidden;
        }

        private void btnFire_Click(object sender, RoutedEventArgs e)
        {

            btnCancel.Visibility = Visibility.Hidden;
            btnFire.Visibility = Visibility.Hidden;
            destroyPlanet(_planetToBeDestroyed, _planetName);

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _removePlanetStatus = false;
            _addPlanetStatus = false;
            _movePlanet = false;
            _planetHasBeenSelected = false;
            btnFire.Visibility = Visibility.Hidden;
            btnCancel.Visibility = Visibility.Hidden;
            brdMessage.Visibility = Visibility.Hidden;
        }

        private void btnMove_Click(object sender, RoutedEventArgs e)
        {
            if (_movePlanet)
            {
                brdMessage.Visibility = Visibility.Hidden;
                btnCancel.Visibility = Visibility.Hidden;
                _movePlanet = false;
                return;
            }
            _planetToBeMoved = null;
            btnCancel.Visibility = Visibility.Visible;
            _movePlanet = true;
            brdMessage.Visibility = Visibility.Visible;
            lblMessage.Content = "Select a planet to update its location";
            btnMessageClose.Visibility = Visibility.Hidden;
        }
        private void movePlanetOnMap(double xCord, double yCord)
        {
            
            try
            {
                _planetManager.UpdatePlanetCoordinates(_planetToBeMoved.PlanetID, xCord, yCord);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
            _planetHasBeenSelected = false;
            _removeDeletedPlanetFromMap(createOrFindPlanetElementName(_planetToBeMoved));
            lblMessage.Content = _planetToBeMoved.PlanetID + " has been moved to its new location";
            btnMessageClose.Visibility = Visibility.Visible;
            btnCancel.Visibility = Visibility.Hidden;
        }
    }
}
