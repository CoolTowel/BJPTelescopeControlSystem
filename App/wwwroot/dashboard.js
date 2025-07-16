// 使用统一的认证管理器
// 认证检查已在 auth.js 中处理，这里只需要页面特定的逻辑

// 页面加载时的初始化
document.addEventListener('DOMContentLoaded', async () => {
    // 检查页面访问权限
    const hasAccess = await authManager.checkPageAccess();
    if (!hasAccess) return;

    // 显示用户信息
    const user = authManager.getCurrentUser();
    console.log(`欢迎，${user}`);
    
    // 这里可以添加仪表板特定的初始化逻辑
});
