// 使用统一的认证管理器
// 认证逻辑已移至 auth.js

// 登录函数
async function login(event) {
    event.preventDefault();
    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;

    const result = await authManager.login(username, password);
    
    if (!result.success) {
        document.getElementById("error").innerText = result.error;
    }
}

// 登出函数
async function logout() {
    await authManager.logout();
}

// 页面特定的初始化逻辑
document.addEventListener('DOMContentLoaded', async () => {
    // 检查页面访问权限
    const hasAccess = await authManager.checkPageAccess();
    if (!hasAccess) return;

    // 如果是主页，显示用户信息
    if (window.location.pathname === '/index.html' || window.location.pathname === '/') {
        const user = authManager.getCurrentUser();
        const app = document.getElementById('app');
        if (app) {
            app.innerHTML = `
            <h2>欢迎，${user}</h2>
            <ul>
                <li><button id="dashboard-btn">控制台</button></li>
                <br>
                <li><a href="/submit-task.html">提交观测任务</a></li>
                <br>
                <li><button onclick="logout()">登出</button></li>
            </ul>
            `;
            // 动态绑定跳转
            document.getElementById('dashboard-btn').onclick = function() {
                if (authManager.isUserAdmin()) {
                    window.location.href = '/admin-dashboard.html';
                } else {
                    window.location.href = '/dashboard.html';
                }
            };
        }
    }
});

