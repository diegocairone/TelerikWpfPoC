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

namespace WpfAppPoC_2
{
    /// <summary>
    /// Interaction logic for WinPaises.xaml
    /// </summary>
    public partial class WinPaises : Window
    {

        public WinPaises()
        {
            InitializeComponent();
        }

        private void HabilitarCmds(Boolean estaEditando)
        {
            radBtnNuevo.IsEnabled = !estaEditando;
            radBtnBorrar.IsEnabled = !estaEditando;
            radBtnGuardar.IsEnabled = !estaEditando;
            radBtnCancelar.IsEnabled = !estaEditando;
        }

        private void RadGridView_BeginningEdit(object sender, Telerik.Windows.Controls.GridViewBeginningEditRoutedEventArgs e)
        {
            HabilitarCmds(estaEditando: true);
        }

        private void RadGridPaises_RowEditEnded(object sender, Telerik.Windows.Controls.GridViewRowEditEndedEventArgs e)
        {
            HabilitarCmds(estaEditando: false);
        }

        private void RadBtnGuardarClick(object sender, RoutedEventArgs e)
        {
            paisesDataSource.SubmitChanges();
        }
        
    }
}
