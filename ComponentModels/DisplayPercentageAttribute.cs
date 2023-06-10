using Serenity.ComponentModel;

namespace Idevs.ComponentModel;

public class DisplayPercentageAttribute : DisplayFormatAttribute
{
    public DisplayPercentageAttribute(int scale = 2)
        : base(scale == 0 ? "#,##0 %" : $"#,##0.{string.Empty.PadRight(scale, '0')} %") { }
}
