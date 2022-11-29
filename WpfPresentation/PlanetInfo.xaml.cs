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
using System.Diagnostics;

namespace WpfPresentation
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        private PlanetVM _selectedPlanet = null;
        private User _user = null;

        public Window2(PlanetVM selectedPlanet, Window owner, User user)
        {
            InitializeComponent();
            _user = user;
            _selectedPlanet = selectedPlanet;
            this.Owner = owner;
        }

        private void btnPlanetInfoClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lblPlanetInfoWindowBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();

            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Maximized)
            {
                this.WindowState = System.Windows.WindowState.Normal;
            }
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (String role in _user.Roles)
            {
                if (role == "Admin")
                {

                }
                if (role == "Trusted User")
                {

                }
                if (role == "New User")
                {

                }
            }
            try
            {
                hypPlanet.Inlines.Clear();
                hypPlanet.Inlines.Add(_selectedPlanet.PlanetID);
                hypPlanet.NavigateUri = new Uri(_selectedPlanet.PlanetArticleLink);

                hypRegion.Inlines.Clear();
                hypRegion.Inlines.Add(_selectedPlanet.RegionID);
                hypRegion.NavigateUri = new Uri(_selectedPlanet.RegionArticleLink);

                hypSector.Inlines.Clear();
                hypSector.Inlines.Add(_selectedPlanet.SectorID);
                hypSector.NavigateUri = new Uri(_selectedPlanet.SectorArticleLink);

                hypSystem.Inlines.Clear();
                hypSystem.Inlines.Add(_selectedPlanet.SystemID);
                hypSystem.NavigateUri = new Uri(_selectedPlanet.SystemArticleLink);

                txtGrid.Text = _selectedPlanet.GridNumber;

                txtCoordinates.Text = "(" + _selectedPlanet.PlanetCoordinateX.ToString() + ", " + _selectedPlanet.PlanetCoordinateY.ToString() + ")";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }

        private void btnSubmitPlanetCreation_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void btnPlanetMove_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
