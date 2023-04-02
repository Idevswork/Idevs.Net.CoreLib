using Serenity.ComponentModel;

namespace Idevs.ComponentModel;

public partial class DateMonthFormatterAttribute : CustomFormatterAttribute
{
    public const string Key = "Idevs.DateMonthFormatter";

    public DateMonthFormatterAttribute() : base(Key)
    {
    }

    public string Display
    {
        get => GetOption<string>("display") ?? "2-digit";
        set => SetOption("display", value);
    }

    public string Locale
    {
        get => GetOption<string>("locale") ?? "en";
        set => SetOption("locale", value);
    }
}