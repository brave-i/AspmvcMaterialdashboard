// using System;
//using System.Collections.Generic;
// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;
// using System.Linq;
//using System.Web;

//namespace Chatison.Models
//{
//     //contact table with first name(optional), last name(optional), phone number, email,
//     //   opt in sms(1=yes/0=no) default=1, opt in email(1/0), organizationId, datecreated

//    [Table("Contacts")]
//    public class Contact : PersistentObject
//    {
//        [StringLength(250)]
//        public string FirstName { get; set; }
//        [StringLength(250)]
//        public string LastName { get; set; }
//        [StringLength(250)]
//        public string PhoneNumber { get; set; }
//        [StringLength(250)]
//        public string Email { get; set; }
//        public bool SmsOpt { get; set; }
//        public bool SmsEmail { get; set; }
//        [ForeignKey("Organization")]
//        public long OrganizationId { get; set; }
//        public Organization Organization { get; set; }
//    }
//}