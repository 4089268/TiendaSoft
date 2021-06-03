import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'

Vue.use(VueRouter)

  const routes = [
  { path: '/', name: 'Home', component: Home },
  { path: '/Test1', name: 'Test1', component: () => import(/* webpackChunkName: "about" */ '../views/Test1.vue')  },
  { path: '/about', name: 'About', component: () => import(/* webpackChunkName: "about" */ '../views/About.vue')  }
]

const router = new VueRouter({
  routes
})

export default router
