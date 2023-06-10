using Serenity.ComponentModel;

namespace Idevs.ComponentModel;

public class DisplayPercentageAttribute : DisplayFormatAttribute
{
    public DisplayPercentageAttribute()
        : base("#,##0.00 %") { }
}
