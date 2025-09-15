<script setup lang="ts">
import { useGamblerStore } from '@/stores/gambler'

const gamblerStore = useGamblerStore()

function updateName(event: Event) {
  const target = event.target as HTMLInputElement
  gamblerStore.setGamblerName(target.value)
}
</script>

<template>
  <div class="gambler-name-input-container">
    <label for="gamblerName" class="gambler-name-label">Nombre</label>
    <input
      id="gamblerName"
      :value="gamblerStore.gambler.name"
      @input="updateName"
      type="text"
      placeholder="Ingresa tu nombre"
      class="gambler-name-input"
    />
  </div>
</template>

<style scoped>
.gambler-name-input-container {
  display: flex;
  flex-direction: column;
  gap: 5px;
  width: 100%;
  max-width: 300px;
}

.gambler-name-label {
  font-size: 1em;
  color: #555;
}

.gambler-name-input {
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
  width: 100%;
  box-sizing: border-box;
  font-size: 1em;
}

.gambler-name-input::placeholder {
  color: #aaa;
}
</style>