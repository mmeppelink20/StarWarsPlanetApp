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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataObjects;
using LogicLayer;
using LogicLayerInterfaces;

namespace WpfPresentation
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {

        private User _user = null;
        private UserManager _cabinManager = new UserManager();
        public Window1()
        {
            InitializeComponent();
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void windowBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();

            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            UserManager userManager = new UserManager();

            string userName = txtUsername.Text;
            string password = psdPassword.Password;

            if(userName.Length == 0)
            {
                MessageBox.Show("You must enter a username");
                txtUsername.Focus();
                return;
            }
            if (password.Length == 0)
            {
                MessageBox.Show("Please enter a password");
                psdPassword.Focus();
                return;
            }


            try
            {
                _user = userManager.LoginUser(userName, password);

                LoginUser();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }

        private void LoginUser()
        {
            MainWindow _mainWindow = new MainWindow(_user);
            _mainWindow.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnLogin.IsDefault = true;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Maximized)
            {
                this.WindowState = System.Windows.WindowState.Normal;
            }
        }
    }
}
