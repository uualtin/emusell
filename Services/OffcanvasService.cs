namespace Emusell.Services;

public class OffcanvasService
{
    private bool _isOpen = false;
    public event Action? OnOffcanvasChanged;

    public bool IsOpen => _isOpen;

    public void Open()
    {
        if (!_isOpen)
        {
            _isOpen = true;
            OnOffcanvasChanged?.Invoke();
        }
    }

    public void Close()
    {
        if (_isOpen)
        {
            _isOpen = false;
            OnOffcanvasChanged?.Invoke();
        }
    }

    public void Toggle()
    {
        _isOpen = !_isOpen;
        OnOffcanvasChanged?.Invoke();
    }
}

