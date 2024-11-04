<template>
  <nav class="corner-navbar">
    <div class="corner-navbar__content">
      <button class="menu-trigger" @click="toggleMenu">
        <span class="icon">📋</span>
        <span v-if="upcomingTasksCount > 0" class="task-counter">
          {{ upcomingTasksCount }}
        </span>
      </button>

      <div class="corner-navbar__menu" :class="{ 'menu-open': isMenuOpen }">
        <ul>
          <li>
            <router-link to="/home" @click.native="isMenuOpen = false">
              <span class="icon">🏠</span>
              <span class="text">Home</span>
            </router-link>
          </li>
          <li>
            <router-link to="/" @click.native="handleUpcomingTodosClick">
              <span class="icon">📋</span>
              <span class="text">Zadania</span>
              <span v-if="upcomingTasksCount > 0" class="task-counter">
                {{ upcomingTasksCount }}
              </span>
            </router-link>
          </li>
        </ul>
      </div>
    </div>
  </nav>
  <NotificationPopup v-if="showPopup" :tasks="tasks" :visible="showPopup" @close="closePopup" />
</template>

<script>
  import axios from 'axios';
  import EventBus from '../EventBus.js';
  import NotificationPopup from './NotificationPopup.vue';
  export default {
    name: 'CornerNavbar',
    components: {
      NotificationPopup
    },
    data() {
      return {
        isMenuOpen: false,
        upcomingTasksCount: 0,
        checkInterval: null,
        tasks: [],
        showPopup: false,
      }
    },
    methods: {
      toggleMenu() {
        this.isMenuOpen = !this.isMenuOpen;
      },
      async checkUpcomingTasks() {
        try {
          const response = await axios.get('/api/todo/upcoming-todos?days=2');
          this.tasks = response.data;

          this.upcomingTasksCount = this.tasks.length;

          if (this.tasks.length > 0 && !this.showPopup) {
            this.showPopup = true;
            setTimeout(() => {
              this.showNotification = false;
            }, 5000);
          }

        } catch (error) {
          console.error("Error while checking tasks:", error);
        }
      },
      closePopup() {
        this.showPopup = false;
      },
      handleUpcomingTodosClick() {
        this.isMenuOpen = false;
        if (!this.showPopup && this.tasks.length > 0) {
          this.showPopup = true;

        }
      }
    },
    mounted() {

      this.checkUpcomingTasks();

      this.checkInterval = setInterval(this.checkUpcomingTasks, 300000);
    },
    beforeUnmount() {
      clearInterval(this.checkInterval);
    }
  }
</script>

<style scoped>
  .corner-navbar {
    position: fixed;
    top: 1rem;
    right: 1rem;
    z-index: 1000;
  }

  .corner-navbar__content {
    position: relative;
  }

  .menu-trigger {
    background: white;
    border: none;
    border-radius: 50%;
    width: 3rem;
    height: 3rem;
    display: flex;
    align-items: center;
    justify-content: center;
    cursor: pointer;
    box-shadow: 0 2px 5px rgba(0,0,0,0.2);
    position: relative;
    transition: transform 0.2s;
  }

    .menu-trigger:hover {
      transform: scale(1.05);
    }

    .menu-trigger .task-counter {
      position: absolute;
      top: -5px;
      right: -5px;
      background-color: #ff4444;
      color: white;
      border-radius: 50%;
      padding: 2px 6px;
      font-size: 0.8rem;
      min-width: 20px;
      height: 20px;
      display: flex;
      align-items: center;
      justify-content: center;
    }

  .corner-navbar__menu {
    position: absolute;
    top: calc(100% + 0.5rem);
    right: 0;
    background: white;
    border-radius: 8px;
    box-shadow: 0 2px 10px rgba(0,0,0,0.1);
    padding: 0.5rem;
    min-width: 150px;
    opacity: 0;
    visibility: hidden;
    transform: translateY(-10px);
    transition: all 0.2s;
  }

    .corner-navbar__menu.menu-open {
      opacity: 1;
      visibility: visible;
      transform: translateY(0);
    }

    .corner-navbar__menu ul {
      list-style: none;
      margin: 0;
      padding: 0;
    }

    .corner-navbar__menu li {
      margin: 0.5rem 0;
    }

    .corner-navbar__menu a {
      display: flex;
      align-items: center;
      padding: 0.5rem;
      text-decoration: none;
      color: #333;
      border-radius: 4px;
      transition: background-color 0.2s;
    }

      .corner-navbar__menu a:hover {
        background-color: #f5f5f5;
      }

    .corner-navbar__menu .icon {
      margin-right: 0.5rem;
    }

    .corner-navbar__menu .text {
      font-size: 0.9rem;
    }

    .corner-navbar__menu .task-counter {
      margin-left: auto;
      background-color: #ff4444;
      color: white;
      border-radius: 50%;
      padding: 2px 6px;
      font-size: 0.8rem;
      min-width: 20px;
      height: 20px;
      display: flex;
      align-items: center;
      justify-content: center;
    }
</style>
