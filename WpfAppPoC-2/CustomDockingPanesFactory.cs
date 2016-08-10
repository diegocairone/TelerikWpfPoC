using System.Linq;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Docking;

namespace WpfAppPoC_2
{
    class CustomDockingPanesFactory : DockingPanesFactory
    {
        protected override void AddPane(RadDocking radDocking, RadPane pane)
        {
            var paneModel = pane.DataContext as PaneViewModel;
            if (paneModel != null && !(paneModel.IsDocument))
            {
                var group = radDocking.SplitItems.ToList().FirstOrDefault(i => i.Control.Name == "mainGroup") as RadPaneGroup;
                if (group != null)
                {
                    group.Items.Add(pane);
                }
                return;
            }

            base.AddPane(radDocking, pane);
        }

        protected override RadPane CreatePaneForItem(object item)
        {
            var viewModel = item as PaneViewModel;
            if (viewModel != null)
            {
                RadPane pane = viewModel.IsDocument ? new RadDocumentPane() : new RadPane();
                pane.DataContext = item;
                pane.Header = viewModel.Header;
                RadDocking.SetSerializationTag(pane, viewModel.Header);
                if (viewModel.ContentType != null)
                {
                    pane.Content = System.Activator.CreateInstance(viewModel.ContentType);
                }

                return pane;
            }

            return base.CreatePaneForItem(item);
        }

        protected override void RemovePane(RadPane pane)
        {
            pane.DataContext = null;
            pane.Content = null;
            pane.ClearValue(RadDocking.SerializationTagProperty);
            pane.RemoveFromParent();
        }
    }
}
