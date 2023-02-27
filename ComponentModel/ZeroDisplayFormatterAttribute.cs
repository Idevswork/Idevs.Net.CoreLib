using Serenity.ComponentModel;

namespace Idevs.ComponentModel;

public partial class ZeroDisplayFormatterAttribute : CustomFormatterAttribute
{
    public const string Key = "Idevs.ZeroDisplayFormatter";

    public ZeroDisplayFormatterAttribute(string DisplayValue = "") : base(Key)
    {
    }
}
