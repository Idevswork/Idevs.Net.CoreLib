using Serenity.ComponentModel;

namespace Idevs.ComponentModel;

public partial class CheckboxFormatterAttribute : CustomFormatterAttribute
{
    public const string Key = "Idevs.CheckboxFormatter";

    public CheckboxFormatterAttribute() : base(Key)
    {
    }

    private string cssClass = "";
    public string CssClass
    {
        get => cssClass;
        set
        {
            if (value != cssClass)
            {
                cssClass = value;
                SetOption("cssClass", value);
            }
        }
    }

    private string trueText = "";
    public string TrueText
    {
        get => trueText;
        set
        {
            if (value != trueText)
            {
                trueText = value;
                SetOption("trueText", value);
            }
        }
    }

    private string falseText = "";
    public string FalseText
    {
        get => falseText;
        set
        {
            if (value != falseText)
            {
                falseText = value;
                SetOption("falseText", value);
            }
        }
    }

    private string trueValueIcon = "";
    public string TrueValueIcon
    {
        get => trueValueIcon;
        set
        {
            if (value != trueValueIcon)
            {
                trueValueIcon = value;
                SetOption("trueValueIcon", value);
            }
        }
    }

    private string falseValueIcon = "";
    public string FalseValueIcon
    {
        get => falseValueIcon;
        set
        {
            if (value != falseValueIcon)
            {
                falseValueIcon = value;
                SetOption("falseValueIcon", value);
            }
        }
    }
}