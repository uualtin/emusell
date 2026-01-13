using Emusell.Models;
using Microsoft.JSInterop;
using System.Text.Json;

namespace Emusell.Services;

public class AuthService
{
    private readonly UserService _userService;
    private User? _currentUser;
    private IJSRuntime? _jsRuntime;
    private bool _initialized = false;

    public AuthService(UserService userService)
    {
        _userService = userService;
    }

    public User? CurrentUser => _currentUser;
    public bool IsAuthenticated => _currentUser != null;
    public bool IsAdmin => _currentUser?.Role == UserRole.Admin;
    public bool IsSeller => _currentUser?.Role == UserRole.Seller;
    public bool IsBuyer => _currentUser?.Role == UserRole.Buyer;

    public event Action? OnAuthStateChanged;

    public async Task InitializeAsync(IJSRuntime jsRuntime)
    {
        if (_initialized) return;
        
        _jsRuntime = jsRuntime;
        _initialized = true;
        
        // Try to restore user from localStorage
        await RestoreUserFromStorageAsync();
    }

    private async Task RestoreUserFromStorageAsync()
    {
        if (_jsRuntime == null) return;
        
        try
        {
            var userJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "currentUser");
            if (!string.IsNullOrEmpty(userJson))
            {
                var storedUser = JsonSerializer.Deserialize<StoredUser>(userJson);
                if (storedUser != null && !string.IsNullOrEmpty(storedUser.Id))
                {
                    // Verify user still exists and is active
                    var user = await _userService.GetUserByIdAsync(storedUser.Id);
                    if (user != null && user.IsActive)
                    {
                        _currentUser = user;
                        OnAuthStateChanged?.Invoke();
                    }
                    else
                    {
                        // User no longer valid, clear storage
                        await ClearStorageAsync();
                    }
                }
            }
        }
        catch
        {
            // Ignore errors, user will need to login again
        }
    }

    private async Task SaveUserToStorageAsync()
    {
        if (_jsRuntime == null || _currentUser == null) return;
        
        try
        {
            var storedUser = new StoredUser
            {
                Id = _currentUser.Id,
                Email = _currentUser.Email,
                FullName = _currentUser.FullName,
                Role = _currentUser.Role.ToString()
            };
            
            var userJson = JsonSerializer.Serialize(storedUser);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "currentUser", userJson);
        }
        catch
        {
            // Ignore storage errors
        }
    }

    private async Task ClearStorageAsync()
    {
        if (_jsRuntime == null) return;
        
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "currentUser");
        }
        catch
        {
            // Ignore errors
        }
    }

    public async Task<(bool Success, string Message)> LoginAsync(string email, string password)
    {
        var user = await _userService.GetUserByEmailAsync(email);
        
        if (user == null)
            return (false, "Kullanıcı bulunamadı.");

        if (!user.IsActive)
            return (false, "Hesabınız askıya alınmış. Lütfen yönetici ile iletişime geçin.");

        if (!_userService.VerifyPassword(password, user.PasswordHash))
            return (false, "Şifre hatalı.");

        _currentUser = user;
        await SaveUserToStorageAsync();
        OnAuthStateChanged?.Invoke();
        return (true, "Giriş başarılı.");
    }

    public async Task<(bool Success, string Message)> RegisterAsync(string email, string password, string fullName, string phone, UserRole role = UserRole.Buyer)
    {
        var existingUser = await _userService.GetUserByEmailAsync(email);
        if (existingUser != null)
            return (false, "Bu e-posta adresi zaten kayıtlı.");

        var user = new User
        {
            Email = email,
            PasswordHash = password, // Will be hashed in UserService
            FullName = fullName,
            Phone = phone,
            Role = role
        };

        await _userService.CreateUserAsync(user);
        _currentUser = user;
        await SaveUserToStorageAsync();
        OnAuthStateChanged?.Invoke();
        return (true, "Kayıt başarılı.");
    }

    public async Task LogoutAsync()
    {
        _currentUser = null;
        await ClearStorageAsync();
        OnAuthStateChanged?.Invoke();
    }

    // Legacy sync method for compatibility
    public void Logout()
    {
        _currentUser = null;
        if (_jsRuntime != null)
        {
            _ = ClearStorageAsync();
        }
        OnAuthStateChanged?.Invoke();
    }

    public void SetCurrentUser(User user)
    {
        _currentUser = user;
        if (_jsRuntime != null)
        {
            _ = SaveUserToStorageAsync();
        }
        OnAuthStateChanged?.Invoke();
    }

    public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
    {
        if (_currentUser == null) return false;

        if (!_userService.VerifyPassword(currentPassword, _currentUser.PasswordHash))
            return false;

        await _userService.ChangePasswordAsync(_currentUser.Id!, newPassword);
        return true;
    }

    public async Task<bool> UpdateProfileAsync(string fullName, string phone, string address)
    {
        if (_currentUser == null) return false;

        _currentUser.FullName = fullName;
        _currentUser.Phone = phone;
        _currentUser.Address = address;

        var result = await _userService.UpdateUserAsync(_currentUser);
        if (result)
        {
            await SaveUserToStorageAsync();
        }
        return result;
    }

    // Simple class to store minimal user info
    private class StoredUser
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Role { get; set; }
    }
}
