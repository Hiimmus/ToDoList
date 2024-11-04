<template>
  <div class="todo-card">
    <div v-if="isEditing">
      <input v-model="editedTodo.name" placeholder="Nazwa" class="form-input" />
      <input v-model="editedTodo.description" placeholder="Opis" class="form-input" />
      <select v-model="editedTodo.priority" class="form-select">
        <option value="1">Niski</option>
        <option value="2">Średni</option>
        <option value="3">Wysoki</option>
      </select>
      <input type="date" v-model="editedTodo.dueDate" class="form-input" />

      <button class="btn-minimal" @click="saveEdit">Zapisz</button>
      <button class="btn-minimal" @click="cancelEdit">Anuluj</button>
    </div>

    <div v-else>
      <div class="todo-header">
        <h3>{{ todo.name }}</h3> 
        <span class="priority">{{ todo.priority }}</span>
      </div>

      <p class="description">{{ todo.description }}</p>

      <div class="todo-details">
        <div>
          <span class="label">Termin:</span>
          {{ formatDate(todo.dueDate) }}
        </div>

        <div>
          <input type="checkbox" v-model="todo.isCompleted" @change="updateStatus">
          <span class="label">Status:</span>
          {{ todo.isCompleted ? 'Zakończone' : 'W trakcie' }}
        </div>
      </div>

      <div class="todo-meta">
        <small>Utworzono: {{ formatDate(todo.createdDate) }}</small>
        <small>Aktualizacja: {{ formatDate(todo.updatedDate) }}</small>
      </div>

      <div class="todo-actions">
        <button class="btn-minimal" @click="startEdit">Edytuj</button>
        <button class="btn-minimal" @click="$emit('delete', todo.todoId)">Usuń</button>
      </div>
    </div>
  </div>
</template>

<script>
  import axios from 'axios';
  import EventBus from '../../EventBus.js';
  export default {
    name: 'TodoDetail',
    props: {
      todo: {
        type: Object,
        required: true
      }
    },
    data() {
      return {
        isEditing: false,
        editedTodo: {
          name: '',
          description: '',
          priority: 1,
          dueDate: '',
        }
      };
    },
    methods: {
      async updateStatus() {
        try {
          const response = await axios.patch(`/api/Todo/${this.todo.todoId}/status`, { isCompleted: this.todo.isCompleted });
          if (response.status === 204) {
            EventBus.emit('todoStatusUpdated', this.todo);
          } else {
            console.error("Error while updating task status:", response.status);

            this.todo.isCompleted = !this.todo.isCompleted;
          }
        } catch (error) {
          console.error("Error while updating task status:", error);
          this.todo.isCompleted = !this.todo.isCompleted;
        }
      },


      startEdit() {
        this.editedTodo = { ...this.todo };
        this.isEditing = true;
      },
      cancelEdit() {
        this.isEditing = false;
        this.editedTodo = { ...this.todo };
      },
      async saveEdit() {
        try {
          const success = await new Promise((resolve, reject) => {
            this.$emit('edit', {
              todoId: this.todo.todoId,
              updatedData: this.editedTodo,
              onSuccess: resolve,
              onError: reject
            });
          });
          if (success) {

            this.isEditing = false;
          }
        } catch (error) {
          console.error("Error while editing the task:", error);
        }
      },
      formatDate(dateString) {
        if (!dateString) return '';
        const date = new Date(dateString);
        return new Intl.DateTimeFormat('pl-PL', {
          year: 'numeric',
          month: '2-digit',
          day: '2-digit',
          hour: '2-digit',
          minute: '2-digit'
        }).format(date);
      }
    }
  }
</script>

<style scoped>
  .todo-card {
    padding: 1.5rem;
    background: white;
    border: 1px solid #eee;
    width: 40rem;
  }

  .todo-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 1rem;
  }

    .todo-header h3 {
      margin: 0;
      font-size: 1.2rem;
    }

  .priority {
    color: #666;
  }

  .description {
    color: #444;
    margin-bottom: 1.5rem;
  }

  .todo-details {
    display: flex;
    gap: 2rem;
    margin-bottom: 1.5rem;
  }

  .label {
    color: #666;
    margin-right: 0.5rem;
  }

  .todo-meta {
    display: flex;
    justify-content: space-between;
    color: #888;
    font-size: 0.875rem;
    margin-bottom: 1.5rem;
  }

  .todo-actions {
    display: flex;
    gap: 1rem;
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

