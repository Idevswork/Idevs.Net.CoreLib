using Serenity.ComponentModel;

namespace Idevs.ComponentModel;

public class DisplayDateTimeFormatAttribute : DisplayFormatAttribute
{
    public DisplayDateTimeFormatAttribute(bool withSeconds = false)
        : base(withSeconds ? "dd/MM/yyyy HH:mm:ss" : "dd/MM/yyyy HH:mm")
    {
    }
}
