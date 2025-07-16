# 认证系统架构说明

## 概述

新的认证系统采用了模块化的设计，提供了更好的用户权限管理和页面访问控制。

## 核心组件

### 1. `auth.js` - 认证管理器
- **AuthManager 类**：统一的认证管理类
- **功能**：
  - 检查登录状态
  - 根据用户角色重定向
  - 页面访问权限验证
  - 登录/登出处理

### 2. `routes.js` - 路由配置
- **路由定义**：所有页面路径的常量定义
- **权限配置**：每个页面的访问权限要求
- **权限检查函数**：统一的权限验证逻辑

### 3. 页面集成
所有页面都包含以下脚本引用：
```html
<script src="routes.js"></script>
<script src="auth.js"></script>
```

## 权限系统

### 页面权限级别
1. **公共页面**：无需登录（如登录页）
2. **用户页面**：需要登录，普通用户可访问
3. **管理员页面**：需要登录且需要管理员权限

### 自动重定向逻辑
- 未登录用户访问受保护页面 → 重定向到登录页
- 普通用户访问管理员页面 → 重定向到用户仪表板
- 已登录用户访问登录页 → 根据角色重定向到相应仪表板

## 使用方法

### 在页面中使用
```javascript
// 检查页面访问权限
const hasAccess = await authManager.checkPageAccess();
if (!hasAccess) return;

// 获取当前用户信息
const user = authManager.getCurrentUser();

// 检查是否为管理员
const isAdmin = authManager.isUserAdmin();

// 登出
await authManager.logout();
```

### 添加新页面
1. 在 `routes.js` 中添加路由定义
2. 在 `PAGE_PERMISSIONS` 中配置权限要求
3. 在页面中引用必要的脚本
4. 使用 `authManager.checkPageAccess()` 验证权限

## 优势

1. **统一管理**：所有认证逻辑集中在一个模块中
2. **易于维护**：路由和权限配置集中管理
3. **安全性**：自动的权限检查和重定向
4. **可扩展性**：易于添加新的页面和权限级别
5. **代码复用**：避免重复的认证逻辑

## 文件结构

```
wwwroot/
├── auth.js          # 认证管理器
├── routes.js        # 路由配置
├── app.js           # 应用逻辑
├── dashboard.js     # 仪表板逻辑
├── index.html       # 主页
├── login.html       # 登录页
├── dashboard.html   # 用户仪表板
├── admin-dashboard.html # 管理员仪表板
└── README_AUTH.md   # 本文档
``` 