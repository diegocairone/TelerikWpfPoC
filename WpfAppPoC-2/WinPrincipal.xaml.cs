using System.Linq;
using System.Windows;

namespace WpfAppPoC_2
{
    /// <summary>
    /// Interaction logic for WinPrincipal.xaml
    /// </summary>
    public partial class WinPrincipal : Window
    {
        public WinPrincipal()
        {
            InitializeComponent();

            var winLogin = new WinLogin();
            winLogin.Show();

            this.Hide();
        }

        private void RadMenuItemSalir_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            Close();
        }
        
        private void RadDocking_PreviewClose(object sender, Telerik.Windows.Controls.Docking.StateChangeEventArgs e)
        {
            Telerik.Windows.Controls.RadPane panel = e.Panes.FirstOrDefault();
            var panelContent = panel.Content as IPanel;

            if (panelContent.HayModificaciones)
            {
                MessageBoxResult result = MessageBox.Show(messageBoxText: "Hay cambios sin guardar. ¿Seguro que desea continuar?", caption: "Cerrar ventana", button: MessageBoxButton.YesNo, icon: MessageBoxImage.Warning, defaultResult: MessageBoxResult.No);

                if (result == MessageBoxResult.No)
                {
                    
                }
            }
        }        
    }
}
