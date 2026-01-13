using Microsoft.JSInterop;

namespace Emusell.Services;

public class ThemeService
{
    private bool _isDarkMode;
    private IJSRuntime? _jsRuntime;
    private bool _initialized;

    public bool IsDarkMode => _isDarkMode;
    public event Action? OnThemeChanged;

    public async Task InitializeAsync(IJSRuntime jsRuntime)
    {
        if (_initialized) return;
        
        _jsRuntime = jsRuntime;
        try
        {
            var theme = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "theme");
            _isDarkMode = theme == "dark";
            _initialized = true;
            
            // Apply theme to document
            await ApplyThemeToDocument();
        }
        catch
        {
            _isDarkMode = false;
        }
    }

    public async Task ToggleThemeAsync()
    {
        _isDarkMode = !_isDarkMode;
        
        if (_jsRuntime != null)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "theme", _isDarkMode ? "dark" : "light");
            await ApplyThemeToDocument();
        }
        
        OnThemeChanged?.Invoke();
    }

    public async Task SetDarkModeAsync(bool isDark)
    {
        _isDarkMode = isDark;
        
        if (_jsRuntime != null)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "theme", isDark ? "dark" : "light");
            await ApplyThemeToDocument();
        }
        
        OnThemeChanged?.Invoke();
    }

    private async Task ApplyThemeToDocument()
    {
        if (_jsRuntime == null) return;
        
        try
        {
            // Toggle dark-theme class on body/html
            var script = _isDarkMode 
                ? "document.documentElement.classList.add('dark-theme'); document.body.classList.add('dark-theme');"
                : "document.documentElement.classList.remove('dark-theme'); document.body.classList.remove('dark-theme');";
            
            await _jsRuntime.InvokeVoidAsync("eval", script);
        }
        catch
        {
            // Ignore JS errors
        }
    }
}
