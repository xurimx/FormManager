import axios from 'axios';
import store from '../store/index';
let baseUrl = 'https://formmanager.azurewebsites.net/api/';

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

api.interceptors.response.use(response => response, error => {
   const status = error.response.status;
   if (status === 401) {
      console.log('unauthorized');
      if (store.state.refreshToken){
         axios.post(baseUrl+'account/refresh', {
            token: store.state.token,
            refreshToken: store.state.refreshToken
         }).then(value => {
            store.commit('setToken', {token: value.data.token});
            store.commit('setRefreshToken', {refreshToken: value.data.refreshToken});
            error.config.headers.Authorization = `Bearer ${value.data.token}`;
            return axios.request(error.config);
         });
      }
   }
   return Promise.reject(error);
});

export default api;
