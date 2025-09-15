<script setup lang="ts">
import { ref } from 'vue'
import { useGamblerStore } from '@/stores/gambler'
import { gamblerApi } from '@/services/gamblerApi'

const gamblerStore = useGamblerStore()
const isLoading = ref<boolean>(false)
const message = ref<string>('')
const messageType = ref<'success' | 'error'>('success')

async function handleIdentify(): Promise<void> {
  const name = gamblerStore.gambler.name

  if (!name.trim()) {
    showMessage('Por favor ingresa tu nombre', 'error')
    return
  }

  isLoading.value = true
  message.value = ''

  try {
    const response = await gamblerApi.get(name.trim())

    if (!response.success) {
      showMessage(response.message ?? 'Error desconocido', 'error')
      return
    }

    if (response.data) {
      const confirmation = confirm(
        'Se encontró un jugador con este nombre. ¿Deseas cargar sus datos? Se perderá cualquier progreso no guardado.',
      )

      if (confirmation) {
        gamblerStore.setGambler({
          name: response.data.name,
          funds: response.data.funds || 0,
        })

        gamblerStore.setIdentified(true)
      }
    }

    if (!response.data) {
      gamblerStore.setGambler({
        name: gamblerStore.gambler.name,
        funds: 0,
      })

      gamblerStore.setIdentified(false)
    }
  } catch (error) {
    console.log('Error identifying gambler:', error)
  } finally {
    isLoading.value = false
  }
}

function showMessage(text: string, type: 'success' | 'error'): void {
  message.value = text
  messageType.value = type
  setTimeout(() => {
    message.value = ''
  }, 5000)
}
</script>

<template>
  <div class="identify-button-container">
    <button @click="handleIdentify" :disabled="isLoading || !gamblerStore.gambler.name.trim()" class="identify-button">
      {{ isLoading ? 'Identificando...' : 'Identificarse' }}
    </button>
    <div v-if="message" :class="messageType" class="identify-message">
      {{ message }}
    </div>
  </div>
</template>

<style scoped>
.identify-button-container {
  display: flex;
  flex-direction: column;
  gap: 10px;
  align-items: center;
  width: 100%;
  max-width: 300px;
}

.identify-button {
  padding: 10px 15px;
  background-color: #007bff; /* Blue */
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 1em;
  transition: background-color 0.3s ease;
  width: 100%;
}

.identify-button:hover:not(:disabled) {
  background-color: #0056b3;
}

.identify-button:disabled {
  background-color: #cccccc;
  cursor: not-allowed;
}

.identify-message {
  margin-top: 10px;
  font-size: 0.9em;
  text-align: center;
}

.success {
  color: #4CAF50;
}

.error {
  color: #f44336;
}
</style>