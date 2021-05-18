using MudBlazor;

namespace BlazorPatient.Shared
{
    public partial class MainLayout
    {
        MudTheme _currentTheme = new();
        bool _drawerOpen = true;

        protected override void OnInitialized()
        {
            _currentTheme = _defaultTheme;
        }

        void GoHome()
        {
            NavigationManager.NavigateTo("/");
        }

        void DarkMode()
        {
            if (_currentTheme == _defaultTheme)
            {
                _currentTheme = _darkTheme;
            }
            else
            {
                _currentTheme = _defaultTheme;
            }
        }

        void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        readonly MudTheme _defaultTheme = new()
        {
            Palette = new Palette()
            {
                Background = "#ecf0f1",
                BackgroundGrey = "#d8d8d0",
                Surface = "#edebea",
                DrawerBackground = "#edebea",
                DrawerText = "rgba(255,255,255, 0.80)",
                DrawerIcon = "rgba(255,255,255, 0.80)",
                AppbarBackground = "#594AE2",
                AppbarText = "#fff",
                TextPrimary = "rgba(0,0,0, 0.70)",
                TextSecondary = "rgba(0,0,0, 0.50)",
                ActionDefault = "#fff",
                Info = "#594AE2",
                Primary = "#594AE2"

            }
        };
        readonly MudTheme _darkTheme = new()
        {
            Palette = new Palette()
            {
                Black = "#27272f",
                Background = "#32333d",
                BackgroundGrey = "#27272f",
                Surface = "#373740",
                DrawerBackground = "#27272f",
                DrawerText = "rgba(255,255,255, 0.50)",
                DrawerIcon = "rgba(255,255,255, 0.50)",
                AppbarBackground = "#27272f",
                AppbarText = "rgba(255,255,255, 0.70)",
                TextPrimary = "rgba(255,255,255, 0.70)",
                TextSecondary = "rgba(255,255,255, 0.50)",
                ActionDefault = "#adadb1",
                ActionDisabled = "rgba(255,255,255, 0.26)",
                ActionDisabledBackground = "rgba(255,255,255, 0.12)",
                Primary = "#2196F3"
            }
        };
    }
}
