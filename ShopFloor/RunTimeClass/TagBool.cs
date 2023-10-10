using ShopFloor.EFModels;

namespace ShopFloor.RunTimeClass
{
	public class TagBool:TagSuperClass
	{
		public bool value;
		public TagBool(Tag tag) : base(tag)
		{
			if (tag.TagType == 0)
			{
				TagName = tag.TagName;
				tagType = typeof(bool);
				SetValue(false);
			}
			
		}

		public void SetValue(bool b)
		{
			value = b;
			valueStr = value.ToString();
			lastestUpdate = DateTime.Now;
		}
	}
}
