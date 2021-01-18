<template>
    <div :class="{'loading': !this.ready}">
        <router-view/>
    </div>
    <loader v-if="this.loading"></loader>
</template>


<script>

    import {mapActions, mapGetters} from 'vuex';
    import Loader from "./components/Loader";

    export default {
        components: {Loader},
        beforeCreate() {
            this.$store.commit('initialiseStore');
        },

        async created() {
          let initPath = window.location.pathname;
          try {
            await this.$store.dispatch('userinfo');
            await this.$router.push({path: initPath});
          }catch (e) {
            await this.$router.push({name:'Home'});
          }
        },

        computed: {...mapGetters(['role', 'ready', 'loading'])},
        methods: {
            ...mapActions(['userinfo']),
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
