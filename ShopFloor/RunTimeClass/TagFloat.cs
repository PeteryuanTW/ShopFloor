using ShopFloor.EFModels;

namespace ShopFloor.RunTimeClass
{
	public class TagFloat : TagSuperClass
	{
		public float value;
		public TagFloat(Tag tag) : base(tag)
		{
			if (tag.TagType == 2)
			{
				TagName = tag.TagName;
				tagType = typeof(float);
				SetValue(0);
			}
		}
		public void SetValue(float f)
		{
			value = f;
			valueStr = value.ToString();
			lastestUpdate = DateTime.Now;
		}
	}
}
