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

                <p  @click="deleteForm(form.id)"> <svg id="Layer_1" enable-background="new 0 0 512 512" height="24" viewBox="0 0 512 512" width="24" xmlns="http://www.w3.org/2000/svg"><g><path d="m424 64h-88v-16c0-26.467-21.533-48-48-48h-64c-26.467 0-48 21.533-48 48v16h-88c-22.056 0-40 17.944-40 40v56c0 8.836 7.164 16 16 16h8.744l13.823 290.283c1.221 25.636 22.281 45.717 47.945 45.717h242.976c25.665 0 46.725-20.081 47.945-45.717l13.823-290.283h8.744c8.836 0 16-7.164 16-16v-56c0-22.056-17.944-40-40-40zm-216-16c0-8.822 7.178-16 16-16h64c8.822 0 16 7.178 16 16v16h-96zm-128 56c0-4.411 3.589-8 8-8h336c4.411 0 8 3.589 8 8v40c-4.931 0-331.567 0-352 0zm313.469 360.761c-.407 8.545-7.427 15.239-15.981 15.239h-242.976c-8.555 0-15.575-6.694-15.981-15.239l-13.751-288.761h302.44z"/><path d="m256 448c8.836 0 16-7.164 16-16v-208c0-8.836-7.164-16-16-16s-16 7.164-16 16v208c0 8.836 7.163 16 16 16z"/><path d="m336 448c8.836 0 16-7.164 16-16v-208c0-8.836-7.164-16-16-16s-16 7.164-16 16v208c0 8.836 7.163 16 16 16z"/><path d="m176 448c8.836 0 16-7.164 16-16v-208c0-8.836-7.164-16-16-16s-16 7.164-16 16v208c0 8.836 7.163 16 16 16z"/></g></svg></p>
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

<style scoped lang="scss">
table{
  border: 3px solid #efefef;
  thead{
    tr{
      background-color: #e6e6e6;

      th{
        padding: 15px 5px;
        font-size: 18px;
        font-weight: 700;
        &:last-child{
          text-align: center;
        }
      }
    }
  }
  tbody{
    tr{
      background: #efefef87;
      border-bottom: 1px solid #e6e6e6;
      td{
        width: 250px;
        padding: 20px 5px;
        font-size: 16px;
        font-weight: 500;

        p{
          margin: 0;
          cursor: pointer;
          display: flex;
          justify-content: center;
          svg{
            transition: all 0.5s;
          }
          &:hover{
            svg{
              fill: red;
            }
          }
        }
      }
    }
  }
}
</style>
