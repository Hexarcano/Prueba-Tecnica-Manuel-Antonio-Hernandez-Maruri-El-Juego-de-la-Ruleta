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
      showMessage(response.message  ?? 'Error al guardar fondos', 'error')
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
  <div class="save-funds-container">
    <button @click="handleSaveFunds" :disabled="isLoading || !gamblerStore.gambler.name.trim()" class="save-funds-button">
      {{ isLoading ? 'Guardando...' : 'Guardar' }}
    </button>

    <div v-if="message" :class="messageType" class="save-funds-message">
      {{ message }}
    </div>
  </div>
</template>

<style scoped>
.save-funds-container {
  display: flex;
  flex-direction: column;
  gap: 15px;
  align-items: center; /* Reverted to center */
  padding: 20px;
  border: 1px solid #eee;
  border-radius: 8px;
  background-color: #f9f9f9;
  width: 100%;
  max-width: 350px;
  box-shadow: 0 1px 5px rgba(0, 0, 0, 0.05);
}

.save-funds-button {
  padding: 12px 20px;
  background-color: #6c757d; /* Gray for neutral action */
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  font-size: 1.1em;
  transition: background-color 0.3s ease;
  width: 100%;
}

.save-funds-button:hover:not(:disabled) {
  background-color: #5a6268;
}

.save-funds-button:disabled {
  background-color: #cccccc;
  cursor: not-allowed;
}

.save-funds-message {
  margin-top: 10px;
  font-size: 0.95em;
  text-align: center;
}

.success {
  color: #4CAF50;
}

.error {
  color: #f44336;
}

.gambler-info {
  background-color: #e9ecef;
  border-radius: 5px;
  padding: 10px;
  width: 100%;
  box-sizing: border-box;
  font-size: 0.9em;
  color: #495057;
}

.gambler-info p {
  margin: 5px 0;
}

.gambler-info strong {
  color: #343a40;
}
</style>
