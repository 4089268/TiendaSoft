<template>
  <div class="home">

    <div class="ui four column centered doubling grid">
      
      <div class="three wide column" v-for="(item,index) in Productos" :key="index">
        <CartaProducto Imagen="http:/host.com/a/imagen?x=102"
          :Titulo="item.Descripcion"
          :Descripcion="item.Descripcion"
          :Precio="66.66" 
          :CartaClickCallback="CartaClick"/>
      </div>

    </div>
      
    <div class="nodatos">
      <img class="ui small image" src="@/assets/sad.png" />
      <h2 class="ui header">        
        <div class="content">
          Sin Productos
          <div class="sub header">No hay productos que concuerden con la busqueda.</div>
        </div>
      </h2>
    </div>

    

    <div class="ui small modal">
      <div class="content">
        
        <div class="ui grid ">

          <div class="eight wide column">
            <div class="ui fluid image">
              <div class="ui red ribbon label">
                  <i class="heart icon"></i> PROMOCION
              </div>
              <img src="@/assets/productos/papas.jpg">
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
    }
  },
  created() {
    thisComponent = this;
  },
  mounted() {
    $('.ui.modal').modal({centered: true});
    var opciones ={};
    thisComponent.$store.dispatch("ObtenerProductos",opciones);    

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
