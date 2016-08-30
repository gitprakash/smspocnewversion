using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
namespace DataModelLibrary
{
    class DBInitializer : DropCreateDatabaseIfModelChanges<Model1>
    {
        protected override void Seed(Model1 context)
        {
            IList<GenderType> GenderTypes = new List<GenderType>
            {
                new GenderType{ Name="Male"},
                new GenderType{ Name="Female"}
            };
            IList<AccountType> AccountTypes = new List<AccountType>
            {
                new AccountType{ Name="Free"},
                new AccountType{ Name="Preminum"}
            };
            IList<Role> Roles = new List<Role>
            {
                new Role{ Name="Admin",Description="Admin all access", CreatedBy="prakash",CreatedDate=DateTime.Now},
                new Role{ Name="Subscriber",Description="application users", CreatedBy="prakash",CreatedDate=DateTime.Now},
                new Role{ Name="User",Description="user under subscriber", CreatedBy="prakash",CreatedDate=DateTime.Now},

            };
            IList<Subscriber> lstsubscribers = new List<Subscriber>
            {
                new Subscriber{ Username="prakash", FirstName="prakash", Password="password", AccountTypeId=2, GenderTypeId=1, Active=true, Email="prakashr@hcl.com",
                                LastName="rajendran", Mobile=9940499650,IsActivated=true},
                new Subscriber{ Username="testuser", FirstName="test", Password="password", AccountTypeId=2, GenderTypeId=1, Active=true, Email="test@hcl.com",
                                LastName="rajendran", Mobile=9940499651,IsActivated=true}
            };

            var subscriberroles = new List<SubscriberRoles>
            {
                new SubscriberRoles{ RoleId=1,SubscriberId=1,Active=true},
                new SubscriberRoles{ RoleId=2,SubscriberId=1,Active=true},
                new SubscriberRoles{ RoleId=2,SubscriberId=2,Active=true}
            };

            var Standards = new List<Standard> { 
                
                 new Standard{  Name="6"},
                 new Standard{  Name="7"},
                 new Standard{  Name="8"},
                 new Standard{  Name="9"},
                 new Standard{  Name="10"},
                 new Standard{  Name="11"},
                 new Standard{  Name="12"},
                 new Standard{  Name="PRE-KG"},
                 new Standard{  Name="UKG"},
                 new Standard{  Name="LKG"},
            };
            var lstSection = new List<SubscriberSection> { 
                new SubscriberSection{ Subscriber = lstsubscribers[1],Section = new Section{  Name="A"}, 
                    CreatedAt =DateTime.Now,Active =true,Guid = Guid.NewGuid()}
            };

            var lststandards = new List<Standard> { 
                 new Standard{  Name="1"},
                 new Standard{  Name="2"},
                 new Standard{  Name="3"},
                 new Standard{  Name="4"},
                 new Standard{  Name="5"}
                  
            };
            var SubscriberStandards = new List<SubscriberStandards>();
            Standards.ForEach(s => { SubscriberStandards.Add(new SubscriberStandards {Guid=Guid.NewGuid(), CreatedAt = DateTime.Now, Active = true, Subscriber = lstsubscribers[1], Standard = s }); });
            lststandards.ForEach(s => { SubscriberStandards.Add(new SubscriberStandards { Guid=Guid.NewGuid(),CreatedAt = DateTime.Now, Active = true, Subscriber = lstsubscribers[0], Standard = s }); });

            var lstSubscriberStandardSections = new List<SubscriberStandardSections>();
            lstSection.ForEach(s =>
            {
                lstSubscriberStandardSections.Add(new SubscriberStandardSections
                {
                    CreatedAt = DateTime.Now,
                    Active = true,
                    SubscriberStandards = SubscriberStandards[4],
                    SubscriberSection = s
                });
            });
            lstSection.ForEach(s =>
            {
                lstSubscriberStandardSections.Add(new SubscriberStandardSections
                {
                    CreatedAt = DateTime.Now,
                    Active = true,
                    SubscriberStandards = SubscriberStandards[9],
                    SubscriberSection = s
                });
            });
            lstSection.ForEach(s =>
            {
                lstSubscriberStandardSections.Add(new SubscriberStandardSections
                {
                    CreatedAt = DateTime.Now,
                    Active = true,
                    SubscriberStandards = SubscriberStandards[11],
                    SubscriberSection = s
                });
            });
            var lstcontacts = new List<Contact>();
            for (int i = 0; i < 20; i++)
            {
                lstcontacts.Add(new Contact
                 {
                     Name = "Student" + i,
                     Active = true,
                     Mobile = 7040499650 + i,
                     BloodGroup = "AB+",
                     RollNo = "100" + i,
                     CreatedDate = DateTime.Now
                 });
            }

            var lstssc = new List<SubscriberStandardContacts>();
            //  SubscriberStandards.ForEach(s => {
            lstcontacts.ForEach(c =>
            {
                lstssc.Add(new SubscriberStandardContacts { CreatedAt = DateTime.Now, Active = true, SubscriberStandards = SubscriberStandards[4], Contact = c, SubscriberStandardSections = lstSubscriberStandardSections[0] });
            });
            //  });
            var lstsubcribertemplates = new List<SubscriberTemplate>
            {
             new SubscriberTemplate{Subscriber = lstsubscribers[1], Active = true, Guid=Guid.NewGuid(), CreatedAt=DateTime.Now, Templates = new Template
             {
                 Name = "Admission Messages",
                 Description = "Dear Parent, you are requested to submit the admission form along with the registration fees before@date@scholl"
             }},
             new SubscriberTemplate{Subscriber = lstsubscribers[1], Active = true,Guid=Guid.NewGuid(), CreatedAt=DateTime.Now, Templates = new Template
             {
                 Name = "Fees Admission Messages",
                 Description = "Dear Parent, visit the school between 9 am and 12 pm and confirm the admission by paying the fees @date @scholl"
             }},
             new SubscriberTemplate{Subscriber = lstsubscribers[1], Active = true, Guid=Guid.NewGuid(), CreatedAt=DateTime.Now,Templates = new Template
             {
                   Name = "Attendance/Behavior Messages",
                 Description = "Your ward was absent today WITHOUT PRIOR INFORMATION. Please send your ward with the Leave Letter @scholl"
             }},
             new SubscriberTemplate{Subscriber = lstsubscribers[1], Active = true,Guid=Guid.NewGuid(), CreatedAt=DateTime.Now, Templates = new Template
             {
                 Name = "Meeting Messages",
                 Description = "Dear Parent, kindly attend the Parent-Teacher Meeting scheduled on @Date from 9 am to 12 pm @scholl"
             }},
             new SubscriberTemplate{Subscriber = lstsubscribers[1], Active = true,Guid=Guid.NewGuid(), CreatedAt=DateTime.Now, Templates = new Template
             {
                Name = "Holiday Messages",
                 Description = "Dear Parent @Date will be holiday on occasion of @function. -@schoolname"
             }}
            };


            /*  var lstmessagestatus=new List<MessageStatus>
              {
                  new MessageStatus{  Name=MessageStatusEnum.NotSent},
                  new MessageStatus {Name = MessageStatusEnum.Sent},
                  new MessageStatus {Name = MessageStatusEnum.NotDelivered}
              };    
              */
            var lstsubscribermsgbalancecount = new List<SubscriberMessageBalance> {
                new SubscriberMessageBalance{Guid=Guid.NewGuid(), Subcriber=lstsubscribers[1],CreatedAt=DateTime.Now, 
                     OpeningCount=1000, RemainingCount=1000},
                new SubscriberMessageBalance{Guid=Guid.NewGuid(), Subcriber=lstsubscribers[0],CreatedAt=DateTime.Now,OpeningCount=1000, RemainingCount=1000},
            };
            var lstsubscribermsgbalancehistorycount = new List<SubscriberMessageBalanceHistory> {
                new SubscriberMessageBalanceHistory{Guid=Guid.NewGuid(),Subcriber=lstsubscribers[1], CreatedAt=DateTime.Now,   OpeningCount=1000, RemainingCount=1000},
                new SubscriberMessageBalanceHistory{Guid=Guid.NewGuid(), Subcriber=lstsubscribers[0],CreatedAt=DateTime.Now,  OpeningCount=1000, RemainingCount=1000},
            };

            context.Roles.AddRange(Roles);
            context.GenderTypes.AddRange(GenderTypes);
            context.AccountTypes.AddRange(AccountTypes);
            context.Subscribers.AddRange(lstsubscribers);
            context.SubscriberRoles.AddRange(subscriberroles);
            context.Standards.AddRange(Standards);
            context.SubscriberStandards.AddRange(SubscriberStandards);
            context.SubscriberSection.AddRange(lstSection);
            context.SubscriberStandardSections.AddRange(lstSubscriberStandardSections);
            context.Contacts.AddRange(lstcontacts);
            context.SubscriberStandardContacts.AddRange(lstssc);
            context.SubscriberTemplates.AddRange(lstsubcribertemplates);
            context.SubscriberMessageBalance.AddRange(lstsubscribermsgbalancecount);
            context.SubscriberMessageBalanceHistory.AddRange(lstsubscribermsgbalancehistorycount);
            base.Seed(context);
        }
    }
}
