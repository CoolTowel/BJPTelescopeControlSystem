# BJP望远镜控制系统 - 前端文件结构

## 📁 文件夹结构

```
wwwroot/
├── index.html              # 主页
├── login.html              # 登录页面
├── dashboard.html          # 用户控制台
├── admin-dashboard.html    # 管理员控制台
├── submit-task.html        # 观测任务提交页面
├── register.html           # 用户注册页面
├── user-profile.html       # 用户资料页面
├── README.md              # 本说明文件
├── README_AUTH.md         # 认证系统说明文档
├── js/                    # JavaScript文件
│   ├── app.js            # 主应用程序逻辑
│   ├── auth.js           # 认证管理模块
│   ├── dashboard.js      # 用户仪表板逻辑
│   ├── routes.js         # 路由配置
│   ├── submit-task.js    # 任务提交功能
│   └── table.js          # 动态表格组件
└── css/                   # CSS样式文件
    └── table.css         # 表格样式
```

## 🔗 路径引用说明

### JavaScript文件引用
- `src="js/app.js"` - 主应用程序逻辑
- `src="js/auth.js"` - 认证管理模块
- `src="js/routes.js"` - 路由配置
- `src="js/dashboard.js"` - 用户仪表板逻辑
- `src="js/submit-task.js"` - 任务提交功能
- `src="js/table.js"` - 动态表格组件

### CSS文件引用
- `href="css/table.css"` - 表格样式

### 页面跳转路径
- `/index.html` - 主页
- `/login.html` - 登录页面
- `/dashboard.html` - 用户控制台
- `/admin-dashboard.html` - 管理员控制台
- `/submit-task.html` - 观测任务提交页面
- `/register.html` - 用户注册页面
- `/user-profile.html` - 用户资料页面

## 🚀 访问方式

- **主页**: `http://localhost:xxxx/index.html`
- **登录页**: `http://localhost:xxxx/login.html`
- **用户控制台**: `http://localhost:xxxx/dashboard.html`
- **管理员控制台**: `http://localhost:xxxx/admin-dashboard.html`

## 📝 文件组织规范

这种组织方式遵循了常见的前端项目规范：

### ✅ 优点
1. **HTML文件在根目录** - 便于直接访问，URL简洁
2. **静态资源分类** - JS和CSS文件分别放在独立文件夹
3. **路径清晰** - 相对路径简单易懂
4. **便于维护** - 文件类型分类明确

### 🔄 其他常见规范

#### 方案1：完全分离（适合大型项目）
```
wwwroot/
├── pages/          # HTML页面
├── assets/         # 静态资源
│   ├── js/
│   ├── css/
│   └── images/
└── components/     # 可复用组件
```

#### 方案2：按功能模块（适合复杂应用）
```
wwwroot/
├── auth/           # 认证相关
│   ├── login.html
│   └── register.html
├── dashboard/      # 控制台相关
│   ├── user.html
│   └── admin.html
├── tasks/          # 任务相关
│   └── submit.html
├── js/
└── css/
```

#### 方案3：当前方案（推荐用于中小型项目）
```
wwwroot/
├── *.html          # 所有HTML页面
├── js/             # JavaScript文件
├── css/            # CSS样式文件
└── images/         # 图片资源（如有）
```

## 🎯 推荐

对于您的项目，当前的方案是最合适的，因为：
- 项目规模适中
- 页面数量不多
- 便于快速开发和维护
- 符合大多数Web开发者的习惯 