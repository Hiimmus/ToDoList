import { createRouter, createWebHistory } from 'vue-router';
import MainLayout from '../pages/MainLayout.vue';
import HomePage from '../pages/HomePage.vue';
import AboutPage from '../pages/AboutPage.vue';
import TodoList from '../components/TaskPanel/TodoList.vue';
import AllTodosByDate from '../components/TaskPanel/AllTodosByDate.vue';

const routes = [
  {
    path: '/',
    component: MainLayout,
    children: [
      {
        path: 'home',
        name: 'Home',
        component: HomePage,
      },
      {
        path: 'about',
        name: 'About',
        component: AboutPage,
      },

    ],
  },
  {
    path: '/todo/list/:id', 
    name: 'TodoList', 
    component: TodoList,
    props: route => ({
      listId: Number(route.params.id) 
    })
  },
  {
    path: '/todos/date/:date',
    name: 'AllTodosByDate',
    component: AllTodosByDate,
    props: (route) => ({
      selectedDate: route.params.date,
    }),
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router;
