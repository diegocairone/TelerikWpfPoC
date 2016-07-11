using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppPoC_2
{
    class PaisesContext : Servicios.SDLPoC
    {
        public PaisesContext() : base(new Uri(uriString: "http://localhost:8080/odata/PaisService.svc/"))
        {

        }
    }
}
