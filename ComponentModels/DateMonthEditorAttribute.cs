using Serenity.ComponentModel;

namespace Idevs.ComponentModel;

public partial class DateMonthEditorAttribute : CustomEditorAttribute
{
    public const string Key = "DateMonthEditor";

    public DateMonthEditorAttribute()
        : base(Key)
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

    public bool Descending
    {
        get => GetOption<bool>("descending");
        set => SetOption("descending", value);
    }
}
