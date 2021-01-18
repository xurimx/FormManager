<template>
    <div class="home">
        <input type="text" v-model="username">
        <input type="password" v-model="password">
        <input type="submit" value="Submit" @click="login">
        <span v-if="error">{{error}}</span>
    </div>
</template>

<script>
    import {mapActions, mapGetters, mapMutations} from 'vuex';

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
      methods: {
            ...mapActions(['authenticate', 'userinfo', 'resetState']),
            ...mapMutations(['setLoading']),
            login: async function () {
                try {
                    this.setLoading(true);
                    await this.authenticate({
                        'username': this.username,
                        'password': this.password
                    });

                    await this.userinfo();
                    if (this.role === 'admin') {
                        await this.$router.push('admin');
                    } else {
                        await this.$router.push('form');
                    }
                    this.setLoading(false);

                }catch (e) {
                    this.password = '';
                    this.error = e.response.data.Description;
                    this.setLoading(false);
                }
            },
            logout: function(){
                this.resetState();
                this.$router.push('/');
            }
        },
      async mounted() {
        setTimeout(async () => {
          switch (this.role){
            case 'admin': await this.$router.push({path: '/admin'}); break;
            case 'user': await this.$router.push({path: '/form'}); break;
            default: break;
          }
        }, 100)
      }
    }
</script>
