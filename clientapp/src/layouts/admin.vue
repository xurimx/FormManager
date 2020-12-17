<template>
    admin layout
    <button type="button" @click="logout">Logout</button>
    <router-view></router-view>
</template>

<script>
    import {mapMutations} from "vuex";

    export default {
        name: "admin",

        methods: {
            ...mapMutations(['resetState']),

            logout: function () {
              this.resetState();
              this.$router.push('/');
            },

        },
    }
</script>

<style scoped>

</style>
