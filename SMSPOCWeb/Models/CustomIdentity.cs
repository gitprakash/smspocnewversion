using DataModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace SMSPOCWeb.Models
{
    
  public class CustomIdentity : IIdentity
        {
            public IIdentity Identity { get; set; }
            public SubscriberViewModel User { get; set; }

            public CustomIdentity(SubscriberViewModel user)
            {
                Identity = new GenericIdentity(user.Username);
                User = user;
            }

            public string AuthenticationType
            {
                get { return Identity.AuthenticationType; }
            }

            public bool IsAuthenticated
            {
                get { return Identity.IsAuthenticated; }
            }

            public string Name
            {
                get { return Identity.Name; }
            }
        }
    
}