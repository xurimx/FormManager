import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import axios from './utils/api';

import 'bootstrap/dist/css/bootstrap.min.css'
import 'bootstrap/dist/js/bootstrap.bundle.min'

const app = createApp(App).use(store).use(router);
app.config.globalProperties.$axios = () => axios;

app.mount('#app');
