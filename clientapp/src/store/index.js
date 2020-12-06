import { createStore } from 'vuex'
import users from './users.js';
import api from '../utils/api';

const initState = () => ({
    message: 'hello vuex',
    token: '',
    role: ''
});

const store = createStore({
    modules: {users},
    state: initState,
    actions: {
        authenticate: async ({commit}, {username, password}) => {
            let response = await api.post('account/authenticate', {username, password});
            commit('setToken', {token:response.data.token});
        }
    },
    getters: {
      token: state => {
          return state.token;
      }
    },
    mutations: {
        setToken: (state, {token}) => {
            state.token = token;
        }
    }
});

export default store;
