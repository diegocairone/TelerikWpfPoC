using System;
using System.Net;

namespace WpfAppPoC_2
{
    class ServiceContext : Servicios.SDLPoC
    {
        public ServiceContext() : base(new Uri(Properties.Settings.Default.RestServiceUri))
        {
            this.SendingRequest2 += (sender, eventArgs) =>
            {
                var nc = new NetworkCredential(userName: "admin", password: "qwerty123");

                String token = nc.UserName + ":" + nc.Password;

                String encoded = System
                    .Convert
                    .ToBase64String(System.Text.Encoding.GetEncoding(name: "ISO-8859-1")
                    .GetBytes(token));

                eventArgs.RequestMessage.SetHeader(headerName: "Authorization", headerValue: "Basic " + encoded);
            };
        }
        
    }   
}
