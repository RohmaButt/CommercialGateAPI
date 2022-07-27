using System;
using System.Collections.Generic;

namespace SecurityServiceProxy.Models
{
    public class MenuPermission
    {
        public string CssClass { get; set; }
        public string DisplayText { get; set; }
        public Guid PermissionKey { get; set; }
        public Guid TypeKey { get; set; }
        public Guid? ParentKey { get; set; }
        public string URL { get; set; }
        public List<SubPermission> SubPermissions { get; set; }
    }

    public class SubPermission
    {
        public string CssClass { get; set; }
        public string DisplayText { get; set; }
        public Guid PermissionKey { get; set; }
        public Guid TypeKey { get; set; }
        public Guid? ParentKey { get; set; }
        public string URL { get; set; }
    }
}
