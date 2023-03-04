using Serenity.ComponentModel;

namespace Idevs.ComponentModel;

public partial class ZeroDisplayFormatterAttribute : CustomFormatterAttribute
{
    public const string Key = "Idevs.ZeroDisplayFormatter";

    public ZeroDisplayFormatterAttribute() : base(Key)
    {
    }

    public string DisplayText
    {
        get => GetOption<string>("displayText") ?? string.Empty;
        set => SetOption("displayText", value);
    }
}
