<template>
  <div class="add-funds-container">
    <label for="add-funds-input" class="add-funds-label">Agregar fondos</label>
    <input
      id="add-funds-input"
      v-model.number="funds"
      type="number"
      min="0"
      max="99"
      step="0.01"
      placeholder="Ingrese cantidad"
      @keyup.enter="handleAddFunds"
      class="add-funds-input"
    />
    <button @click="handleAddFunds" :disabled="!isValidAmount" class="add-funds-button">Agregar</button>
    <p v-if="message" :class="messageClass" class="add-funds-message">{{ message }}</p>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useGamblerStore } from '@/stores/gambler'

const gamblerStore = useGamblerStore()

const funds = ref<number | null>(null)
const message = ref<string>('')
const messageType = ref<'success' | 'error'>('success')

const isValidAmount = computed(() => {
  return funds.value !== null && funds.value > 0
})

const messageClass = computed(() => ({
  'success-message': messageType.value === 'success',
  'error-message': messageType.value === 'error',
}))

function handleAddFunds(): void {
  if (!isValidAmount.value || funds.value === null) {
    showMessage('Por favor ingrese una cantidad válida', 'error')
    return
  }

  const success = gamblerStore.addFunds(funds.value)

  if (success) {
    showMessage(`Se agregaron $${funds.value} correctamente`, 'success')
    funds.value = null
  } else {
    showMessage('Error al agregar fondos', 'error')
  }
}

function showMessage(text: string, type: 'success' | 'error'): void {
  message.value = text
  messageType.value = type

  setTimeout(() => {
    message.value = ''
  }, 3000)
}
</script>

<style scoped>
.add-funds-container {
  display: flex;
  flex-direction: column;
  gap: 10px;
  align-items: center;
  padding: 15px;
  border: 1px solid #eee;
  border-radius: 5px;
  background-color: #f9f9f9;
  width: 100%;
  max-width: 300px;
}

.add-funds-label {
  font-size: 1em;
  color: #555;
  margin-bottom: 5px;
}

.add-funds-input {
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
  width: 100%;
  box-sizing: border-box;
  font-size: 1em;
}

.add-funds-input::placeholder {
  color: #aaa;
}

.add-funds-button {
  padding: 10px 15px;
  background-color: #4CAF50; /* Green */
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 1em;
  transition: background-color 0.3s ease;
  width: 100%;
}

.add-funds-button:hover:not(:disabled) {
  background-color: #45a049;
}

.add-funds-button:disabled {
  background-color: #cccccc;
  cursor: not-allowed;
}

.add-funds-message {
  margin-top: 10px;
  font-size: 0.9em;
  text-align: center;
}

.success-message {
  color: #4CAF50;
}

.error-message {
  color: #f44336;
}
</style>