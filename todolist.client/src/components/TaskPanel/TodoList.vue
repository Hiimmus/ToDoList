<template>
  <div v-if="todoList">
    <h1>{{ todoList.name }}</h1>
    <button @click="showForm = !showForm" class="btn btn-sm btn-success">Dodaj</button>

    <div v-if="showForm">
      <form @submit.prevent="addTodoToList" class="todo-card">
        <div class="todo-header">
          <h3>Dodaj nowe zadanie</h3>
        </div>
        <input v-model="todoName" placeholder="Nazwa" required class="form-input" />
        <input v-model="todoDescription" placeholder="Opis" class="form-input" />
        <select v-model="todoPriority" class="form-select">
          <option value="1">Niski</option>
          <option value="2">Średni</option>
          <option value="3">Wysoki</option>
        </select>
        <input type="date" v-model="dueDate" required class="form-input" />
        <button type="submit" class="btn-minimal">Zapisz</button>
        <button type="button" @click="showForm = false" class="btn-minimal">Anuluj</button>
      </form>
    </div>

    <TodoItem v-for="todo in todoList.todos"
              :key="todo.todoId"
              :todo="todo"
              @delete="deleteTodo"
              @edit="editTodo" />
  </div>
  <div v-else>
    <p>Loading...</p>
  </div>
</template>

<script>
  import axios from 'axios';
  import { ref } from 'vue';
  import TodoItem from './TodoItem.vue';

  const todoList = ref({ todos: [] })
  const isEditing = ref(false)
  const editError = ref(null)

  export default {
    components: {
      TodoItem
    },
    props: {
      listId: {
        type: Number,
        required: true
      }
    },
    data() {
      return {
        todoList: null,
        currentListId: this.listId,
        todoName: '',
        todoDescription: '',
        todoPriority: 1,
        dueDate: '',
        showForm: false
      };
    },
    async mounted() {
      await this.fetchTodoList();
    },
    async beforeRouteUpdate(to, from) {
      this.currentListId = Number(to.params.id);
      await this.fetchTodoList();
    },
    watch: {
      listId: {
        immediate: true,
        handler(newValue) {
          this.currentListId = newValue;
        }
      }
    },
    methods:
    {
      async fetchTodoList() {
        try {
          const response = await axios.get(`/api/Todo/list/${this.currentListId}`);
          this.todoList = response.data;
        } catch (error) {
          console.error("Error fetching todo list:", error);

        }
      },

      async addTodoToList(todoListid) {
        const newTodo =
        {
          todoListId: this.currentListId,
          name: this.todoName,
          description: this.todoDescription,
          isCompleted: false,
          dueDate: this.dueDate,
          priority: this.todoPriority
        };
        try {
          const response = await axios.post(`/api/Todo`, newTodo,
            {
              headers:
              {
                'Accept': 'text/plain',
                'Content-Type': 'application/json'
              }
            });
          this.todoList.todos.push(response.data);
          this.resetForm();
          this.fetchTodoList();
        }
        catch (error) {
          console.error('Error adding todo:', error);
        }

      },
      resetForm() {
        this.todoName = '';
        this.todoDescription = '';
        this.todoPriority = 1;
        this.dueDate = '';
        this.showForm = false;
      },

      async editTodo({ todoId, updatedData, onSuccess, onError }) {
        try {
          const response = await axios.put(
            `/api/Todo/${todoId}`,
            updatedData,
            {
              headers: {
                'Content-Type': 'application/json',
              },
            }
          );
          const index = this.todoList.todos.findIndex(todo => todo.todoId === todoId);
          if (index !== -1) {
            this.todoList.todos.splice(index, 1, response.data);
          }

          onSuccess(true);
        } catch (error) {
          console.error('Error while editing todo:', error);
          onError(error);
        }
      },
      async deleteTodo(todoId) {
        try {
          await axios.delete(`/api/Todo/${todoId}`);
          this.todoList.todos = this.todoList.todos.filter(todo => todo.todoId !== todoId);
          await this.fetchTodoList();
        }
        catch (error) {
          console.error("Error deleting todo:", error);
        }
      }
    }
  };



</script>
<style scoped>
  .todo-card {
    padding: 1.5rem;
    background: white;
    border: 1px solid #eee;
    width: 40rem;
    margin-bottom: 1rem;
  }

  .todo-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 1rem;
  }

  .form-input {
    width: 100%;
    padding: 0.5rem;
    margin-bottom: 1rem;
    border: 1px solid #ccc;
    border-radius: 4px;
  }

  .form-select {
    width: 100%;
    padding: 0.5rem;
    margin-bottom: 1rem;
    border: 1px solid #ccc;
    border-radius: 4px;
  }

  .btn-minimal {
    background: none;
    border: 1px solid #ddd;
    padding: 0.5rem 1rem;
    cursor: pointer;
  }

    .btn-minimal:hover {
      background: #f5f5f5;
    }
</style>
