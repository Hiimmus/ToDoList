
<template>
  <div class="main-layout">
    <aside class="sidebar">
    
    </aside>

    <main class="content">
   
      <router-view /> 
    </main>
  </div>
</template>

<script>
export default {
  name: 'MainLayout',
    components: {
  },
};

</script>

<style scoped>
  .body {
    background-color: lightblue;
  }
  .main-layout {
    display: flex;
    height: 100vh;
  }

  .sidebar {
    width: 25%;
    background-color: #f0f4f8;
  }

  .content {
    width: 75%;
    padding: 20px;
  }
</style>
