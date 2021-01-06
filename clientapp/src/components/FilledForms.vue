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
        <option value="choose-length" disabled>Choose length</option>
        <option value="5">5</option>
        <option value="15">15</option>
        <option value="25">25</option>
    </select>

    <button type="button" v-bind="{'disabled': navigation.pages.page === 1}"
            @click="navigate({page: navigation.nav.firstPage})">First
    </button>
    <button type="button" v-bind="{'disabled': navigation.nav.previousPage === ''}"
            @click="navigate({page: navigation.nav.previousPage})">Prev
    </button>
    <button type="button" v-bind="{'disabled': navigation.nav.nextPage === ''}"
            @click="navigate({page: navigation.nav.nextPage})">Next
    </button>
    <button type="button" v-bind="{'disabled': navigation.pages.total === navigation.pages.page}"
            @click="navigate({page: navigation.nav.lastPage})">Last
    </button>


</template>

<script>
    import {mapGetters, mapMutations, mapActions} from 'vuex'

    export default {
        name: "FilledForms",
        data() {
            return {
                name: '',
                email: '',
                telephone: '',
                company: '',
                appointment: '',
                page: 1,
                length: 5,
                search: '',
            }
        },
        methods: {
            ...mapMutations(['setReady', 'setLoading']),
            ...mapMutations('forms', ['setPages']),
            ...mapActions('forms', ['fetchForms', 'navigate', 'delete']),
            getForms: async function () {
                this.setLoading(true);
                try {
                    await this.fetchForms({
                        page: this.navigation.pages.page,
                        input: this.search,
                        limit: this.length
                    });
                } finally {
                    this.setLoading(false);
                }
            },
            deleteForm: async function (id) {
                this.setLoading(true);
                try {
                    await this.delete(id);
                    await this.getForms();
                } finally {
                    this.setLoading(false);
                }
            }
        },
        watch: {
            search: function (newSeach) {
                if (newSeach.length >= 3) {
                    this.page = 1;
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
        computed: {
            ...mapGetters('forms', ['forms', 'navigation'])
        },
    }
</script>

<style scoped>


</style>
