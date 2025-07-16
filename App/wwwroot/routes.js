// 路由配置
const ROUTES = {
    // 公共页面
    LOGIN: '/login.html',
    
    // 用户页面
    HOME: '/index.html',
    DASHBOARD: '/dashboard.html',
    SUBMIT_TASK: '/submit-task.html',
    USER_PROFILE: '/user-profile.html',
    
    // 管理员页面
    ADMIN_DASHBOARD: '/admin-dashboard.html',
    
    // 默认页面
    DEFAULT: '/index.html'
};

// 页面权限配置
const PAGE_PERMISSIONS = {
    [ROUTES.LOGIN]: { requiresAuth: false, requiresAdmin: false },
    [ROUTES.HOME]: { requiresAuth: true, requiresAdmin: false },
    [ROUTES.DASHBOARD]: { requiresAuth: true, requiresAdmin: false },
    [ROUTES.SUBMIT_TASK]: { requiresAuth: true, requiresAdmin: false },
    [ROUTES.USER_PROFILE]: { requiresAuth: true, requiresAdmin: false },
    [ROUTES.ADMIN_DASHBOARD]: { requiresAuth: true, requiresAdmin: true }
};

// 根据用户角色获取默认页面
function getDefaultPageForUser(isAdmin) {
    return ROUTES.DEFAULT;
}

// 检查页面访问权限
function checkPagePermission(pagePath, isAuthenticated, isAdmin) {
    const permissions = PAGE_PERMISSIONS[pagePath];
    
    if (!permissions) {
        // 未知页面，重定向到默认页面
        return { allowed: false, redirect: ROUTES.DEFAULT };
    }
    
    if (permissions.requiresAuth && !isAuthenticated) {
        return { allowed: false, redirect: ROUTES.LOGIN };
    }
    
    if (permissions.requiresAdmin && !isAdmin) {
        return { allowed: false, redirect: ROUTES.DASHBOARD };
    }
    
    return { allowed: true };
} 