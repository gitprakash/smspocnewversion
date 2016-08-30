using System.Data.Entity.ModelConfiguration.Conventions;

namespace DataModelLibrary
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=POCDBconnection")
        {
            Database.SetInitializer(new DBInitializer());
           // this.Configuration.LazyLoadingEnabled = false; 
        }
        public virtual DbSet<GenderType> GenderTypes { get; set; }
        public virtual DbSet<AccountType> AccountTypes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Subscriber> Subscribers { get; set; }
        public virtual DbSet<SubscriberRoles> SubscriberRoles { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Standard> Standards { get; set; }
        public virtual DbSet<SubscriberStandards> SubscriberStandards { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<SubscriberStandardSections> SubscriberStandardSections { get; set; }
        public virtual DbSet<SubscriberStandardContacts> SubscriberStandardContacts { get; set; }
        public virtual DbSet<Template> Templates { get; set; }
        public virtual DbSet<SubscriberTemplate> SubscriberTemplates { get; set; }
      //  public virtual DbSet<MessageStatus> MessageStatus { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<SubscriberContactMessage> SubscriberContactMessage { get; set; }
        public virtual DbSet<SubscriberMessageBalance> SubscriberMessageBalance { get; set; }
        public virtual DbSet<SubscriberMessageBalanceHistory> SubscriberMessageBalanceHistory { get; set; }
        public virtual DbSet<SubscriberSection> SubscriberSection { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.GetType(a=)
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}
