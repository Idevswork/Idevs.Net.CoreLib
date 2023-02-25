using Serenity.ComponentModel;

namespace Idevs.ComponentModel;

public partial class CheckboxFormatterAttribute : CustomFormatterAttribute
{
    public const string Key = "Idevs.CheckboxFormatter";

    public CheckboxFormatterAttribute() : base(Key)
    {
    }
}