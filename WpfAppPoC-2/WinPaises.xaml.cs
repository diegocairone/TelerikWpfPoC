using System;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls.DataServices;
using Telerik.Windows.Data;

namespace WpfAppPoC_2
{
    /// <summary>
    /// Interaction logic for WinPaises.xaml
    /// </summary>
    public partial class WinPaises : Window
    {
        private Boolean hayModificaciones = false;

        private Boolean resStateRadGridPaises;
        private Boolean resStateRadPager;
        private Boolean resStateRadBtnNuevo;
        private Boolean resStateRadBtnBorrar;
        private Boolean resStateRadBtnGuardar;
        private Boolean resStateRadBtnCancelar;
        private Boolean resStateRadBtnCerrar;

        public WinPaises()
        {
            InitializeComponent();
            LeerEstados();
        }

        private void LeerEstados()
        {
            this.resStateRadGridPaises = RadGridPaises.IsEnabled;
            this.resStateRadPager = RadPagerPaises.IsEnabled;
            this.resStateRadBtnNuevo = radBtnNuevo.IsEnabled;
            this.resStateRadBtnBorrar = radBtnBorrar.IsEnabled;
            this.resStateRadBtnGuardar = radBtnGuardar.IsEnabled;
            this.resStateRadBtnCancelar = radBtnCancelar.IsEnabled;
            this.resStateRadBtnCerrar = radBtnCerrar.IsEnabled;
        }

        private void Inhabilitar()
        {
            LeerEstados();

            RadGridPaises.IsEnabled = false;
            RadPagerPaises.IsEnabled = false;
            radBtnNuevo.IsEnabled = false;
            radBtnBorrar.IsEnabled = false;
            radBtnGuardar.IsEnabled = false;
            radBtnCancelar.IsEnabled = false;
            radBtnCerrar.IsEnabled = false;
        }

        private void Rehabilitar()
        {
            RadGridPaises.IsEnabled = this.resStateRadGridPaises;
            RadPagerPaises.IsEnabled = this.resStateRadPager;
            radBtnNuevo.IsEnabled = this.resStateRadBtnNuevo;
            radBtnBorrar.IsEnabled = this.resStateRadBtnBorrar;
            radBtnGuardar.IsEnabled = this.resStateRadBtnGuardar;
            radBtnCancelar.IsEnabled = this.resStateRadBtnCancelar;
            radBtnCerrar.IsEnabled = this.resStateRadBtnCerrar;
        }
        
        private void PaisesDataSource_LoadingData(object sender, LoadingDataEventArgs e)
        {
            Inhabilitar();
            BarraEstadoItem.Content = "RECIBIENDO DATOS ...";
            Console.WriteLine(value: "Data loading...");
        }

        private void PaisesDataSource_LoadedData(object sender, Telerik.Windows.Controls.DataServices.LoadedDataEventArgs e)
        {
            Rehabilitar();

            hayModificaciones = false;
            radBtnGuardar.IsEnabled = false;
            radBtnCancelar.IsEnabled = false;

            BarraEstadoItem.Content = "LISTO";
            Console.WriteLine(value: "Data loaded.");
        }

        private void PaisesDataSource_SubmittingChanges(object sender, DataServiceSubmittingChangesEventArgs e)
        {
            Inhabilitar();
            BarraEstadoItem.Content = "ENVIANDO DATOS DATOS ...";
            Console.WriteLine(value: "Submitting ...");
        }

        private void PaisesDataSource_SubmittedChanges(object sender, DataServiceSubmittedChangesEventArgs e)
        {
            Rehabilitar();
            Console.WriteLine(value: "Submitted.");
        }

        private void RadBtnNuevoClick(object sender, RoutedEventArgs e)
        {
            RadGridPaises.BeginInsert();
            hayModificaciones = true;
        }

        private void RadBtnBorrarClick(object sender, RoutedEventArgs e)
        {
            if (RadGridPaises.SelectedItems.Count == 0)
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

            this.radBtnGuardar.IsEnabled = true;
            this.radBtnCancelar.IsEnabled = true;
            this.hayModificaciones = true;
        }

        private void RadBtnGuardarClick(object sender, RoutedEventArgs e)
        {
            BarraEstadoItem.Content = "ENVIADO DATOS AL SERVIDOR";
            paisesDataSource.SubmitChanges();
        }

        private void RadBtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            paisesDataSource.RejectChanges();
        }

        private void RadBtnCerrarClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
        private void RadGridPaises_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            radBtnGuardar.IsEnabled = true;
            radBtnCancelar.IsEnabled = true;

            hayModificaciones = true;

            BarraEstadoItem.Content = "DATOS MODIFICADOS !!";
            
        }

        private void RadGridPaises_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            this.radBtnBorrar.IsEnabled = RadGridPaises.SelectedItems.Count() != 0;
        }

        private void RadGridPaises_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            Telerik.Windows.Controls.GridView.GridViewDataControl grid = e.OwnerGridViewItemsControl;
            grid.CurrentColumn = grid.Columns[0];

            hayModificaciones = true;
        }
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (hayModificaciones)
            {
                MessageBoxResult result = MessageBox.Show(messageBoxText: "Hay cambios sin guardar. ¿Seguro que desea continuar?", caption: "Cerrar ventana", button: MessageBoxButton.YesNo, icon: MessageBoxImage.Warning, defaultResult: MessageBoxResult.No);

                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

    }
}
