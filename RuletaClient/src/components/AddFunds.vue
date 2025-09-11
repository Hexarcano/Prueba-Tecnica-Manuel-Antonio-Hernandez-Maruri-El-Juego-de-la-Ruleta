<template>
  <div>
    <label for="add-funds-input">Agregar fondos</label>
    <input
      id="add-funds-input"
      v-model.number="funds"
      type="number"
      min="0"
      step="0.01"
      placeholder="Ingrese cantidad"
      @keyup.enter="handleAddFunds"
    />
    <button @click="handleAddFunds" :disabled="!isValidAmount">Agregar</button>
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
