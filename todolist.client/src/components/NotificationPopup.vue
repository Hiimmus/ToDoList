<template>
  <div v-if="visible" class="global-notification-overlay">
    <div class="global-notification-popup">
      <div class="notification-header">
        <h3>Nadchodzące zadania</h3>
        <button class="close-button" @click="closePopup">×</button>
      </div>
      <div class="notification-content">
        <ul v-if="tasks.length > 0">
          <li v-for="task in tasks" :key="task.todoId" class="task-item">
            <div class="task-title">{{ task.name }}</div>
            <div class="task-date">{{ formatDate(task.dueDate) }}</div>
          </li>
        </ul>
        <p v-else>Brak nadchodzących zadań</p>
      </div>
    </div>
  </div>
</template>

<script>
  import EventBus from '../EventBus.js';

  export default {
    name: 'NotificationPopup',
    props: {
      visible: {
        type: Boolean,
        default: false,
      },
      tasks: {
        type: Array,
        default: () => [],
      }
    },
    data() {
      return {
      };
    },
    methods: {
      closePopup() {
        this.$emit('close');
      },
      formatDate(date) {
        return new Date(date).toLocaleDateString('pl-PL', {
          day: 'numeric',
          month: 'long',
          year: 'numeric',
          hour: '2-digit',
          minute: '2-digit'
        });
      }
    },
    mounted() {

    },
    beforeDestroy() {

    }
  };
</script>

<style scoped>
  .global-notification-overlay {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background-color: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 9999;
  }

  .global-notification-popup {
    background: white;
    border-radius: 8px;
    width: 90%;
    max-width: 500px;
    max-height: 80vh;
    overflow-y: auto;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  }

  .notification-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 1rem;
    border-bottom: 1px solid #eee;
  }

    .notification-header h3 {
      margin: 0;
      color: #333;
    }

  .close-button {
    background: none;
    border: none;
    font-size: 1.5rem;
    cursor: pointer;
    padding: 0.5rem;
    line-height: 1;
  }

  .notification-content {
    padding: 1rem;
  }

  .task-item {
    padding: 0.75rem;
    border-bottom: 1px solid #eee;
    list-style: none;
  }

    .task-item:last-child {
      border-bottom: none;
    }

  .task-title {
    font-weight: 500;
    margin-bottom: 0.25rem;
  }

  .task-date {
    font-size: 0.9rem;
    color: #666;
  }

  ul {
    margin: 0;
    padding: 0;
  }
</style>
