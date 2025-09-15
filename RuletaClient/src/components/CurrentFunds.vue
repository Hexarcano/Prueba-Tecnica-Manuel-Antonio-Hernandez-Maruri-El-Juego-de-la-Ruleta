<script setup lang="ts">
import { useGamblerStore } from '@/stores/gambler'

const gamblerStore = useGamblerStore()
</script>

<template>
  <div class="current-funds-container">
    <p><strong>Fondos actuales:</strong> ${{ gamblerStore.currentFunds }}</p>
  </div>
</template>

<style scoped>
.current-funds-container {
  padding: 10px 15px;
  background-color: #e0f7fa; /* Light blue background */
  border-radius: 5px;
  font-size: 1.2em;
  color: #00796b;
  text-align: center; /* Keep center as per original minimalist design */

  max-width: 300px;
  box-sizing: border-box;
}

.current-funds-container strong {
  color: #004d40;
}
</style>