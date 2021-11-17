namespace DigitalerKassenBonPwa;

using MudBlazor;

public class MudCvjmTheme : MudTheme
{
    public MudCvjmTheme()
    {
        this.Palette = new Palette()
        {
            Primary = "#e40038",
            Secondary = "#60605d",
        };

        //this.Typography = new Typography()
        //{
        //    Default = new Default()
        //    {
        //        FontFamily = new[] { "Source Sans Pro", "sans-serif" }
        //    }
        //};
    }
}