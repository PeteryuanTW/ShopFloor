using System;
using System.Collections.Generic;

namespace ShopFloor.EFModels
{
    public partial class Tag
    {
        public string TagName { get; set; } = null!;
        public int TagType { get; set; }
    }
}
