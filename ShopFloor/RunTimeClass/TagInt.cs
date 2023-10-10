using ShopFloor.EFModels;
using ShopFloor.Pages.Tag;

namespace ShopFloor.RunTimeClass
{
	public class TagInt:TagSuperClass
	{
		public int value;
		public TagInt(Tag tag) : base(tag)
		{
			if (tag.TagType == 1)
			{
				TagName = tag.TagName;
				tagType = typeof(int);
				SetValue(0);
			}
		}
		public void SetValue(int i)
		{
			value = i;
			valueStr = value.ToString();
			lastestUpdate = DateTime.Now;
		}
	}
}
