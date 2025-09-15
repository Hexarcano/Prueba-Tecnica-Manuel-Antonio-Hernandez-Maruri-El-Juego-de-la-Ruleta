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
  <div>
    <button @click="handleIdentify" :disabled="isLoading || !gamblerStore.gambler.name.trim()">
      {{ isLoading ? 'Identificando...' : 'Identificarse' }}
    </button>
    <div v-if="message" :class="messageType">
      {{ message }}
    </div>
  </div>
</template>
