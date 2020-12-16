<template>
    <div class="home">
        <input type="text" v-model="username">
        <input type="password" v-model="password">
        <input type="submit" value="Submit" @click="login">
        <span v-if="error">{{error}}</span>
    </div>

</template>

<script>
    import {mapActions, mapGetters} from 'vuex';

    export default {
        name: 'Home',
        data() {
            return {
                username: '',
                password: '',
                error: null
            }
        },
        computed: {...mapGetters(['token', 'role'])},
        components: {},
        methods: {
            ...mapActions(['authenticate', 'userinfo', 'resetState']),
            login: async function () {
                try {
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

                }catch (e) {
                    this.password = '';
                    this.error = e.response.data.Description;
                }
            },
            logout: function(){
                this.resetState();
                this.$router.push('/');
            }
        },
    }
</script>
