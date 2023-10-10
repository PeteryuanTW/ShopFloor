using System;
using ShopFloor.EFModels;
using static DevExpress.Utils.Filtering.ExcelFilterOptions;

namespace ShopFloor.RunTimeClass
{
	public class TagSuperClass
	{
		
		public string TagName { get; set; }
        public Type tagType { get; set; }

		public string valueStr { get; set; }

		public DateTime lastestUpdate { get; set; }
		public TagSuperClass(Tag tag)
		{
		}
	}
}
