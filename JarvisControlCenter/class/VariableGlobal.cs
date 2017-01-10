using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarvisControlCenter
{
    class VariableGlobals
    {
        private static InfosApplication infoApplication = new InfosApplication();

        public string ipFhem;
        public string portFhem;
        public string loginFhem;
        public string passFhem;

        public void getInfosAppilcation() {
            ipFhem = infoApplication.infosAppilcation("ipFhem");
            portFhem = infoApplication.infosAppilcation("portFhem");
            loginFhem = infoApplication.infosAppilcation("loginFhem");
            passFhem = infoApplication.infosAppilcation("passFhem");

        }
    }
}
