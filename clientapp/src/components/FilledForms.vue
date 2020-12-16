<template>
    <table>
        <thead>
        <tr>
            <th>Name</th>
            <th>Email</th>
            <th>Telephone</th>
            <th>Company</th>
            <th>Appointment</th>
            <th>Delete Form</th>
        </tr>
        </thead>
        <tbody>
        <tr v-for="form in forms" :key="form.id">
            <td>{{ form.name}}</td>
            <td>{{ form.email}}</td>
            <td>{{ form.telephone}}</td>
            <td>{{ form.company}}</td>
            <td>{{ form.appointment}}</td>
            <td><button type="button" @click="deleteForm(form.id)">Delete</button> </td>
        </tr>
        </tbody>
    </table>

</template>

<script>
    import axios from '../utils/api'
    import {mapGetters} from 'vuex'

    export default {
        name: "FilledForms",
        data() {
            return {
                name: '',
                email: '',
                telephone: '',
                company: '',
                appointment: '',
                forms: [],
            }
        },
        methods: {
            getForms: async function () {
                let response = await axios.get('Forms',{
                    headers: {
                        'Authorization': 'Bearer ' + this.token,
                    }
                } );
                this.forms = response.data.items;
            },
            deleteForm: async function(id) {
                await axios.delete('Forms/' +id, {
                    headers: {
                        'Authorization': 'Bearer ' + this.token,
                    }
                });
                await  this.getForms();
            }
        },
        async mounted() {
            await this.getForms();
        },
        computed: {...mapGetters(['token'])},
    }
</script>

<style scoped>


</style>