using Newtonsoft.Json;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls.DataServices;

namespace WpfAppPoC_2
{
    /// <summary>
    /// Interaction logic for PanelPaises.xaml
    /// </summary>
    public partial class PanelPaises : UserControl
    {
        private Boolean hayModificaciones = false;

        private Boolean resStateRadGridPaises;
        private Boolean resStateRadPager;
        private Boolean resStateRadBtnNuevo;
        private Boolean resStateRadBtnBorrar;
        private Boolean resStateRadBtnGuardar;
        private Boolean resStateRadBtnCancelar;

        public PanelPaises()
        {
            InitializeComponent();
            LeerEstados();
        }

        public Boolean HayModificaciones { get { return this.hayModificaciones; } } 

        private void LeerEstados()
        {
            this.resStateRadGridPaises = RadGridPaises.IsEnabled;
            this.resStateRadPager = RadPagerPaises.IsEnabled;
            this.resStateRadBtnNuevo = radBtnNuevo.IsEnabled;
            this.resStateRadBtnBorrar = radBtnBorrar.IsEnabled;
            this.resStateRadBtnGuardar = radBtnGuardar.IsEnabled;
            this.resStateRadBtnCancelar = radBtnCancelar.IsEnabled;
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
        }

        private void Rehabilitar()
        {
            RadGridPaises.IsEnabled = this.resStateRadGridPaises;
            RadPagerPaises.IsEnabled = this.resStateRadPager;
            radBtnNuevo.IsEnabled = this.resStateRadBtnNuevo;
            radBtnBorrar.IsEnabled = this.resStateRadBtnBorrar;
            radBtnGuardar.IsEnabled = this.resStateRadBtnGuardar;
            radBtnCancelar.IsEnabled = this.resStateRadBtnCancelar;
        }

        private void PaisesDataSource_LoadingData(object sender, LoadingDataEventArgs e)
        {
            Inhabilitar();
            BarraEstadoItem.Content = "RECIBIENDO DATOS ...";
            Console.WriteLine(value: "Data loading...");
        }

        private void PaisesDataSource_LoadedData(object sender, Telerik.Windows.Controls.DataServices.LoadedDataEventArgs e)
        {
            if (e.HasError)
            {
                e.MarkErrorAsHandled();

                if (e.Error.InnerException is Microsoft.OData.Client.DataServiceClientException)
                {
                    var ex = e.Error.GetBaseException() as Microsoft.OData.Client.DataServiceClientException;

                    String jsonResponse = ex.Message;
                    OdataServiceSpringResponse response = JsonConvert.DeserializeObject<OdataServiceSpringResponse>(jsonResponse);

                    MessageBoxResult result = MessageBox.Show(
                        messageBoxText: response.Message,
                        caption: "OCURRIO UN ERROR",
                        button: MessageBoxButton.OK,
                        icon: MessageBoxImage.Error);

                }
                else if (e.Error.InnerException is Microsoft.OData.Client.DataServiceTransportException)
                {
                    var ex = e.Error.InnerException as Microsoft.OData.Client.DataServiceTransportException;

                    MessageBoxResult result = MessageBox.Show(
                        messageBoxText: ex.Message,
                        caption: "OCURRIO UN ERROR",
                        button: MessageBoxButton.OK,
                        icon: MessageBoxImage.Error);
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show(
                        messageBoxText: e.Error.Message,
                        caption: "OCURRIO UN ERROR",
                        button: MessageBoxButton.OK,
                        icon: MessageBoxImage.Error);
                }
            }

            Rehabilitar();

            hayModificaciones = false;
            radBtnNuevo.IsEnabled = true;
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
            if (e.HasError)
            {
                e.MarkErrorAsHandled();

                if (e.Error.InnerException is Microsoft.OData.Client.DataServiceClientException)
                {
                    var ex = e.Error.GetBaseException() as Microsoft.OData.Client.DataServiceClientException;

                    String jsonResponse = ex.Message;
                    String message;

                    switch (ex.StatusCode)
                    {
                        case 401:
                            OdataServiceSpringResponse response401 = JsonConvert.DeserializeObject<OdataServiceSpringResponse>(jsonResponse);
                            message = response401.Message;
                            break;
                        default:
                            OdataServiceSdlResponse response = JsonConvert.DeserializeObject<OdataServiceSdlResponse>(jsonResponse);
                            message = response.Error.Message;

                            if (message.ToLower().Contains(value: "access is denied"))
                            {
                                message = "ACCESO DENEGADO";
                            }

                            break;
                    }

                    MessageBoxResult result = MessageBox.Show(
                        messageBoxText: message,
                        caption: "OCURRIO UN ERROR",
                        button: MessageBoxButton.OK,
                        icon: MessageBoxImage.Error);

                }
                else if (e.Error.InnerException is Microsoft.OData.Client.DataServiceTransportException)
                {
                    var ex = e.Error.InnerException as Microsoft.OData.Client.DataServiceTransportException;

                    MessageBoxResult result = MessageBox.Show(
                        messageBoxText: ex.Message,
                        caption: "OCURRIO UN ERROR",
                        button: MessageBoxButton.OK,
                        icon: MessageBoxImage.Error);
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show(
                        messageBoxText: e.Error.Message,
                        caption: "OCURRIO UN ERROR",
                        button: MessageBoxButton.OK,
                        icon: MessageBoxImage.Error);
                }
            }


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
            //Close();
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
        
    }
}
