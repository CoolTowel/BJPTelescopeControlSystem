<template>
  <div>
    <h2>欢迎，{{ userStore.username }}</h2>
    <button @click="logout">退出登录</button>
  </div>
</template>

<script setup>
import { useUserStore } from '../stores/user';
import { useRouter } from 'vue-router';

const userStore = useUserStore();
const router = useRouter();

const logout = () => {
  userStore.logout();
  router.push('/login');
};
</script>
