import {createRouter, createWebHistory} from 'vue-router'
import Home from '../views/Home.vue'
import Admin from '../layouts/admin';
import Dashboard from "../views/admin/Dashboard";
import Users from "../views/admin/Users";
import Forms from "../views/admin/Forms";
import Form from "../views/Form";
import NotFound from "../views/NotFound";

const routes = [
    {path: '/', name: 'Home', component: Home},
    {path: '/form', name: 'Form', component: Form},

    {path: '/admin', name: 'Admin',component: Admin,
        children: [
            {path: '', name: 'Dashboard', component: Dashboard},
            {path: 'users', name: 'Users', component: Users},
            {path: 'forms', name: 'Forms', component: Forms},
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

export default router
