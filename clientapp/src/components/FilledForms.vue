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
            <td>
                <button type="button" @click="deleteForm(form.id)">Delete</button>
            </td>
        </tr>
        </tbody>
    </table>

    <input type="search" placeholder="Search" v-model="search">


    <select name="length" id="length" v-model="length">
        <option value="choose-length" disabled selected>Choose length</option>
        <option value="5">5</option>
        <option value="15">15</option>
        <option value="25">25</option>
    </select>

    <button type="button" @click="previousPage">Prev</button>
    <button type="button" @click="nextPage">Next</button>


</template>

<script>
    import axios from '../utils/api'
    import {mapGetters, mapMutations} from 'vuex'

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
                page: 1,
                length: 5,
                search: '',
            }
        },
        methods: {
            ...mapMutations(['setReady']),
            getForms: async function () {
                this.setReady(false);
                let response = await axios.get(`Forms?limit=${this.length}&page=${this.page}&SearchInput=${this.search}`,);
                this.forms = response.data.items;
                this.setReady(true);
            },
            deleteForm: async function (id) {
                await axios.delete('Forms/' + id, {
                    headers: {
                        'Authorization': 'Bearer ' + this.token,
                    }
                });
                await this.getForms();
            },

            nextPage: function () {
                this.page++;
                this.getForms();
            },
            previousPage: function () {
                if (this.page !== 1) {
                    this.page--;
                    this.getForms();
                }
            },
        },
        watch: {
            search: function (newSeach) {
                if (newSeach.length >= 3) {
                    this.getForms();
                }
                if (newSeach.length === 0) {
                    this.getForms();
                }
            },
            length: function () {
                this.getForms();
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