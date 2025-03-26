using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL_Business_Objects_Layer_
{
    public class Menu
    {
        public string MenuId { get; set; }
        public string MenuName { get; set; }
        public string Li_Class { get; set; }
        public string A_Class { get; set; }
        public string URL { get; set; }
        public string Fa_Icon { get; set; }
        public string Arrow { get; set; }
        public string MenuOrder { get; set; }
        public string MenuPer { get; set; }
        public string IsApprove { get; set; }
        public string Compcode { get; set; }
        public string UserId { get; set; }
        public List<SubMenu> submenus { get; set; }
    }
    public class SubMenu
    {
        public string MenuId { get; set; }
        public string SubMenuId { get; set; }
        public string SubMenuName { get; set; }
        public string Li_Class { get; set; }
        public string A_Class { get; set; }
        public string URL { get; set; }
        public string Fa_Icon { get; set; }
        public string Arrow { get; set; }
        public string SubMenuOrder { get; set; }
        public string AddPer { get; set; }
        public string EditPer { get; set; }
        public string DeletePer { get; set; }
        public string ViewPer { get; set; }
        public string PrintPer { get; set; }
        public string SubMenuPer { get; set; }
        public string IsApprove { get; set; }
        public string Compcode { get; set; }
    }
}
