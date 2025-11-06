using Microsoft.JSInterop;

namespace Emusell.Services;

public class ThemeService
{
    private bool _isDarkMode = false;
    private IJSRuntime? _jsRuntime;
    private const string CookieName = "emusell-theme";
    private const int CookieExpiryDays = 365;

    public event Action? OnThemeChanged;

    public bool IsDarkMode => _isDarkMode;

    public async Task InitializeAsync(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        await LoadThemeFromCookie();
    }

    private async Task LoadThemeFromCookie()
    {
        if (_jsRuntime == null) return;

        try
        {
            var theme = await _jsRuntime.InvokeAsync<string>("cookieHelper.getCookie", CookieName);
            if (!string.IsNullOrEmpty(theme))
            {
                _isDarkMode = theme == "dark";
                OnThemeChanged?.Invoke();
            }
        }
        catch
        {
            // Cookie okunamazsa varsayılan değer kullanılır
        }
    }

    private async Task SaveThemeToCookie()
    {
        if (_jsRuntime == null) return;

        try
        {
            var theme = _isDarkMode ? "dark" : "light";
            await _jsRuntime.InvokeVoidAsync("cookieHelper.setCookie", CookieName, theme, CookieExpiryDays);
        }
        catch
        {
            // Cookie yazılamazsa sessizce devam et
        }
    }

    public async Task ToggleTheme()
    {
        _isDarkMode = !_isDarkMode;
        await SaveThemeToCookie();
        OnThemeChanged?.Invoke();
    }

    public async Task SetTheme(bool isDark)
    {
        if (_isDarkMode != isDark)
        {
            _isDarkMode = isDark;
            await SaveThemeToCookie();
            OnThemeChanged?.Invoke();
        }
    }
}

