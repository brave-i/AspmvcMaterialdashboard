using Chatison.Entities;
using Chatison.Utilities;
using Chatison.ViewModels.Admin.Contact;
using System.Collections.Generic;
using System.Linq;

namespace Chatison.Factory
{
    public class ContactFactory
    {
        public static Contact CreateContact(AddContactVm model, string userId)
        {
            var contact = new Contact
            {
                UserId = userId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MobileProviderId = model.ProviderId,
                IsOptOut = model.IsOptOut,
                CreatedAt = Utility.GetDateTime(),
                Source = model.Source
            };

            if (model.GroupIds == null || !model.GroupIds.Any())
            {
                return contact;
            }

            contact.GroupContacts = new List<GroupContact>();

            foreach (var groupId in model.GroupIds.Distinct())
            {
                contact.GroupContacts.Add(new GroupContact
                {
                    UserId = userId,
                    GroupId = groupId,
                    CreatedAt = Utility.GetDateTime()
                });
            }

            return contact;
        }

        public static List<int> CreateContact(Contact contact, EditContactVm model)
        {
            contact.FirstName = model.FirstName;
            contact.LastName = model.LastName;
            contact.MobileProviderId = model.ProviderId;
            contact.IsOptOut = model.IsOptOut;
            contact.UpdatedAt = Utility.GetDateTime();

            if (contact.GroupContacts == null)
            {
                contact.GroupContacts = new List<GroupContact>();
            }

            if (model.GroupIds == null || !model.GroupIds.Any())
            {
                return null;
            }

            model.GroupIds = model.GroupIds.Distinct().ToList();

            var removedGroups = contact.GroupContacts.Select(x => x.GroupId).Except(model.GroupIds).ToList();
            var addedGroups = model.GroupIds.Except(contact.GroupContacts.Select(x => x.GroupId)).ToList();

            foreach (var groupId in addedGroups)
            {
                contact.GroupContacts.Add(new GroupContact
                {
                    UserId = contact.UserId,
                    GroupId = groupId,
                    CreatedAt = Utility.GetDateTime()
                });
            }

            return removedGroups;
        }
    }
}
