//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Web;

//namespace Chatison.Models
//{
//    public class PersistentObject
//    {
//        protected PersistentObject()
//        {
//            CreationDateTime = DateTime.Now;
//        }

//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        [Key]
//        public long Id { get; set; }

//        public DateTime CreationDateTime { set; get; }
//    }
//}