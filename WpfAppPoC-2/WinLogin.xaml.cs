using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace WpfAppPoC_2
{
    /// <summary>
    /// Interaction logic for WinLogin.xaml
    /// </summary>
    public partial class WinLogin : Window
    {
        public WinLogin()
        {
            InitializeComponent();
        }

        private void TxtUsuario_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(TxtUsuario.Text.Length == 0 || TxtPassword.Password.Length == 0)
            {
                RadBtnAceptar.IsEnabled = false;
                return;
            }

            RadBtnAceptar.IsEnabled = true;
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (TxtUsuario.Text.Length == 0 || TxtPassword.Password.Length == 0)
            {
                RadBtnAceptar.IsEnabled = false;
                return;
            }

            RadBtnAceptar.IsEnabled = true;
        }

        private void RadBtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            Usuario usuario = Usuario.GetInstance();

            usuario.NombreUsuario = TxtUsuario.Text;
            usuario.Password = TxtPassword.Password;

            using (var wc = new WebClient())
            {
                wc.Headers.Add("Authorization: Basic " + usuario.GetEncodedAuthToken());
                wc.OpenReadCompleted += (o, a) =>
                {
                    if(a.Error == null)
                    {
                        Application.Current.MainWindow.Show();
                        Close();

                    } else
                    {
                        MessageBoxResult result = MessageBox.Show(
                            messageBoxText: a.Error.Message,
                            caption: "OCURRIO UN ERROR",
                            button: MessageBoxButton.OK,
                            icon: MessageBoxImage.Error);
                    }
                };
                wc.OpenReadAsync(new Uri(Properties.Settings.Default.EchoServiceUri));
            }
        }

        private void RadBtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Application.Current.MainWindow.Close();
        }
        
    }
}
