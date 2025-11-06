// Minimal cookie helper for Blazor
window.cookieHelper = {
    setCookie: function(name, value, days) {
        const expires = days ? `; expires=${new Date(Date.now() + days * 86400000).toUTCString()}` : '';
        document.cookie = `${name}=${value}${expires}; path=/; SameSite=Lax`;
    },
    
    getCookie: function(name) {
        const nameEQ = name + "=";
        const ca = document.cookie.split(';');
        for (let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) === ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) === 0) return c.substring(nameEQ.length, c.length);
        }
        return null;
    },
    
    deleteCookie: function(name) {
        document.cookie = `${name}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`;
    }
};

