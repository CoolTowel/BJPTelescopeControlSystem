import { createRouter, createWebHistory } from 'vue-router';
import LoginView from '../views/LoginView.vue';
import DashboardView from '../views/DashboardView.vue';

const routes = [
  { path: '/', redirect: '/login' },
  { path: '/login', component: LoginView },
  { path: '/dashboard', component: DashboardView },
  {
         path: '/about',
         name: 'about',
         // route level code-splitting
         // this generates a separate chunk (About.[hash].js) for this route
         // which is lazy-loaded when the route is visited.
         component: () => import('../views/AboutView.vue'),
       },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;

// 原模板部分
//import { createRouter, createWebHistory } from 'vue-router'
//import HomeView from '../views/HomeView.vue'

//const router = createRouter({
//  history: createWebHistory(import.meta.env.BASE_URL),
//  routes: [
//    {
//      path: '/',
//      name: 'home',
//      component: HomeView,
//    },
//    {
//      path: '/about',
//      name: 'about',
//      // route level code-splitting
//      // this generates a separate chunk (About.[hash].js) for this route
//      // which is lazy-loaded when the route is visited.
//      component: () => import('../views/AboutView.vue'),
//    },
//  ],
//})

//export default router
