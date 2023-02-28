using Serenity.ComponentModel;

namespace Idevs.ComponentModel;

public partial class CheckboxFormatterAttribute : CustomFormatterAttribute
{
    public const string Key = "Idevs.CheckboxFormatter";

    public CheckboxFormatterAttribute(
        string cssClass = "",
        string trueValue = "",
        string falseValue = "",
        string trueValueIcon = "",
        string falseValueIcon = ""
    ) : base(Key)
    {
        SetOption("cssClass", cssClass);
        SetOption("trueValue", trueValue);
        SetOption("falseValue", falseValue);
        SetOption("trueValueIcon", trueValueIcon);
        SetOption("falseValueIcon", falseValueIcon);
    }
}