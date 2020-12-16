<template>
    <h1>Fill form</h1>


    <input type="text" placeholder="Name" v-model="name"/>
    <input type="email" placeholder="Email" v-model="email"/>
    <input type="tel" placeholder="Tel" v-model="tel"/>
    <input type="text" placeholder="Company" v-model="company"/>
    <input type="datetime-local" placeholder="Appointment" v-model="appointment"/>


    <input type="submit" @click="dataPost">
    <span v-if="error">{{error}}</span>

</template>

<script>

    import axios from '../utils/api';
    import {mapGetters} from 'vuex';

    export default {
        name: "Form",
        data() {
            return {
                name: '',
                email: '',
                tel: '',
                company: '',
                appointment: '',
                error: ''
            }
        },
        methods: {
            dataPost: async function () {
                try {
                    await axios.post('Forms', {
                        name: this.name,
                        email: this.email,
                        telephone: this.tel,
                        company: this.company,
                        appointment: this.appointment,
                    });
                    this.name = '';
                    this.email = '';
                    this.tel = '';
                    this.company = '';
                    this.appointment = '';
                }catch (e) {
                    this.error = e.response.data.Description;
                }
            }
        },
        computed: {...mapGetters(['token'])},
    }
</script>

<style scoped>

</style>
