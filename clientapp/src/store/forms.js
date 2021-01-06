import axios from '../utils/api';
const initState = () => ({
    message: 'hello forms vuex module',
    forms: [],
    page: 1,
    total: 1,
    limit: 5,
    navigation: {}
});

export default {
    namespaced: true,
    state: initState,
    actions: {
        fetchForms: async ({commit}, {page, input, limit, orderBy, orderDirection}) => {
            let response = await axios.get(`forms?page=${page !== undefined || null ? page : 0}`+
                                       `${input !== undefined || null || '' ? '&searchInput='+input : ''}`+
                                       `${limit !== undefined || null || '' ? '&limit='+limit : ''}`+
                                       `${orderBy !== undefined || null ? '&orderBy='+orderBy : ''}`+
                                       `${orderDirection !== undefined || null ? '&orderDirection='+orderDirection : ''}`);
            commit('setForms', {forms: response.data.items});
            commit('setNavigation', {navigation: response.data.navigation});
            commit('setPages', {page: response.data.page, total: response.data.totalPages});
        },
        navigate: async ({commit}, {page}) => {
            let response = await axios.get('forms'+ page);
            commit('setForms', {forms: response.data.items});
            commit('setNavigation', {navigation: response.data.navigation});
            commit('setPages', {page: response.data.page, total: response.data.totalPages});
        },
        delete: async (inject, id) => {
            await axios.delete('forms/'+id);
        },
        submitForm: async (inject, {form}) => {
            await axios.post('forms', {
                name: form.name,
                email: form.email,
                telephone: form.telephone,
                company: form.company,
                appointment: form.appointment,
            });
        }
    },
    mutations: {
        setForms: (state, {forms}) => {
            state.forms = forms;
        },
        setNavigation: (state, {navigation}) => {
            state.navigation = navigation;
        },
        setPages: (state, {page, total}) => {
            state.page = page;
            state.total = total;
        }
    },
    getters: {
        forms: (state) => {
            return state.forms;
        },
        navigation: (state) => {
            return {
                pages: {
                    page: state.page,
                    total: state.total,
                },
                nav: state.navigation
            }
        }
    }
}
