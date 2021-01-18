<template>
    <h1>Fill form</h1>
    <button type="button" @click="logout">Logout</button>


    <input type="text" placeholder="Name" v-model="name"/>
    <input type="email" placeholder="Email" v-model="email"/>
    <input type="tel" placeholder="Tel" v-model="tel"/>
    <input type="text" placeholder="Company" v-model="company"/>
    <input type="datetime-local" placeholder="Appointment" v-model="appointment"/>


    <input type="submit" @click="dataPost">
    <span v-if="error">{{ error }}</span>

</template>

<script>

import axios from '../utils/api';
import {mapGetters, mapMutations} from 'vuex';

const initData = () => ({
    name: '',
    email: '',
    tel: '',
    company: '',
    appointment: '',
    error: ''
});
export default {
    name: "Form",
    data: initData,
    methods: {
        dataPost: function() {
            axios.post('Forms', {
                name: this.name,
                email: this.email,
                telephone: this.tel,
                company: this.company,
                appointment: this.appointment,
            }).then(() => {
                this.resetForm();
            }).catch(reason => {
                this.error = reason.response.data.Description;
            });
        },
        ...mapMutations(['resetState']),
        logout: function () {
            this.resetState();
            this.$router.push('/');
        },
        resetForm: function () {
            Object.assign(this.$data, initData());
        }
    },
    computed: {...mapGetters(['token'])},
}
</script>

<style scoped>

</style>
