namespace Chatison.Utilities
{
    public class Constants
    {
        public enum RecordStatus { Created = 1, Active = 2, Inactive = 3, Deleted = 4 }

        public enum ResponseStatus { Success, Error }

        public enum ContactSource { Manually, Imported }

        public const string DateFormat = "MM/dd/yyyy";

        public struct UserRoles
        {
            public const string Admin = "admin";
            public const string User = "user";
        }
    }
}
