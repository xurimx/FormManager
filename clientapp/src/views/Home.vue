<template>
    <div class="home">
        Login Page
    </div>
</template>

<script>
    import {mapState, mapActions, mapGetters} from 'vuex';

    export default {
        name: 'Home',
        computed: {...mapState('users', ['message']), ...mapGetters(['token'])},
        components: {},
        methods: {
            ...mapActions(['authenticate'])
        },
        async created() {
            await this.authenticate({username: 'admin', password: 'P@ssw0rd'});
            console.log(this.token);
        }
    }
</script>
