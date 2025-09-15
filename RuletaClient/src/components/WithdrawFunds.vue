<template>
  <div class="withdraw-funds-container">
    <label for="withdraw-funds-input" class="withdraw-funds-label">Retirar</label>
    <input
      id="withdraw-funds-input"
      v-model.number="funds"
      type="number"
      min="0"
      max="99"
      step="0.01"
      :max="gamblerStore.currentFunds"
      placeholder="Ingrese cantidad"
      @keyup.enter="handleWithdrawFunds"
      class="withdraw-funds-input"
    />
    <button @click="handleWithdrawFunds" :disabled="!canWithdraw" class="withdraw-funds-button">Retirar</button>
    <p v-if="message" :class="messageClass" class="withdraw-funds-message">{{ message }}</p>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useGamblerStore } from '@/stores/gambler'

const gamblerStore = useGamblerStore()

const funds = ref<number | null>(null)
const message = ref<string>('')
const messageType = ref<'success' | 'error'>('success')

const canWithdraw = computed(() => {
  return funds.value !== null && funds.value > 0 && gamblerStore.canWithdraw(funds.value)
})

const messageClass = computed(() => ({
  'success-message': messageType.value === 'success',
  'error-message': messageType.value === 'error',
}))

function handleWithdrawFunds(): void {
  if (funds.value === null || funds.value <= 0) {
    showMessage('Por favor ingrese una cantidad válida', 'error')
    return
  }

  if (funds.value > gamblerStore.currentFunds) {
    showMessage('Fondos insuficientes', 'error')
    return
  }

  const success = gamblerStore.withdrawFunds(funds.value)

  if (success) {
    showMessage(`Se retiraron $${funds.value} correctamente`, 'success')
    funds.value = null
  } else {
    showMessage('Error al retirar fondos', 'error')
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
.withdraw-funds-container {
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

.withdraw-funds-label {
  font-size: 1em;
  color: #555;
  margin-bottom: 5px;
}

.withdraw-funds-input {
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
  width: 100%;
  box-sizing: border-box;
  font-size: 1em;
}

.withdraw-funds-input::placeholder {
  color: #aaa;
}

.withdraw-funds-button {
  padding: 10px 15px;
  background-color: #dc3545; /* Red */
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 1em;
  transition: background-color 0.3s ease;
  width: 100%;
}

.withdraw-funds-button:hover:not(:disabled) {
  background-color: #c82333;
}

.withdraw-funds-button:disabled {
  background-color: #cccccc;
  cursor: not-allowed;
}

.withdraw-funds-message {
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