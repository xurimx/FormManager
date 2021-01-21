<template>

  <div class="form-wrapper">
    <form class="form-signin login-form home">
      <img src="../assets/talenko-logo.png" alt="">
      <h1>Please sign in</h1>

      <input type="text" v-model="username" class="form-control" placeholder="Username" autofocus v-on:keyup.enter="login"/>

      <input type="password" v-model="password" class="form-control" placeholder="Password" autofocus v-on:keyup.enter="login"/>

      <input type="button" class="submit-btn" value="Submit" @click="login"/>

      <span class="error-msg" v-if="error">{{ error }}</span>
      <p class="text-muted">&copy; 2021</p>
    </form>
  </div>
</template>

<style lang="scss">


.form-wrapper {
  display: -ms-flexbox;
  display: -webkit-box;
  display: flex;
  -ms-flex-align: center;
  -ms-flex-pack: center;
  -webkit-box-align: center;
  align-items: center;
  -webkit-box-pack: center;
  justify-content: center;
  padding-top: 40px;
  padding-bottom: 40px;
  background: linear-gradient(to right, #f5f9f9, #cddef4);
  height: 100vh;

  .form-signin {
    width: 100%;
    max-width: 330px;
    padding: 15px;
    margin: 0 auto;
    display: flex;
    justify-content: center;
    flex-direction: column;
    align-items: center;

    img, h1 {
      margin-bottom: 30px;

      &:not(img) {
        color: #000;
        font-size: 32px;
        font-weight: 500;
      }
    }

    .submit-btn, .text-muted {
      margin-top: 30px;
    }

    .error-msg{
      margin-top: 30px;
      color: #f22000;
      font-weight: 500;
      font-size: 16px;
    }

    input {
      position: relative;
      box-sizing: border-box;
      height: auto;
      padding: 10px;
      font-size: 16px;
      margin-top: 10px;
    }

    .submit-btn {
      display: inline-block;
      font-weight: 400;
      text-align: center;
      white-space: nowrap;
      vertical-align: middle;
      -webkit-user-select: none;
      -moz-user-select: none;
      -ms-user-select: none;
      user-select: none;
      padding: .375rem .75rem;
      font-size: 1rem;
      line-height: 1.5;
      border-radius: .25rem;
      color: #fff;
      background-color: #56ab2f;
      border: none;
      transition: all 1s;

      &:hover {
        background-color: #56ab2fdb;
      }

      &:focus {
        outline: none;
        border: none;
      }
    }
  }

}

</style>

<script>
import {mapActions, mapGetters, mapMutations} from 'vuex';

export default {
  name: 'Home',
  data() {
    return {
      username: '',
      password: '',
      error: null
    }
  },
  computed: {...mapGetters(['token', 'role'])},
  methods: {
    ...mapActions(['authenticate', 'userinfo', 'resetState']),
    ...mapMutations(['setLoading']),
    login: async function () {
      try {
        this.setLoading(true);
        await this.authenticate({
          'username': this.username,
          'password': this.password
        });

        await this.userinfo();
        if (this.role === 'admin') {
          await this.$router.push('admin');
        } else {
          await this.$router.push('form');
        }
        this.setLoading(false);

      } catch (e) {
        this.password = '';
        this.error = e.response.data.Description;
        this.setLoading(false);
      }
    },
    logout: function () {
      this.resetState();
      this.$router.push('/');
    }
  },
  async mounted() {
    setTimeout(async () => {
      switch (this.role) {
        case 'admin':
          await this.$router.push({path: '/admin'});
          break;
        case 'user':
          await this.$router.push({path: '/form'});
          break;
        default:
          break;
      }
    }, 100)
  }
}
</script>
