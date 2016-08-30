using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Repositorylibrary;
using DataModelLibrary;
using DataServiceLibrary;
using System.Data.Entity;

namespace SMSPOCWeb.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            container.RegisterType<DbContext, Model1>();
            container.RegisterType<IGenericRepository<Role>, Genericrepository<Role>>();
            container.RegisterType<IGenericRepository<Subscriber>, Genericrepository<Subscriber>>();
            container.RegisterType<IGenericRepository<SubscriberRoles>, Genericrepository<SubscriberRoles>>();
            container.RegisterType<IGenericRepository<AccountType>, Genericrepository<AccountType>>();
            container.RegisterType<IGenericRepository<GenderType>, Genericrepository<GenderType>>();
            container.RegisterType<IGenericRepository<Contact>, Genericrepository<Contact>>();
            container.RegisterType<IGenericRepository<SubscriberStandards>, Genericrepository<SubscriberStandards>>();
            container.RegisterType<IGenericRepository<SubscriberStandardSections>, Genericrepository<SubscriberStandardSections>>();
            container.RegisterType<IGenericRepository<SubscriberStandardContacts>, Genericrepository<SubscriberStandardContacts>>();
            container.RegisterType<IGenericRepository<SubscriberTemplate>, Genericrepository<SubscriberTemplate>>();
            container.RegisterType<IGenericRepository<SubscriberContactMessage>, Genericrepository<SubscriberContactMessage>>();
            container.RegisterType<IGenericRepository<MessageStatus>, Genericrepository<MessageStatus>>();
            container.RegisterType<IGenericRepository<SubscriberMessageBalance>, Genericrepository<SubscriberMessageBalance>>();
            container.RegisterType<IGenericRepository<SubscriberMessageBalanceHistory>, Genericrepository<SubscriberMessageBalanceHistory>>();
            container.RegisterType<IGenericRepository<SubscriberSection>, Genericrepository<SubscriberSection>>();
            container.RegisterType<IRoleService, RoleService>();
            container.RegisterType<IAccountService, AccountService>();
            container.RegisterType<IUserRoleService, UserRoleService>();
            container.RegisterType<IContactService, ContactService>();
            container.RegisterType<ISubscriberStandardService, SubscriberStandardService>();
            container.RegisterType<ITemplateService, TemplateService>();
            container.RegisterType<IMessageService, MessageService>();


        }
    }
}
