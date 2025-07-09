<template>
  <div class="login">
    <h2>登录</h2>
    <form @submit.prevent="submit">
      <input v-model="username" placeholder="用户名" required />
      <input v-model="password" type="password" placeholder="密码" required />
      <button type="submit">登录</button>
      <p v-if="error">{{ error }}</p>
    </form>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import api from '../api';
import { useUserStore } from '../stores/user';

const username = ref('');
const password = ref('');
const error = ref('');
const router = useRouter();
const userStore = useUserStore();

const submit = async () => {
  try {
    const res = await api.post('/auth/login', { username: username.value, password: password.value });
    userStore.setUser({ token: res.data.token, username: username.value });
    await router.push('/dashboard');
  } catch (err) {
    console.log(err.response);  // 👈 打印错误
    error.value = err.response?.data?.message || '登录失败';
  }
};
</script>
