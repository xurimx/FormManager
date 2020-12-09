<template>
    <div :class="{'loading': !this.ready}">
        <router-view/>
    </div>
    <toast></toast>
</template>


<script>

    import {mapActions, mapMutations, mapGetters} from 'vuex';
    import Toast from "./components/Toast";

    export default {
        components: {Toast},

        beforeCreate() {
            this.$store.commit('initialiseStore');
        },

        async mounted() {
            await this.$store.dispatch('userinfo');
            if (this.role === 'admin') {
                this.$router.push('admin');
            }
            if(this.role === 'user'){
                this.$router.push('form');
            }
        },

        computed: {...mapGetters(['role', 'ready'])},
        methods: {
            ...mapActions(['userinfo']),
            ...mapMutations(['setToken']),
        }
    }

</script>

<style lang="scss">
    .loading{
        display: none;
    }
    #app {
        position: relative;
    }

    #nav {
        padding: 30px;

        a {
            font-weight: bold;
            color: #2c3e50;

            &.router-link-exact-active {
                color: #42b983;
            }
        }
    }
</style>
