<template>
    <input type="text" placeholder="Username" v-model="username">
    <input type="email" placeholder="Email" v-model="email">
    <input type="password" placeholder="Password" v-model="password">
    <input type="submit" placeholder="Create User" @click="createUser">

</template>

<script>
    import axios from '../utils/api'
    import {mapGetters} from 'vuex'

    export default {
        name: "CreateAccount",
        data() {
            return {
                username: '',
                email: '',
                password: '',
            }
        },
        methods: {
            createUser: async function () {
                await axios.post('Users/createuser', {
                    username: this.username,
                    email: this.email,
                    password: this.password,
                    role: 'user',
                },{
                    headers: {
                        'Authorization': 'Bearer ' + this.token,
                    }
                });
                this.username = '';
                this.email = '';
                this.password = '';
            }
        },
        computed: {...mapGetters(['token'])},
    }
</script>

<style scoped>

</style>