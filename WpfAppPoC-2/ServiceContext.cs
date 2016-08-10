using System;
using System.Net;

namespace WpfAppPoC_2
{
    class ServiceContext : Servicios.SDLPoC
    {
        public ServiceContext() : base(new Uri(Properties.Settings.Default.OdataServiceUri))
        {
            this.SendingRequest2 += (sender, eventArgs) =>
            {
                eventArgs.RequestMessage.SetHeader(headerName: "Authorization", headerValue: "Basic " + Usuario.GetInstance().GetEncodedAuthToken());
            };
        }
        
    }   
}
