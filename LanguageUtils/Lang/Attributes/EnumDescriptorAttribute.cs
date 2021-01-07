using System;

namespace MSHC.Lang.Attributes
{
	[AttributeUsage(AttributeTargets.All)]
	public class EnumDescriptorAttribute : Attribute
	{
		public readonly string Description;
		public readonly bool Visible;

		public EnumDescriptorAttribute(string value, bool visible = true)
		{
			Description = value;
			Visible = visible;
		}
	}
}
