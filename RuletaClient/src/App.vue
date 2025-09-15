<script setup lang="ts">
import Ruleta from './components/RuletaApp.vue'
</script>

<template>
  <Ruleta />
</template>

<style>
body {
  font-family: 'Arial', 'Helvetica', sans-serif;
  background-color: #f4f4f4; /* Light gray background */
  color: #333; /* Dark gray text */
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

#app {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  padding: 10px;
}
</style>
