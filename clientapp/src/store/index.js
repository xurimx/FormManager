import {createStore} from 'vuex'
import users from './users.js';
import forms from './forms.js';
import axios from '../utils/api';
// import router from '../router/index'

const initState = () => ({
    message: 'hello vuex',
    token: null,
    refreshToken: null,
    role: '',
    active: false,
    loading: false,
    ready: false,
    user: null
});

const store = createStore({
    modules: {users, forms},
    state: initState,
    actions: {
        authenticate: async ({commit}, {username, password}) => {
            let response = await axios.post('account/authenticate', {username, password});
            commit('setToken', {token: response.data.token});
            commit('setRefreshToken', {refreshToken: response.data.refreshToken});
        },
        userinfo: async ({commit, state}) => {
            try {
                if (state.token) {
                    let response = await axios.get('account/userinfo', {
                        headers: {
                            'Authorization': 'Bearer ' + state.token,
                        },
                    });
                    let role = response.data.roles[0];
                    commit('setRole', {role: role});
                    commit('setUser', {user: response.data})
                }
            } finally {
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
        },
        loading: state => {
            return state.loading;
        },
        user: state => {
            return state.user;
        }
    },
    mutations: {
        setToken: (state, {token}) => {
            state.token = token;
        },
        setRefreshToken: (state, {refreshToken}) => {
            state.refreshToken = refreshToken
        },
        setRole: (state, {role}) => {
            state.role = role;
        },
        setReady: (state, payload) => {
            state.ready = payload;
        },
        setLoading: (state, payload) => {
            state.loading = payload;
        },
        initialiseStore: (state) => {
            let token = localStorage.getItem('token');
            let refresh = localStorage.getItem('refreshToken');
            if (token != null) {
                state.token = token;
            }
            if (refresh != null) {
                state.refreshToken = refresh;
            }
        },
        resetState: state => {
            state.token = null;
            state.refreshToken = null;
        },
        setUser: (state, {user}) =>{
            state.user = user;
        }
    }
});

store.subscribe((mutation, state) => {
    localStorage.setItem('token', state.token);
    localStorage.setItem('refreshToken', state.refreshToken);
});

export default store;
