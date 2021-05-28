using System.Threading.Tasks;
using MudBlazor;

namespace BlazorPatient.Shared
{
    public partial class MainLayout
    {
        private MudTheme _currentTheme = new();
        private bool _drawerOpen = true;
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            var isDarkmode = await localStorage.GetItemAsync<bool>("Darkmode");
            _currentTheme = isDarkmode ? _darkTheme : _defaultTheme;
            StateHasChanged();
        }

        private async Task DarkMode()
        {
            _currentTheme = _currentTheme == _defaultTheme ? _darkTheme : _defaultTheme;
            await localStorage.SetItemAsync("Darkmode",_currentTheme == _darkTheme);
        }

        private void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        private readonly MudTheme _defaultTheme = new()
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

        private readonly MudTheme _darkTheme = new()
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
