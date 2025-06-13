using System.ComponentModel;

namespace Application.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum enumElement)
    {
        var type = enumElement.GetType();

        var memInfo = type.GetMember(enumElement.ToString());
        if (memInfo != null && memInfo.Length > 0)
        {
            var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attrs != null && attrs.Length > 0)
                return ((DescriptionAttribute)attrs[0]).Description;
        }

        return enumElement.ToString();
    }
}