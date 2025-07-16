// 认证管理模块
class AuthManager {
    constructor() {
        this.currentUser = null;
        this.isAdmin = false;
    }

    // 检查登录状态
    async checkLoginStatus() {
        try {
            const response = await fetch('/api/auth/check-login', { 
                credentials: 'include' 
            });
            
            if (response.ok) {
                const data = await response.json();
                this.currentUser = data.user;
                this.isAdmin = data.isAdmin;
                return { isLoggedIn: true, user: data.user, isAdmin: data.isAdmin };
            } else {
                return { isLoggedIn: false, user: null, isAdmin: false };
            }
        } catch (error) {
            console.error('检查登录状态失败:', error);
            return { isLoggedIn: false, user: null, isAdmin: false };
        }
    }

    // 根据用户角色重定向
    async redirectBasedOnRole() {
        const authStatus = await this.checkLoginStatus();
        const currentPath = window.location.pathname;
        
        // 检查页面权限
        const permission = checkPagePermission(currentPath, authStatus.isLoggedIn, authStatus.isAdmin);
        
        if (!permission.allowed) {
            window.location.href = permission.redirect;
            return;
        }
        
        // 如果已登录且在登录页，重定向到默认页面
        if (authStatus.isLoggedIn && currentPath === ROUTES.LOGIN) {
            const defaultPage = getDefaultPageForUser(authStatus.isAdmin);
            window.location.href = defaultPage;
            return;
        }
    }

    // 检查页面访问权限
    async checkPageAccess() {
        const authStatus = await this.checkLoginStatus();
        const currentPath = window.location.pathname;
        
        const permission = checkPagePermission(currentPath, authStatus.isLoggedIn, authStatus.isAdmin);
        
        if (!permission.allowed) {
            window.location.href = permission.redirect;
            return false;
        }
        
        return true;
    }

    // 获取当前用户信息
    getCurrentUser() {
        return this.currentUser;
    }

    // 检查是否为管理员
    isUserAdmin() {
        return this.isAdmin;
    }

    // 登录
    async login(username, password) {
        try {
            const response = await fetch("/api/auth/login", {
                method: "POST",
                credentials: "include",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ username, password }),
            });

            if (response.ok) {
                const data = await response.json();
                console.log("登录成功", data);
                
                // 登录成功后重新检查状态并重定向
                await this.redirectBasedOnRole();
                return { success: true };
            } else {
                return { success: false, error: "登录失败，请重试" };
            }
        } catch (error) {
            console.error("登录失败:", error);
            return { success: false, error: "网络错误，请重试" };
        }
    }

    // 登出
    async logout() {
        try {
            const response = await fetch('/api/auth/logout', {
                method: 'POST',
                credentials: 'include',
            });
            
            if (response.ok) {
                this.currentUser = null;
                this.isAdmin = false;
                window.location.href = ROUTES.LOGIN;
            }
        } catch (error) {
            console.error("登出失败:", error);
        }
    }
}

// 创建全局认证管理器实例
const authManager = new AuthManager();

// 页面加载时自动检查认证状态
document.addEventListener('DOMContentLoaded', async () => {
    await authManager.redirectBasedOnRole();
}); 