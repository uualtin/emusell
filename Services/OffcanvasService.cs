namespace Emusell.Services;

public class OffcanvasService
{
    private bool _isOpen;

    public bool IsOpen => _isOpen;
    public event Action? OnOffcanvasChanged;

    public void Open()
    {
        _isOpen = true;
        OnOffcanvasChanged?.Invoke();
    }

    public void Close()
    {
        _isOpen = false;
        OnOffcanvasChanged?.Invoke();
    }

    public void Toggle()
    {
        _isOpen = !_isOpen;
        OnOffcanvasChanged?.Invoke();
    }
}
