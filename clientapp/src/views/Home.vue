<template>
    <div class="home">
        <input type="text" v-model="username">
        <input type="password" v-model="password">
        <input type="submit" value="Submit" @click="login">
    </div>
</template>

<script>
    import {mapActions, mapGetters} from 'vuex';
    //import axios from '../utils/api';

    export default {
        name: 'Home',
        data() {
            return {
                username: '',
                password: '',
            }
        },
        computed: {...mapGetters(['token', 'role'])},
        components: {},
        methods: {
            ...mapActions(['authenticate', 'userinfo']),
            login: async function () {
                await this.authenticate({
                    'username': this.username,
                    'password': this.password
                });

                await this.userinfo();

                if (this.role === 'admin') {
                    this.$router.push('Admin');
                } else {
                    this.$router.push('Form');
                }
            }
        },
    }
</script>
