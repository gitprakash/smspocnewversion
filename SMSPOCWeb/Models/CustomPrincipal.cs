using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace SMSPOCWeb.Models
{
    public class CustomPrincipal : IPrincipal
    {
        private readonly CustomIdentity MyIdentity;
        public CustomPrincipal(CustomIdentity _myIdentity)
        {
            MyIdentity = _myIdentity;
        }
        public IIdentity Identity
        {
            get { return MyIdentity; }
        }

        public bool IsInRole(string role)
        {
            return Roles.IsUserInRole(role);
        }
    }
}