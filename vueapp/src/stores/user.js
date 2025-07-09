import { defineStore } from 'pinia';

export const useUserStore = defineStore('user', {
  state: () => ({
    token: null,
    username: null,
  }),
  actions: {
    setUser({ token, username }) {
      this.token = token;
      this.username = username;
    },
    logout() {
      this.token = null;
      this.username = null;
    },
  },
});
