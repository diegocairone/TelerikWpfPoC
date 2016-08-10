using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Docking;

namespace WpfAppPoC_2
{
    class WinPrincipalViewModel : ViewModelBase
    {
        private DelegateCommand paisesCommand;

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

        private void AbrirPanelPaises(object param)
        {
            Console.WriteLine(value: "Abriendo paises");
            this.Panes.Add(new PaneViewModel(typeof(PanelPaises)) { Header = "Paises", InitialPosition = DockState.DockedRight, IsDocument = true });
        }
    }
}
