<template>
    <div class="root">

        <div class="col1">
            <img src="@/assets/logo2.png" style="height:90%; width:auto; margin-top:2px;"/>
        </div>

        <div class="col2">

            <div class="row r1" >
                <div class="titulo">Mi Tiendita</div>
                <div style="width:400px;">
                    <div class="ui icon fluid input">
                        <i class="search icon"></i>
                        <input type="text" placeholder="Search..."/>
                    </div>
                </div>
            </div>
            
            <div class="row r2"> 
                <div class="ui dropdown">
                    <div class="text">Categorias</div>
                    <i class="dropdown icon"></i>
                    <div class="menu">
                        <div class="item"><router-link to="/">Abarrotes</router-link></div>
                        <div class="item"><router-link to="/">Frutas y Verduras</router-link></div>
                        <div class="item"><router-link to="/">Enlatados</router-link></div>
                        <div class="item"><router-link to="/">Licores</router-link></div>
                        <div class="item"><router-link to="/">Limpieza</router-link></div>
                    </div>
                </div>            
                <router-link to="/">Ofertas</router-link>
                <router-link to="/">Historial</router-link>            
                <router-link to="/about">Ayuda</router-link>
            </div>

        </div>

    </div>    
</template>

<script>
    var thisComponent;
export default {
    name: 'Cabecera',
    props: {},
    data() {
        return {
            catProductos:[
                { title: 'CocaCola' },
                { title: 'Pepsi' },
                { title: 'Sabritas' },
                { title: 'Suavitel' },
                { title: 'Gamesa' },
                { title: 'Leche' },
                { title: 'Huevos' },
                { title: 'Pan' },
                { title: 'Carniceria' },
                { title: 'Lacteos' },
                { title: 'Cerveza' },
                { title: 'Licores' },
                { title: 'Verduras' },
                { title: 'Frutas' },
                { title: 'Tortillas' },
                { title: 'Aguacate' },
                { title: 'Botana' },
                { title: 'Agua' },
                { title: 'Galletas' },
                { title: 'Refrescos' },
                { title: 'Sorprendeme' }                
            ]
        }
    },
    mounted(){
        $('.ui.dropdown').dropdown();
        $('.ui.search').search({ source: thisComponent.catProductos});
    },
    beforeCreate(){
        thisComponent = this;
    }
}
</script>

<style scoped lang="scss">
    .root{
        padding: 4px;
        background-color: #f9a825;
        //background-color: #ff8f00;        
        justify-content: center;
        display: flex;
        flex-direction: row;
        .col1{
            width: 120px;
        }
        .col2{
            display: flex;
            flex-direction: column;
        }
    }

    .titulo{
        font-size: 1rem;
    }
    .r1{
        display: flex;
        flex-direction: row;
        justify-content: center;
        align-items: center;
        .titulo{
            font-weight: bold;
            font-size: 2rem;
            margin-left: 10px;
            margin-right: 30px;
        }
        height: 80px;
    }
    .r2 {        
        a,.ui.dropdown{ 
            margin-left: 1.5rem;
            font-weight: bold;
            font-size: 1rem;
            color: white;

            &.router-link-exact-active {
            color: #2c3e50;
            }
        }
    }
</style>
