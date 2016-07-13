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
        private Boolean hayModificaciones = false;

        public WinPaises()
        {
            InitializeComponent();
        }
        
        private void RadGridView_BeginningEdit(object sender, Telerik.Windows.Controls.GridViewBeginningEditRoutedEventArgs e)
        {
            this.hayModificaciones = true;

            this.radBtnGuardar.IsEnabled = true;
            this.radBtnCancelar.IsEnabled = true;

            BarraEstadoItem.Content = "DATOS MODIFICADOS !!";
        }

        private void RadBtnGuardarClick(object sender, RoutedEventArgs e)
        {
            paisesDataSource.SubmitChanges();
            hayModificaciones = false;
        }

        private void RadBtnCancelarClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void RadGridPaises_DataLoading(object sender, Telerik.Windows.Controls.GridView.GridViewDataLoadingEventArgs e)
        {
            this.hayModificaciones = false;

            this.radBtnNuevo.IsEnabled = false;
            this.radBtnBorrar.IsEnabled = false;
            this.radBtnGuardar.IsEnabled = false;
            this.radBtnCancelar.IsEnabled = false;

            BarraEstadoItem.Content = "RECIBIENDO DATOS ...";
            System.Diagnostics.Debug.WriteLine(message: "Recibiendo..");
        }

        private void RadGridPaises_DataLoaded(object sender, EventArgs e)
        {
            this.hayModificaciones = false;

            this.radBtnNuevo.IsEnabled = true;
            this.radBtnBorrar.IsEnabled = false;
            this.radBtnGuardar.IsEnabled = false;
            this.radBtnCancelar.IsEnabled = false;

            BarraEstadoItem.Content = "LISTO";
            System.Diagnostics.Debug.WriteLine(message: "Listo");
        }

        private void RadGridPaises_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            this.radBtnBorrar.IsEnabled = RadGridPaises.SelectedItems.Count() != 0;
            System.Diagnostics.Debug.WriteLine(format: "Filas seleccionadas {0}", args: new object[] { e.AddedItems.Count() });
        }

        private void RadBtnNuevoClick(object sender, RoutedEventArgs e)
        {
            RadGridPaises.BeginInsert();
        }

        private void RadBtnBorrarClick(object sender, RoutedEventArgs e)
        {
            if(RadGridPaises.SelectedItems.Count == 0)
            {
                return;
            }

            var itemsToRemove = new System.Collections.ObjectModel.ObservableCollection<object>();

            foreach (var item in RadGridPaises.SelectedItems)
            {
                itemsToRemove.Add(item);
            }

            foreach (var item in itemsToRemove)
            {
                (RadGridPaises.ItemsSource as Telerik.Windows.Data.DataItemCollection).Remove(item);
            }

            paisesDataSource.SubmitChanges();
            hayModificaciones = false;
        }

        private void RadGridPaises_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            var grid = e.OwnerGridViewItemsControl;
            grid.CurrentColumn = grid.Columns[0];
        }
    }
}
