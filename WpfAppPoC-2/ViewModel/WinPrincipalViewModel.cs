using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Docking;

namespace WpfAppPoC_2
{
    class WinPrincipalViewModel : ViewModelBase
    {
        private DelegateCommand paisesCommand;
        private DelegateCommand provinciasCommand;

        public WinPrincipalViewModel()
        {
            this.Panes = new ObservableCollection<PaneViewModel>();
        }

        public ObservableCollection<PaneViewModel> Panes { get; set; }

        public ICommand PaisesCommand
        {
            get
            {
                if (paisesCommand == null)
                    paisesCommand = new DelegateCommand(this.AbrirPanelPaises);

                return paisesCommand;
            }
        }

        public ICommand ProvinciasCommand
        {
            get
            {
                if (provinciasCommand == null)
                    provinciasCommand = new DelegateCommand(this.AbrirPanelProvincias);

                return provinciasCommand;
            }
        }
        
        private void AbrirPanelPaises(object param)
        {
            this.Panes.Add(new PaneViewModel(typeof(Views.PanelPaises)) { Header = "Paises", InitialPosition = DockState.DockedRight, IsDocument = true });
        }

        private void AbrirPanelProvincias(object param)
        {
            this.Panes.Add(new PaneViewModel(typeof(Views.PanelProvincias)) { Header = "Provincias", InitialPosition = DockState.DockedRight, IsDocument = true });
        }
    }
}
