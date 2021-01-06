import axios from 'axios';
import store from '../store/index';
let baseUrl = 'https://localhost:5001/api/';

let api =  axios.create({
   baseURL: baseUrl
});

api.interceptors.request.use(function (config) {
   let token = store.state.token;
   if (token){
      config.headers.Authorization = `Bearer ${token}`;
   }
   return config;
});

api.interceptors.response.use(response => response, async error => {
   const status = error.response.status;
   if (status === 401) {
      if (store.state.refreshToken) {
         let value = await api.post('account/refresh', {
            token: store.state.token,
            refreshToken: store.state.refreshToken
         });
         store.commit('setToken', {token: value.data.token});
         store.commit('setRefreshToken', {refreshToken: value.data.refreshToken});
         error.config.headers.Authorization = `Bearer ${value.data.token}`;
         return await axios.request(error.config);
      } else {
         console.log('rejected call');
         return Promise.reject(error);
      }
   }
});

export default api;
