using Serenity.ComponentModel;

namespace Idevs.ComponentModel;

public class DisplayTimeFormatAttribute : DisplayFormatAttribute
{
    public DisplayTimeFormatAttribute(bool withSeconds = false)
        : base(withSeconds ? "HH:mm:ss" : "HH:mm")
    {
    }
}
