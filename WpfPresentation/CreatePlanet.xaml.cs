using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for CreatePlanet.xaml
    /// </summary>
    public partial class CreatePlanet : Window
    {
        private PlanetManager _planetManager = new PlanetManager();

        private double _newPlanetX;
        private double _newPlanetY;
        public CreatePlanet(Window owner, double newPlanetX, double newPlanetY)
        {
            _newPlanetX = newPlanetX;
            _newPlanetY = newPlanetY;
            this.Owner = owner;
            InitializeComponent();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Maximized)
            {
                this.WindowState = System.Windows.WindowState.Normal;
            }
        }

        private void lblPlanetInfoWindowBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();

            }
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private bool _checkValidURI(string uri)
        {
            bool result = false;

            Uri uriResult;
            result = Uri.TryCreate(uri, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            return result;
        }

        private void btnSubmitPlanetCreation_Click(object sender, RoutedEventArgs e)
        {
            // planet
            try
            {
                if (txtPlanetNameInput.Text.Length >= 2 && txtPlanetNameInput.Text.Length <= 50)
                {
                    if (_planetManager.RetrievePlanetByPlanetID(txtPlanetNameInput.Text) == 0)
                    {
                        if (_checkValidURI(txtPlanetArticleInput.Text))
                        {
                            // handeled at the end of the method
                        }
                        else
                        {
                            MessageBox.Show("Invalid planet article URL.");
                            txtPlanetArticleInput.Focus();
                            return;
                        }
                        if (txtPlanetArticleInput.Text.Length >= 250)
                        {
                            MessageBox.Show("Invalid planet article URL\nMust not exceed 250 characters.");
                            txtPlanetArticleInput.Focus();
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show(txtPlanetNameInput.Text + " already exists.");
                        txtPlanetNameInput.Focus();
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Planet name must be between 2-50 characters.");
                    txtPlanetNameInput.Focus();
                    return;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }

            // region
            try
            {
                if (_planetManager.RetrieveRegionByRegionID(txtRegionNameInput.Text) == 0)
                {
                    if(txtRegionNameInput.Text.Length >= 2 && txtRegionNameInput.Text.Length <= 50)
                    {
                        
                        if (_checkValidURI(txtRegionArticleInput.Text))
                        {
                            _planetManager.AddRegionRecord(txtRegionNameInput.Text, txtRegionArticleInput.Text);
                        }
                        else
                        {
                            MessageBox.Show("Invalid region article URL.");
                            txtRegionArticleInput.Focus();
                            return;
                        }
                        if(txtRegionArticleInput.Text.Length >= 250)
                        {
                            MessageBox.Show("Invalid region article URL.\nMust not exceed 250 characters");
                            txtRegionArticleInput.Focus();
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Region name must be between 2-50 characters.");
                        txtRegionNameInput.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }

            // sector
            try
            {
                if (_planetManager.RetrieveSectorBySectorID(txtSectorNameInput.Text) == 0)
                {
                    
                    if (txtSectorNameInput.Text.Length >= 2 && txtSectorNameInput.Text.Length <= 50)
                    {
                        if (_checkValidURI(txtSectorArticleInput.Text))
                        {
                            _planetManager.AddSectorRecord(txtSectorNameInput.Text, txtRegionNameInput.Text, txtSectorArticleInput.Text);
                        }
                        else
                        {
                            MessageBox.Show("Invalid sector article URL.");
                            txtSectorArticleInput.Focus();
                            return;
                        }
                        if(txtSectorArticleInput.Text.Length >= 250)
                        {
                            MessageBox.Show("Invalid sector article URL\nMust not exceed 250 characters");
                            txtSectorArticleInput.Focus();
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sector name must be between 2-50 characters.");
                        txtSectorNameInput.Focus();
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }

            // system
            try
            {
                if (_planetManager.RetrievePlanetarySystemBySystemID(txtSystemNameInput.Text) == 0)
                {

                    if (txtSystemNameInput.Text.Length >= 2 && txtSystemNameInput.Text.Length <= 50)
                    {
                        if (_checkValidURI(txtSystemArticleInput.Text))
                        {
                            _planetManager.AddPlanetarySystemRecord(txtSystemNameInput.Text, txtSectorNameInput.Text, txtSystemArticleInput.Text);
                        }
                        else
                        {
                            MessageBox.Show("Invalid system article URL.");
                            txtSystemArticleInput.Focus();
                            return;
                        }
                        if(txtSystemArticleInput.Text.Length >= 250)
                        {
                            MessageBox.Show("Invalid system article URL\nMust not exceed 250 characters");
                            txtSystemArticleInput.Focus();
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("System name must be between 2-50 characters.");
                        txtSystemArticleInput.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }

            if(!(Regex.IsMatch(txtGridInput.Text, @"[a-zA-Z]-[0-9]+") || Regex.IsMatch(txtGridInput.Text, @"[a-zA-Z]-\d\d")))
            {
                if(!(txtGridInput.Text.Length <= 4 && txtGridInput.Text.Length >= 3))
                {
                    MessageBox.Show("Grid number must be 3-4 chracters long");
                    txtGridInput.Focus();
                    return;
                }
 
                MessageBox.Show(txtGridInput.Text + " is an invalid grid number format\nValid format ex: B-1 or A-20");
                txtGridInput.Focus();
                return;
            }


            // create the record
            try
            { 
                Planet planet = new Planet();
                planet.PlanetID = txtPlanetNameInput.Text;
                planet.SystemID = txtSystemNameInput.Text;
                planet.GridNumber = txtGridInput.Text;
                planet.PlanetArticleLink = txtPlanetArticleInput.Text;
                planet.PlanetCoordinateX = (decimal)_newPlanetX;
                planet.PlanetCoordinateY = (decimal)_newPlanetY;

                _planetManager.AddPlanetRecord(planet);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }

            this.Close();
        }

        private void btnPlanetInfoClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtCoordinates.Text = "(" + _newPlanetX.ToString() + ", " + _newPlanetY.ToString() + ")";

        }
    }
}
