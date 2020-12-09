import {createStore} from 'vuex'
import users from './users.js';
import axios from '../utils/api';
// import router from '../router/index'

const initState = () => ({
    message: 'hello vuex',
    token: null,
    role: '',
    active: false,
    ready: false,
});

const store = createStore({
    modules: {users},
    state: initState,
    actions: {
        authenticate: async ({commit}, {username, password}) => {
            let response = await axios.post('account/authenticate', {username, password});
            commit('setToken', {token: response.data.token});
        },
        userinfo: async ({commit, state}) => {
            try {
                let response = await axios.get('account/userinfo', {
                    headers: {
                        'Authorization': 'Bearer ' + state.token,
                    },
                });
                let role = response.data.roles[0];
                commit('setRole', {role: role});
                commit('setReady', true);
            } catch (e) {
                commit('setReady', true);
            }
        },
    },
    getters: {
        token: state => {
            return state.token;
        },
        role: state => {
            return state.role;
        },
        active: state => {
            return state.active;
        },
        ready: state => {
            return state.ready;
        }
    },
    mutations: {
        setToken: (state, {token}) => {
            state.token = token;
        },
        setRole: (state, {role}) => {
            state.role = role;
        },
        setReady: (state, payload) => {
            state.ready = payload;
        },
        initialiseStore: (state) => {
            let token = localStorage.getItem('token');
            if (token != null) {
                state.token = token;
            }
        }
    }
});

store.subscribe((mutation, state) => {
    localStorage.setItem('token', state.token);
});
export default store;
