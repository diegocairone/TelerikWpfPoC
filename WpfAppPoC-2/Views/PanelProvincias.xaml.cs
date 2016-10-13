using Microsoft.OData.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Telerik.Windows.Data;

namespace WpfAppPoC_2.Views
{
    /// <summary>
    /// Interaction logic for PanelProvincias.xaml
    /// </summary>
    public partial class PanelProvincias : UserControl, IPanel
    {
        private Boolean hayModificaciones = false;

        private Boolean resStateRadGridProvincias;
        private Boolean resStateRadPager;
        private Boolean resStateRadBtnNuevo;
        private Boolean resStateRadBtnBorrar;
        private Boolean resStateRadBtnGuardar;
        private Boolean resStateRadBtnCancelar;
        
        public PanelProvincias()
        {
            InitializeComponent();
        }
        
        private void PaisesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Servicios.Pais pais = e.AddedItems.Cast<Servicios.Pais>().FirstOrDefault();

            if (pais != null)
            {
                FilterDescriptor df = provinciasDataSource.FilterDescriptors.Cast<FilterDescriptor>().FirstOrDefault();

                if(df == null)
                {
                    df = new FilterDescriptor(member: "paisID", filterOperator: FilterOperator.IsEqualTo, filterValue: pais.id);
                    
                    provinciasDataSource.FilterDescriptors.Add(df);

                } else
                {
                    df.Value = pais.id;
                }
            }            
        }

        private void ProvinciasDataSource_LoadingData(object sender, Telerik.Windows.Controls.DataServices.LoadingDataEventArgs e)
        {
            Inhabilitar();
            BarraEstadoItem.Content = "RECIBIENDO DATOS ...";
        }

        private void ProvinciasDataSource_LoadedData(object sender, Telerik.Windows.Controls.DataServices.LoadedDataEventArgs e)
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
            RadBtnNuevo.IsEnabled = true;
            RadBtnGuardar.IsEnabled = false;
            RadBtnCancelar.IsEnabled = false;

            PaisesComboBox.IsEnabled = true;

            BarraEstadoItem.Content = "LISTO";
        }
        
        private void ProvinciasDataSource_SubmittingChanges(object sender, Telerik.Windows.Controls.DataServices.DataServiceSubmittingChangesEventArgs e)
        {
            // NECESARIO PARA ENVIAR HTTP PUT EN LUGAR DE HTTP PATCH
            e.SaveChangesOptions = Microsoft.OData.Client.SaveChangesOptions.ReplaceOnUpdate;

            Inhabilitar();
            BarraEstadoItem.Content = "ENVIANDO DATOS DATOS ...";
        }

        private void ProvinciasDataSource_SubmittedChanges(object sender, Telerik.Windows.Controls.DataServices.DataServiceSubmittedChangesEventArgs e)
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
        }

        private void RadGridProvincias_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            Telerik.Windows.Controls.GridView.GridViewDataControl grid = e.OwnerGridViewItemsControl;
            grid.CurrentColumn = grid.Columns[0];

            hayModificaciones = true;
            PaisesComboBox.IsEnabled = false;
        }

        private void RadGridProvincias_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            RadBtnGuardar.IsEnabled = true;
            RadBtnCancelar.IsEnabled = true;

            hayModificaciones = true;
            PaisesComboBox.IsEnabled = false;

            BarraEstadoItem.Content = "DATOS MODIFICADOS !!";
        }

        private void RadGridProvincias_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            this.RadBtnBorrar.IsEnabled = RadGridProvincias.SelectedItems.Count() != 0;
        }

        private void RadBtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            RadGridProvincias.BeginInsert();
            hayModificaciones = true;
            PaisesComboBox.IsEnabled = false;
        }

        private void RadBtnBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (RadGridProvincias.SelectedItems.Count == 0)
            {
                return;
            }

            var itemsToRemove = new System.Collections.ObjectModel.ObservableCollection<object>();

            foreach (var item in RadGridProvincias.SelectedItems)
            {
                itemsToRemove.Add(item);
            }

            foreach (var item in itemsToRemove)
            {
                (RadGridProvincias.ItemsSource as Telerik.Windows.Data.DataItemCollection).Remove(item);
            }

            this.RadBtnGuardar.IsEnabled = true;
            this.RadBtnCancelar.IsEnabled = true;
            this.hayModificaciones = true;
            this.PaisesComboBox.IsEnabled = false;
        }

        private void RadBtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            BarraEstadoItem.Content = "ENVIADO DATOS AL SERVIDOR";
            provinciasDataSource.SubmitChanges();
        }

        private void RadBtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            provinciasDataSource.RejectChanges();
        }

        public Boolean HayModificaciones { get { return this.hayModificaciones; } }

        private void LeerEstados()
        {
            this.resStateRadGridProvincias = RadGridProvincias.IsEnabled;
            this.resStateRadPager = RadPagerProvincias.IsEnabled;
            this.resStateRadBtnNuevo = RadBtnNuevo.IsEnabled;
            this.resStateRadBtnBorrar = RadBtnBorrar.IsEnabled;
            this.resStateRadBtnGuardar = RadBtnGuardar.IsEnabled;
            this.resStateRadBtnCancelar = RadBtnCancelar.IsEnabled;
        }

        private void Inhabilitar()
        {
            LeerEstados();

            RadGridProvincias.IsEnabled = false;
            RadPagerProvincias.IsEnabled = false;
            RadBtnNuevo.IsEnabled = false;
            RadBtnBorrar.IsEnabled = false;
            RadBtnGuardar.IsEnabled = false;
            RadBtnCancelar.IsEnabled = false;
        }

        private void Rehabilitar()
        {
            RadGridProvincias.IsEnabled = this.resStateRadGridProvincias;
            RadPagerProvincias.IsEnabled = this.resStateRadPager;
            RadBtnNuevo.IsEnabled = this.resStateRadBtnNuevo;
            RadBtnBorrar.IsEnabled = this.resStateRadBtnBorrar;
            RadBtnGuardar.IsEnabled = this.resStateRadBtnGuardar;
            RadBtnCancelar.IsEnabled = this.resStateRadBtnCancelar;
        }

    }
}
