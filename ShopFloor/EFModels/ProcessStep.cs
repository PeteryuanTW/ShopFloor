using System;
using System.Collections.Generic;

namespace ShopFloor.EFModels
{
    public partial class ProcessStep
    {
        public string ProcessName { get; set; } = null!;
        public int Action { get; set; }
        public string Target { get; set; } = null!;
        public string Value { get; set; } = null!;
        public int Step { get; set; }
    }
}
