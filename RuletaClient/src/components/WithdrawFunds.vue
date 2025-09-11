<template>
  <div>
    <label for="withdraw-funds-input">Retirar</label>
    <input
      id="withdraw-funds-input"
      v-model.number="funds"
      type="number"
      min="0"
      step="0.01"
      :max="gamblerStore.currentFunds"
      placeholder="Ingrese cantidad"
      @keyup.enter="handleWithdrawFunds"
    />
    <button @click="handleWithdrawFunds" :disabled="!canWithdraw">Retirar</button>
    <p v-if="message" :class="messageClass">{{ message }}</p>
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
