import {createRouter, createWebHistory} from 'vue-router'
import Home from '../views/Home.vue'
import Admin from '../layouts/admin';
import Dashboard from "../views/admin/Dashboard";
import Users from "../views/admin/Users";
import Forms from "../views/admin/Forms";
import Form from "../views/Form";
import NotFound from "../views/NotFound";
import store from "../store/index";
import Smtp from "@/views/admin/Smtp";

const routes = [
    {path: '/', name: 'Home', component: Home},
    {path: '/form', name: 'Form', component: Form, meta: { authorize: 'user' }},

    {path: '/admin', name: 'Admin',component: Admin, meta: { authorize: 'admin' },
        children: [
            {path: '', name: 'Dashboard', component: Dashboard},
            {path: 'users', name: 'Users', component: Users},
            {path: 'forms', name: 'Forms', component: Forms},
            {path: 'smtp', name: 'SMTP', component: Smtp},
            {path: '/*', component: NotFound},
        ]
    },

    {path: '/*', component: NotFound},
];

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    mode: 'history',
    routes
});


router.beforeEach((to, from, next) => {
    const { authorize } = to.meta;
    const token = store.state.token;
    const role = store.state.role;

    if (authorize) {
        if (!token) {
            return next({ path: '/', query: { returnUrl: to.path } });
        }
        if (authorize !== role) {
            return next({ path: '/' });
        }
    }
    next();
});

export default router
