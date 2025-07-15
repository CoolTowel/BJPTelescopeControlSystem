async function checkLogin() {
    console.log("开始检查登录状态...");
    const res = await fetch('/api/auth/check-login', { credentials: 'include' });
    console.log("返回状态码：", res.status);
    if (res.ok) {
        //check if user is Administrator
        const data = await res.json();
        if (data.isAdmin) {
            window.location.href = '/admin-dashboard.html';
        }
    }
}

checkLogin();
