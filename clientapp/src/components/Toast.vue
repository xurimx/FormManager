<template>
    <div class="alert-message" v-bind:class="{ 'success': success, 'error': !success , 'active': active}">
        <h1>Success</h1>
        <p>Your message has been successfully sent.</p>
        <svg id="Group_1730" data-name="Group 1730" xmlns="http://www.w3.org/2000/svg" width="24" height="24"
             viewBox="0 0 24 24">
            <path id="Path_123" data-name="Path 123" d="M0,0H24V24H0Z" fill="none"/>
            <line id="Line_1" data-name="Line 1" x1="12" y2="12" transform="translate(6 6)" fill="none" stroke="#fff"
                  stroke-linecap="round" stroke-linejoin="round" stroke-width="2"/>
            <path id="Path_12553" data-name="Path 12553" d="M0,0,6,6l6,6" transform="translate(6 6)" fill="none"
                  stroke="#fff" stroke-linecap="round" stroke-width="2"/>
        </svg>
    </div>
</template>

<script>
    import {mapGetters} from "vuex";

    export default {
        data() {
            return {
                success: true
            }
        },
        name: "Toast",
        computed: {...mapGetters(['active'])},
    }
</script>

<style scoped lang="scss">
    .alert-message {
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%, 50%);
        background-color: #4DAE51;
        border-radius: 10px;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        text-align: center;
        padding: 20px 200px;
        -webkit-box-shadow: 0px 0px 20px -5px rgba(0,0,0,1);
        -moz-box-shadow: 0px 0px 20px -5px rgba(0,0,0,1);
        box-shadow: 0px 0px 20px -5px rgba(0,0,0,1);
        opacity: 0;
        svg{
            pointer-events: none;
        }
        &.error{
            background-color: #F23933;
        }
        &.active{
            opacity: 1;
            svg{
                pointer-events: all;
            }
        }
    }
</style>