<template>
    <h1>Admin Dashboard</h1>
    <CreateAccount></CreateAccount>

    <FilledForms></FilledForms>
</template>

<script>
    import CreateAccount from "../../components/CreateAccount";
    import FilledForms from "../../components/FilledForms";
    export default {
        name: "Dashboard",
        components: {FilledForms, CreateAccount}
    }
</script>

<style scoped>

</style>
