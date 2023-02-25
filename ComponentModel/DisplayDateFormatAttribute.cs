using Serenity.ComponentModel;

namespace Idevs.ComponentModel;

public class DisplayDateFormatAttribute : DisplayFormatAttribute
{
    public DisplayDateFormatAttribute() : base("dd/MM/yyyy")
    {
    }
}
