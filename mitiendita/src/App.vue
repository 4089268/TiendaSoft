<template>
  <div id="app">
    <Cabecera class="cabecera"/>    
    <router-view/>
  </div>
</template>

<script>
  import Cabecera from '@/components/Cabecera.vue'
  export default {
    components:{ Cabecera }    
  }
</script>

<style lang="scss">
  html{
    font-size: 14px;
  }
  .cabecera{
    height: 80px;
  }

  #app {
    font-family: Avenir, Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    text-align: center;
    color: #2c3e50;
  }  
</style>