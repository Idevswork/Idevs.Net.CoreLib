using Serenity.ComponentModel;

namespace Idevs.ComponentModel;

public partial class LookupFormatterAttribute : CustomFormatterAttribute
{
    public const string Key = "Idevs.LookupFormatter";

    public LookupFormatterAttribute() : base(Key)
    {
    }

    public string LookupKey
    {
        get => GetOption<string>("lookupKey") ?? string.Empty;
        set => SetOption("lookupKey", value);
    }
}