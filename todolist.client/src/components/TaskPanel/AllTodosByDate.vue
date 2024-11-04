<template>
  <div>
    <h2>Lista zadań na dzień {{ selectedDate }}</h2>
    <div v-if="todos.length === 0">Brak zadań do wyświetlenia.</div>
    <div v-else>
      <todo-item v-for="todo in todos"
                 :key="todo.todoId"
                 :todo="todo"
                 @delete="deleteTodo"
                 @edit="editTodo" />
    </div>
  </div>
</template>

<script>
  import TodoItem from './TodoItem.vue';
  import EventBus from '../../EventBus.js';
  import axios from 'axios';
  export default {
    components: {
      TodoItem,
      axios,
    },
    props: {
      selectedDate: {
        type: String,
        required: true,
      },
    },
    data() {
      return {
        todos: [],
      };
    },
    mounted() {
      EventBus.on('updateTodos', (todos) => {
        this.todos = todos;
      });
    },
    methods: {
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
          const index = this.todos.findIndex(todo => todo.todoId === todoId);
          if (index !== -1) {
            this.todos.splice(index, 1, response.data);
          }
          onSuccess(true);
        } catch (error) {
          console.error("Error while editing:", error);
          onError(error);
        }
      },
      async deleteTodo(todoId) {
        try {
          await axios.delete(`/api/Todo/${todoId}`);
          this.todos = this.todos.filter(todo => todo.todoId !== todoId);
        }
        catch (error) {
          console.error("Error deleting todo:", error);
        }
      },
    },
    beforeDestroy() {
      EventBus.$off('updateTodos');
    },

  };
</script>
