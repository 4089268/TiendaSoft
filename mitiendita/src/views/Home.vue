<template>
  <div class="home">

    <div class="ui four column centered doubling grid">
      <div class="three wide column">
        <CartaProducto Imagen="http:/host.com/a/imagen?x=102" Titulo="Doritos Nachos" Descripcion="Bolsa Doritos Nachos de 55 grs." :Precio="18.20" :CartaClickCallback="CartaClick"/>
      </div>
      <div class="three wide column">
        <CartaProducto Imagen="http:/host.com/a/imagen?x=102" Titulo="Doritos Nachos" Descripcion="Bolsa Doritos Nachos de 55 grs." :Precio="9.50" :Promocion="true" :PrecioPromocion="8.2" :CartaClickCallback="CartaClick"/>
      </div>
      <div class="three wide column">
        <CartaProducto Imagen="http:/host.com/a/imagen?x=102" Titulo="Doritos Nachos" Descripcion="Bolsa Doritos Nachos de 55 grs." :Precio="18.20" :CartaClickCallback="CartaClick"/>
      </div>
      <div class="three wide column">
        <CartaProducto Imagen="http:/host.com/a/imagen?x=102" Titulo="Doritos Nachos" Descripcion="Bolsa Doritos Nachos de 55 grs." :Precio="18.20" :CartaClickCallback="CartaClick"/>
      </div>
      <div class="three wide column">
        <CartaProducto Imagen="http:/host.com/a/imagen?x=102" Titulo="Doritos Nachos" Descripcion="Bolsa Doritos Nachos de 55 grs." :Precio="18.20" :CartaClickCallback="CartaClick"/>
      </div>
      <div class="three wide column">
        <CartaProducto Imagen="http:/host.com/a/imagen?x=102" Titulo="Doritos Nachos" Descripcion="Bolsa Doritos Nachos de 55 grs." :Precio="9.50" :Promocion="true" :PrecioPromocion="8.2" :CartaClickCallback="CartaClick"/>
      </div>
      <div class="three wide column">
        <CartaProducto Imagen="http:/host.com/a/imagen?x=102" Titulo="Doritos Nachos" Descripcion="Bolsa Doritos Nachos de 55 grs." :Precio="18.20" :CartaClickCallback="CartaClick"/>
      </div>
      <div class="three wide column">
        <CartaProducto Imagen="http:/host.com/a/imagen?x=102" Titulo="Doritos Nachos" Descripcion="Bolsa Doritos Nachos de 55 grs." :Precio="18.20" :CartaClickCallback="CartaClick"/>
      </div>
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
var thisComponent;
export default {
  name: 'Home',
  components: { CartaProducto},
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
</style>
