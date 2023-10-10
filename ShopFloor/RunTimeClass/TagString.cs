using ShopFloor.EFModels;

namespace ShopFloor.RunTimeClass
{
	public class TagString : TagSuperClass
	{
		public string value;
		public TagString(Tag tag) : base(tag)
		{
			if (tag.TagType == 3)
			{
				TagName = tag.TagName;
				tagType = typeof(string);
				SetValue("");
			}
		}
		public void SetValue(string s)
		{
			value = s;
			valueStr = value.ToString();
			lastestUpdate = DateTime.Now;
		}
	}
}
