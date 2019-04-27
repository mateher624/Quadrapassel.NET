using Quadrapassel.UI.Themes.Abstraction;

namespace Quadrapassel.UI.Themes
{
    public static class ThemeManager
    {
        public static ITheme GlobalTheme { get; private set; }

        public static void SetTheme(Theme theme)
        {
            switch (theme)
            {
                case Theme.Light:
                    GlobalTheme = new GlobalThemeLight();
                    return;
                case Theme.Dark:
                    GlobalTheme = new GlobalThemeDark();
                    return;
            }
        }
    }
}
