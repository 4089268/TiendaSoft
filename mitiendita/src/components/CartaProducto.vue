<template>
    <div class="ui link card" v-on:click="CartaClick()">

        <div class="ui fluid image">        
            <!--- Hay un bug con el icono de promocion
            <div class="ui red ribbon label" v-if="Promocion">
                <i class="heart icon"></i> PROMOCION
            </div>
            -->
            
            <img src="@/assets/productos/papas.jpg">
        </div>

        <div class="center aligned content">
            <div class="header">{{Titulo}}</div>
            <div class="meta">
                <a class="ui tiny teal label">Cereales</a>
                <a class="ui tiny teal label">Mas vendido</a>                
            </div>
            <div class="description">{{Descripcion}} </div>
            <div class="precio" v-bind:class="{promo:Promocion}">{{FormatNumber("precio")}}</div>
            <div class="precio" v-if="Promocion">{{FormatNumber("promo")}}</div>
        </div>

        <div class="extra content center aligned">
            <div class="precio">{{FormatNumber("precio")}}</div>            
        </div>
        
      </div>
</template>
<script>
var thisComponent;
export default {
    props:{
        Imagen:String,
        Titulo:String,
        Meta:String,
        Descripcion:String,
        Precio:Number,
        Promocion:false,
        PrecioPromocion:0.0,
        CartaClickCallback:Function
    },
    methods: {
        FormatNumber:(parm)=>{
            if(parm =="promo"){
                return "$ " +  thisComponent.PrecioPromocion.toFixed(2).toString();
            }else{
                return "$ " +  thisComponent.Precio.toFixed(2).toString();
            }
        },
        CartaClick:(e)=>{
            thisComponent.CartaClickCallback(666);
        }        
    },
    created(){ thisComponent = this; }    
}
</script>
<style scoped lang="scss">
    .precio{
        margin-top:2px;
        font-size: 1.6rem;
        font-weight: bold;
        padding:8px 20px;
        &.promo{
            font-size: 1rem;
            text-decoration: line-through;
        }
    }   

</style>