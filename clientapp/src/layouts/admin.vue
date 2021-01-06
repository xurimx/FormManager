<template>
    admin layout
    <ul>
        <li><router-link active-class="active" to="/admin/forms">Forms</router-link></li>
        <li><router-link active-class="active" to="/admin/users">Users</router-link></li>
        <button type="button" @click="logout">Logout</button>
    </ul>
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
