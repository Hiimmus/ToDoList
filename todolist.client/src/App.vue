<template>
  <div class="app-container">

    <div class="sidebar">
      <div class="input-group ms-6">
        <datepicker v-model="dueDate"
                    inline
                    @update:modelValue="onDateSelect"
                    placeholder="Wybierz datę"
                    class="form-control"></datepicker>
        <span class="input-group-text border-0"
              @click="fetchTodos"
              style="cursor: pointer;">
          <i class="bi bi-search"></i>

          <Loader v-if="isLoading" />
        </span>

        <span class="input-group-text border-0"
              style="cursor: pointer;">
          <i class="bi bi-calendar"></i>
        </span>
      </div>

      <ListsSidebar />
    </div>
    <div class="content">
      <corner-navbar />
      <router-view />
    </div>
  </div>

</template>


<script>
  import ListsSidebar from './components/ListsSidebar/ListsSidebar.vue';
  import TodoList from './components/TaskPanel/TodoList.vue';
  import Datepicker from 'vue3-datepicker';
  import axios from 'axios';
  import EventBus from './EventBus.js';
  import Loader from './components/ListsSidebar/Loader.vue';
  import CornerNavbar from './components/CornerNavbar.vue';

  export default {
    components: {
      ListsSidebar,
      TodoList,
      Datepicker,
      Loader,
      CornerNavbar,
    },
    name: 'App',
    data() {
      return {
        showCalendar: false,
        dueDate: null,
        todos: [],
        searchQuery: '',
        isLoading: false,
      };
    },
    methods: {


      mounted() {
        this.dueDate = new Date();
        this.dueDate = new Date(this.dueDate.toLocaleDateString('en-CA'));
      },

      async onDateSelect() {

        document.activeElement.blur()
        this.fetchTodos();
        setTimeout(() => {
          this.fetchTodos();
        }, 0);
      },

      async fetchTodos() {

        this.isLoading = true;
        if (!this.dueDate) {
          this.isLoading = false;
          return;
        }
        const formattedDate = this.dueDate.toLocaleDateString('en-CA');

        try {
          const response = await axios.get(`/api/Todo/date/${formattedDate}`);
          this.todos = response.data;
          EventBus.emit('updateTodos', this.todos);

          this.$router.push({
            name: 'AllTodosByDate',
            params: { date: formattedDate },
          });

        } catch (error) {
          console.error("Error while fetching tasks:", error);
        } finally {
          this.isLoading = false;
        }


      },
    }

  };


</script>


<style scoped>
  .app-container {
    display: flex;
    height: 100vh;
    background-color: #f0f4f8;
    color: #333;
    font-family: Arial, sans-serif;
  }
  .sidebar {
    width: 25%;
    background-color: #ffffff;
    padding: 20px;
    box-shadow: 2px 0 5px rgba(0, 0, 0, 0.1);
  }

    .sidebar h2 {
      font-size: 1.5em;
      margin-bottom: 20px;
    }

    .sidebar ul {
      list-style: none;
      padding: 0;
    }

    .sidebar li {
      margin: 10px 0;
    }

    .sidebar button {
      width: 100%;
      padding: 10px;
      background-color: #007bff;
      color: #fff;
      border: none;
      cursor: pointer;
      font-size: 1em;
    }

      .sidebar button:hover {
        background-color: #0056b3;
      }
  .content {
    flex: 1;
    padding: 20px;
    overflow-y: auto;
  }
</style>
