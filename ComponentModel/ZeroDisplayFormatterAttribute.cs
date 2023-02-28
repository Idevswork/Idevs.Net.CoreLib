using Serenity.ComponentModel;

namespace Idevs.ComponentModel;

public partial class ZeroDisplayFormatterAttribute : CustomFormatterAttribute
{
    public const string Key = "Idevs.ZeroDisplayFormatter";

    public ZeroDisplayFormatterAttribute() : base(Key)
    {
    }

    private string displayText = "";
    public string DisplayText
    {
        get => displayText;
        set
        {
            if (value != displayText)
            {
                displayText = value;
                SetOption("displayText", value);
            }
        }
    }
}
