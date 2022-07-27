using SecurityServiceProxy.Models;
using System.Collections.Generic;

namespace SecurityServiceProxy.ExtendedModels
{
    //convert Security Service Permissions model to Customized PermissionModel as per requirements of CommercialGate application using Adapter pattern
    public static class PermissionModelMapper
    {
        public static List<MenuPermission> CustomizeAdminMenuList(this List<dynamic> mainMenus)
        {
            List<MenuPermission> MenuList = new List<MenuPermission>();
            foreach (var mainMenu in mainMenus)
            {
                MenuPermission Menupermission = new MenuPermission
                {
                    PermissionKey = mainMenu.Parent.PermissionKey,
                    CssClass = mainMenu.Parent.CssClass,
                    DisplayText = mainMenu.Parent.DisplayText,
                    URL = mainMenu.Parent.URL,
                    TypeKey = mainMenu.Parent.TypeKey,
                    ParentKey = mainMenu.Parent.ParentKey
                };
                MenuList.Add(Menupermission);
            }
            return MenuList;
        }

        public static List<MenuPermission> CustomizeMenuList(this List<dynamic> mainMenus)
        {
            List<MenuPermission> MenuList = new List<MenuPermission>();
            foreach (var mainMenu in mainMenus)
            {
                MenuPermission Menupermission = new MenuPermission
                {
                    PermissionKey = mainMenu.Parent.PermissionKey,
                    CssClass = mainMenu.Parent.CssClass.Replace("tooltip", ""),
                    DisplayText = mainMenu.Parent.DisplayText,
                    URL = mainMenu.Parent.URL.Replace("~", ""),// "",
                    TypeKey = mainMenu.Parent.TypeKey,
                    ParentKey = mainMenu.Parent.ParentKey
                };
                List<SubPermission> ChildListobj = new List<SubPermission>();
                SubPermission Childobj = new SubPermission()
                {
                    PermissionKey = mainMenu.Parent.PermissionKey,
                    CssClass = mainMenu.Parent.CssClass.Replace("tooltip", ""),
                    DisplayText = mainMenu.Parent.DisplayText,
                    URL = mainMenu.Parent.URL.Replace("~", ""),
                    TypeKey = mainMenu.Parent.TypeKey,
                    ParentKey = mainMenu.Parent.ParentKey
                };
                ChildListobj.Add(Childobj);
                foreach (var submenu in mainMenu.Sub)
                {
                    Childobj = new SubPermission()
                    {
                        PermissionKey = submenu.PermissionKey,
                        CssClass = submenu.CssClass.Replace("tooltip", ""),
                        DisplayText = submenu.DisplayText,
                        URL = submenu.URL.Replace("~", ""),
                        TypeKey = submenu.TypeKey,
                        ParentKey = submenu.ParentKey
                    };
                    ChildListobj.Add(Childobj);
                }
                Menupermission.SubPermissions = new List<SubPermission>();
                Menupermission.SubPermissions.AddRange(ChildListobj);
                MenuList.Add(Menupermission);
            }
            return MenuList;
        }

    }
}
