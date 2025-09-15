<script setup lang="ts">
import { useBetStore } from '@/stores/bet'

const betStore = useBetStore()
</script>

<template>
  <div class="bet-display-container"><strong>Apuesta actual:</strong> ${{ betStore.bet.amount ?? 'N/A' }}</div>
</template>

<style scoped>
.bet-display-container {
  padding: 10px 15px;
  background-color: #e9e9e9; /* Light gray background */
  border-radius: 5px;
  font-size: 1.1em;
  color: #333;
  text-align: center; /* Keep center as per original minimalist design */

  max-width: 300px;
  box-sizing: border-box;
}

.bet-display-container strong {
  color: #555;
}
</style>