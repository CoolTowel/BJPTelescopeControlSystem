import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:5000/api', // 修改为你的 ASP.NET Core 地址
  withCredentials: true, // 如果用 Cookie 登录
});

export default api;
