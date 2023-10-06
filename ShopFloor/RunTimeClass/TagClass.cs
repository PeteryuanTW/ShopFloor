using System;
using ShopFloor.EFModels;
using static DevExpress.Utils.Filtering.ExcelFilterOptions;

namespace ShopFloor.RunTimeClass
{
	public class TagClass
	{
		
		public string TagName { get; set; }
        public Type tagType { get; set; }

		public dynamic value { get; set; }

		public DateTime lastestUpdate { get; set; }
		public TagClass(Tag tag)
		{
			TagName = tag.TagName;
			switch (tag.TagType)
			{
				case 0:
					tagType = typeof(bool);
					value = false;
					break;
				case 1:
					tagType = typeof(int);
					value = 0;
					break;
				case 2:
					tagType = typeof(float);
					value = 0.0;
					break;
				case 3:
					tagType = typeof(string);
					value = "";
					break;
				default:
					break;
			}
			lastestUpdate = DateTime.Now;
		}
	}
}
