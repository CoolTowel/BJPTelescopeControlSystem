async function checkLogin() {
    console.log("开始检查登录状态...");
    const res = await fetch('/api/auth/check-login', { credentials: 'include' });
    console.log("返回状态码：", res.status);
    if (res.ok) {
        if (window.location.pathname === '/login.html') {
            console.log("已登录，跳转到主页");
            window.location.href = '/index.html';
        }
        const data = await res.json();
        console.log("登录用户：", data.user);
        if (window.location.pathname === '/index.html' | window.location.pathname === '/') {
            const app = document.getElementById('app');
            app.innerHTML = `
            <h2>欢迎，${data.user}</h2>
            <ul>
                <li><a href="/dashboard.html">控制台</a></li>
                <br>
                <li><a href="/submit-task.html">提交观测任务</a></li>
                <br>
                <li><button onclick="logout()">登出</button></li>
            </ul>
            `;
        }
    } else {
        if (window.location.pathname !== '/login.html') {
            console.log("未登录，跳转登录页");
            window.location.href = '/login.html';
            }
        }
    }


async function login(event) {
    event.preventDefault();
    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;

    const res = await fetch("/api/auth/login", {
        method: "POST",
        credentials: "include",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, password }),
    });

    if (res.ok) {
        const data = await res.json();
        console.log("登录成功", data);
        window.location.href = "/index.html";
    } else {
        console.error("登录失败，状态码：", res.status);
        document.getElementById("error").innerText = "登录失败，请重试";
    }
}

async function logout() {
    const res = await fetch('/api/auth/logout', {
        method: 'POST',
        credentials: 'include',
    });
    console.log("返回状态码：", res.status);
    window.location.href = '/login.html';
}

document.addEventListener('DOMContentLoaded', () => {
    checkLogin();
});

