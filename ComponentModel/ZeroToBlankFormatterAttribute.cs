using Serenity.ComponentModel;

namespace Idevs.ComponentModel;

public partial class ZeroToBlankFormatterAttribute : CustomFormatterAttribute
{
    public const string Key = "Idevs.ZeroToBlankFormatter";

    public ZeroToBlankFormatterAttribute() : base(Key)
    {
    }
}
