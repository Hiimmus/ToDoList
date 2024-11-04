
<template>

  <div class="border border-s p-3 rounded">
    <table class="table">
      <thead>
        <tr>
          <th>Nazwa</th>
          <th>Akcje</th>
        </tr>
      </thead>

      <tbody>
        <tr v-for="item in todoLists" :key="item.todoListId">
          <td>
            <router-link :to="`/todo/list/${item.todoListId}`"
                         class="list-link d-inline-block text-decoration-none">
              <i class="bi bi-list-check"></i>
              <span class="fw-medium">{{ item.name }}</span>
              
            </router-link>
          </td>
          <td>

            <button @click="removeList(item.todoListId)" class="btn btn-sm btn-danger">Usuń</button>
          </td>
        </tr>
        <tr>

          <td>
            <input v-model="newListName" type="text" class="form-control form-control-sm" placeholder="Nazwa nowej listy">
          </td>
          <td>
            <button @click="addTodoList" class="btn btn-sm btn-success">Dodaj</button>
          </td>
        </tr>

      </tbody>
    </table>
  </div>
</template>







<script>
  import axios from 'axios';

  export default {

    data() {
      return {
        todoLists: [],
        newListName: '',
      };
    },
    methods:
    {
      async fetchTodoLists() {
        try {
          const response = await axios.get('/api/TodoList');
          this.todoLists = await response.data;
        } catch (error) {
          console.error("Error while fetching TODO list:", error);
        }
      },
      async addTodoList() {

        if (!this.newListName) {
          alert("Podaj nazwę nowej listy.");
          return;
        }
        try {
          const response = await axios.post('/api/TodoList/', { name: this.newListName });
          this.todoLists.push(response.data)
          this.newListName = '';
        } catch (error) {
          console.error("Error while adding TODO list:", error);
          alert("An error occurred while adding the list.");
        }
      },
      async removeList(listId) {
        try {
          await axios.delete(`/api/TodoList/${listId}`);
          this.todoLists = this.todoLists.filter(list => list.todoListId !== listId);
          this.fetchTodoLists();
        }
        catch (error) {
          console.error("Error while deleting TODO list:", error);
          alert("An error occurred while deleting the list.")
        }
      },

    },
    mounted() {
      this.fetchTodoLists();
    },
  };
</script>

<style scoped>

  ul {
    list-style-type: none;
    padding: 0;
  }

  li {
    margin-bottom: 10px;
  }

  button {
    background-color: #007bff;
    color: white;
    border: none;
    border-radius: 4px;
    padding: 5px 10px;
    cursor: pointer;
  }

  .list-link {
    padding: 0.25rem 0.5rem;
    border-radius: 4px;
    transition: background-color 0.2s;
  }

    .list-link:hover {
      background-color: rgba(0,0,0,0.05);
    }
</style>
