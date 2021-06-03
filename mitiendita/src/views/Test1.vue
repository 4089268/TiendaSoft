<template>
    <div class="main">
        <input type="button" value="ClickMe" v-on:click="Boton1CLick()" />
        <div v-for="(x,index) in datos" :key="index">
            {{x}}
        </div>
    </div>  
</template>

<script>
var thisComponent;
export default {
    name:"Test1",
    data() {
        return {
            datos:[],
            i:1
        }
    },
    methods: {
        Boton1CLick:()=>{
            thisComponent.datos.push("Elemento" + thisComponent.i);
            thisComponent.i += 1;

        }        
    },
    created(){thisComponent = this;},
}
</script>

<style scoped lang="scss">
    .main{
        display: flex;
        flex-direction: column;
        align-items: center;
    }
    input{
        width: 80px;
        padding: 10px;
    }

</style>