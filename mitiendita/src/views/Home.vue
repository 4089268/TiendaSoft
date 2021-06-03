
<template>
    <div class="home" ref="mainRoot">

      <div class="ui four column centered doubling grid">
        
        <div class="three wide column" v-for="(item,index) in TmpProductos" :key="index">
          <CartaProducto :IdProducto="item.Id"
            :Titulo="item.Descripcion"
            :Descripcion="item.Descripcion"
            :Precio="66.66" 
            :CartaClickCallback="CartaClick"/>
        </div>

      </div>
        
      <div class="nodatos" v-on:click="Obtener20Productos">
        <img class="ui small image" src="@/assets/sad.png" />
        <h2 class="ui header">
          <div class="content">
            Sin Productos
            <div class="sub header">No hay productos que concuerden con la busqueda.</div>
          </div>
        </h2>
      </div>


      <!-- *********** Modal para ver los detalles del producto seleccionado *********** -->
      <div class="ui small modal">
        <div class="content">
          
          <div class="ui grid ">

            <div class="eight wide column">
              <div class="ui fluid image">
                <div class="ui red ribbon label">
                    <i class="heart icon"></i> PROMOCION
                </div>
                <!-- <img src="@/assets/productos/papas.jpg"> -->              
                <img src="http://127.0.0.1:5000/imagen/6666">              
              </div>
            </div>

            <div class="eight wide column">

              <div class="ui grid" style="height:100%;">
                <div class="row">
                  <h2>Producto xxxxx</h2>
                </div>              
                <div class="row">
                  <div> Precio </div>
                  <div> Promocion </div>
                  <div> Disponibles </div>
                  <div> Cantidad </div>
                </div>
                <div class="bottom aligned row">
                  <div class="ui fluid teal button">Comprar ahora</div>
                  <div class="ui fluid blue button">Agregar al carrito</div>
                </div>
              </div>

            </div>

          </div>
          
        </div>
      </div>

    </div>
  </template>

<script>
  import CartaProducto from "@/components/CartaProducto.vue"
  import { mapState } from 'vuex'

  var thisComponent;
  export default {
    name: 'Home',
    components: { CartaProducto},
    data() {
      return {
        TmpProductos:[]
      }
    },
    computed: {
      ObtenerProductos:()=>{      
        console.dir(thisComponent)
        return thisComponent.$store.Productos;
      },
      ...mapState(['Productos'])

    },  
    methods: {
      CartaClick:(e)=>{
        $('.ui.modal').modal('show');
      },
      Obtener20Productos:()=>{
        console.log("cargando productos");
        for( var i = 0; i < 20; i++ ){
          thisComponent.TmpProductos.push(thisComponent.Productos[i]);
        }
      },
      ScrollEvent:(e)=>{
        const {scrollTop, scrollHeight, clientHeight }  = document.documentElement;
        //console.log({scrollTop, scrollHeight, clientHeight});
        
                      
        if ((scrollTop + clientHeight) >= (scrollHeight - 10)) {
          console.log("CargandoProductos");
          thisComponent.Obtener20Productos();
        
        }
      }
    },
    created() {
      thisComponent = this;
    },
    mounted() {
      $('.ui.modal').modal({centered: true});
      var opciones ={};
      window.addEventListener("scroll", thisComponent.ScrollEvent);
      thisComponent.$store.dispatch("ObtenerProductos",opciones);
    },
    beforeDestroy(){
      window.removeEventListener("scroll",thisComponent.ScrollEvent);
      thisComponent.Obtener20Productos();
    }

  }
</script>

<style lang="scss">

  .home{
    padding: 20px;
    display: flex;
    flex-direction: column;
    align-items: center;  
  }
  .nodatos{
    height: 70vh;
    width:100%;    
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
  }
</style>
