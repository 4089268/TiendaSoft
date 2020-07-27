import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

let rutaServidor ='http://127.0.0.1:5000/';

export default new Vuex.Store({
  state: {
    Busqueda:{
      PalabraBusqueda:'',
      Filtros:[]
    },
    Productos:[]
  },
  mutations: {
    ActualizarProductos:(state, datos)=>{
      // Verificar si lo que se recive es un arreglo      
      state.Productos = datos.productos;
    }
  },
  actions: {
    ObtenerProductos:(context, options)=>{
      $.ajax({
        type: "GET",
        url: rutaServidor+"productos",
        dataType: "json",
        success: (response)=>{
          context.commit("ActualizarProductos",response);
        },
        error:(err)=>{
          alert("Error en la consulta ");
        }
      });
    },
    Busqueda:(context)=>{
      $.ajax({
        type: "GET",
        url: rutaServidor+"busqueda/" + context.state.Busqueda.PalabraBusqueda,
        dataType: "json",
        success: (response)=>{
          context.commit("ActualizarProductos",response);
        },
        error:(err)=>{
          alert("Error en la consulta");
        }
      });

    }

  },
  modules: {
  }
})
