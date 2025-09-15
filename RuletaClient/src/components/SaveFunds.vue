<script setup lang="ts">
import { ref } from 'vue'
import { useGamblerStore } from '@/stores/gambler'
import { gamblerApi, ETransaction } from '@/services/gamblerApi'

const gamblerStore = useGamblerStore()
const isLoading = ref<boolean>(false)
const message = ref<string>('')
const messageType = ref<'success' | 'error'>('success')

async function handleSaveFunds(): Promise<void> {
  const { name } = gamblerStore.gambler
  const { currentFunds, initialFunds } = gamblerStore.gambler

  isLoading.value = true

  try {
    let response

    if (gamblerStore.isIdentified) {
      const difference = currentFunds - initialFunds

      if (difference === 0) {
        showMessage('No hay cambios en los fondos para guardar.', 'success')
        isLoading.value = false
        return
      }

      const transactionType = difference > 0 ? ETransaction.Add : ETransaction.Withdraw
      const amount = Math.abs(difference)

      response = await gamblerApi.updateFunds(name, {
        funds: amount,
        transaction: transactionType,
      })

      if (response.success && response.data) {
        gamblerStore.setGambler({
          name: response.data.name,
          funds: response.data.funds || 0,
        })
        gamblerStore.resetFunds()
      }
    } else {
      response = await gamblerApi.create({
        name: name,
        funds: currentFunds,
      })

      if (response.success && response.data) {
        gamblerStore.setGambler({
          name: response.data.name,
          funds: response.data.funds || 0,
        })
        gamblerStore.setIdentified(true)
      }
    }

    if (response.success) {
      showMessage('Datos guardados correctamente', 'success')
    } else {
      showMessage('Error al guardar fondos', 'error')
    }
  } catch (error) {
    console.error('Error:', error)
    showMessage('Error de conexión', 'error')
  } finally {
    isLoading.value = false
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

<template>
  <div>
    <button @click="handleSaveFunds" :disabled="isLoading || !gamblerStore.gambler.name.trim()">
      {{ isLoading ? 'Guardando...' : 'Guardar' }}
    </button>

    <div v-if="message" :class="messageType">
      {{ message }}
    </div>

    <div v-if="gamblerStore.gambler.name">
      <p><strong>Jugador:</strong> {{ gamblerStore.gambler.name }}</p>
      <p v-if="gamblerStore.isIdentified">
        <strong>Fondos iniciales:</strong> ${{ gamblerStore.gambler.initialFunds }}
      </p>
      <p><strong>Fondos actuales:</strong> ${{ gamblerStore.currentFunds }}</p>
    </div>
  </div>
</template>
