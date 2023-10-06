using System;
using System.Collections.Generic;

namespace ShopFloor.EFModels
{
    public partial class ActionConfig
    {
        public decimal Code { get; set; }
        public string Action { get; set; } = null!;
    }
}
