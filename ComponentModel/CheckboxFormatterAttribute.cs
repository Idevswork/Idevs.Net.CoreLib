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
    }
}