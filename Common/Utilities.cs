using System;

namespace Common
{
    public class Utilities
    {
    }

    public sealed class PermissionType
    {
        private readonly Guid value;
        public static readonly PermissionType Action = new PermissionType("0401623A-5F4F-E611-825D-DC536096F252");
        public static readonly PermissionType Attribute = new PermissionType("258BCD81-5F4F-E611-825D-DC536096F252");
        public static readonly PermissionType Application = new PermissionType("3B5F6596-AE1B-E711-B298-9A2FDFB3B689");
        public static readonly PermissionType Menu = new PermissionType("3C5F6596-AE1B-E711-B298-9A2FDFB3B689");
        private PermissionType(string id)
        {
            this.value = new Guid(id);
        }

        public Guid Value
        {
            get
            {
                return value;
            }
        }


    }

}