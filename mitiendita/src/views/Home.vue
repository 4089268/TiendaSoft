<template>
  <div class="home">
        
    <div class="ui link cards">
      <!-- <div class="ui grid container"> -->
      <div class="ui stackable four column grid">
        
        <div class="four wide column">
          <CartaProducto Imagen="http:/host.com/a/imagen?x=102" Titulo="Doritos Nachos" Descripcion="Bolsa Doritos Nachos de 55 grs." :Precio="18.20"/>
        </div>
        <div class="four wide column">
          <CartaProducto Imagen="http:/host.com/a/imagen?x=102" Titulo="Doritos Nachos" Descripcion="Bolsa Doritos Nachos de 55 grs." :Precio="9.50" :Promocion="true" :PrecioPromocion="8.2"/>
        </div>
        <div class="four wide column">
          <CartaProducto Imagen="http:/host.com/a/imagen?x=102" Titulo="Doritos Nachos" Descripcion="Bolsa Doritos Nachos de 55 grs." :Precio="18.20"/>
        </div>
        <div class="four wide column">
          <CartaProducto Imagen="http:/host.com/a/imagen?x=102" Titulo="Doritos Nachos" Descripcion="Bolsa Doritos Nachos de 55 grs." :Precio="18.20"/>
        </div>
        <div class="four wide column">
          <CartaProducto Imagen="http:/host.com/a/imagen?x=102" Titulo="Doritos Nachos" Descripcion="Bolsa Doritos Nachos de 55 grs." :Precio="18.20"/>
        </div>
        <div class="four wide column">
          <CartaProducto Imagen="http:/host.com/a/imagen?x=102" Titulo="Doritos Nachos" Descripcion="Bolsa Doritos Nachos de 55 grs." :Precio="9.50" :Promocion="true" :PrecioPromocion="8.2"/>
        </div>
        <div class="four wide column">
          <CartaProducto Imagen="http:/host.com/a/imagen?x=102" Titulo="Doritos Nachos" Descripcion="Bolsa Doritos Nachos de 55 grs." :Precio="18.20"/>          
        </div>
        <div class="four wide column">
          <CartaProducto Imagen="http:/host.com/a/imagen?x=102" Titulo="Doritos Nachos" Descripcion="Bolsa Doritos Nachos de 55 grs." :Precio="18.20"/>
        </div>
      </div>
    </div>
    
  </div>
</template>

<script>
import CartaProducto from "@/components/CartaProducto.vue"

export default {
  name: 'Home',
  components: { CartaProducto}
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
