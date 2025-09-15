<script setup lang="ts">
import { ref, reactive, computed, watch } from 'vue'
import { useGamblerStore } from '@/stores/gambler'
import type { Bet, BetResponse } from '@/types/bet'
import { EColor, ECondition } from '@/types/bet'
import { betApi } from '@/services/betApi'

const gamblerStore = useGamblerStore()

const currentBet = reactive<Bet>({
  bet: 0,
  chosenColor: EColor.Red,
  chosenNumber: null,
  chosenCondition: null,
})

const betType = ref<'color' | 'number' | 'condition'>('color')

const betResult = ref<BetResponse | null>(null)
const lastBet = ref<Bet | null>(null)
const betPlaced = ref(false)
const errorMessage = ref('')

watch(betType, (newType) => {
  if (newType === 'color') {
    currentBet.chosenNumber = null
    currentBet.chosenCondition = null
  }

  if (newType === 'number') {
    currentBet.chosenCondition = null
    currentBet.chosenNumber = 0
  }

  if (newType === 'condition') {
    currentBet.chosenNumber = null
    currentBet.chosenCondition = ECondition.Even
  }
})

const canPlaceBet = computed(() => {
  if (!currentBet.bet || currentBet.bet < 1) return false
  if (currentBet.bet > gamblerStore.currentFunds) return false
  return true
})

async function placeBet() {
  if (!canPlaceBet.value) {
    errorMessage.value = 'Cantidad apostada no válida.'
    return
  }

  const success = gamblerStore.withdrawFunds(currentBet.bet)

  if (!success) {
    errorMessage.value = 'Error al realizar la apuesta. Fondos insuficientes.'
    return
  }

  const response = await betApi.post(currentBet)

  if (response.success && response.data) {
    gamblerStore.addFunds(response.data.gains)

    betResult.value = response.data
    lastBet.value = { ...currentBet }
    betPlaced.value = true
    errorMessage.value = ''

    currentBet.bet = 0
  } else {
    gamblerStore.addFunds(currentBet.bet) // Refund
    errorMessage.value = `El servidor no pudo procesar la apuesta: ${response.message}`
  }
}

function cleanBet() {
  betPlaced.value = false
  betResult.value = null
  lastBet.value = null
  errorMessage.value = ''
}
</script>

<template>
  <div class="ruleta-game-container">
    <h3>Haz una apuesta</h3>

    <div class="bet-input-group">
      <label for="bet">Fondos apostados:</label>
      <input
        id="bet"
        type="number"
        v-model.number="currentBet.bet"
        placeholder="0.00"
        min="1"
        @input="cleanBet"
        class="bet-input"
      />
    </div>

    <div class="bet-type-selection">
      <label class="radio-label"><input type="radio" value="color" v-model="betType" /> Solo color</label>
      <label class="radio-label"><input type="radio" value="number" v-model="betType" /> Numero</label>
      <label class="radio-label"
        ><input type="radio" value="condition" v-model="betType" /> Condición (Par/Impar)</label
      >
    </div>

    <div class="bet-options">
      <div v-if="betType === 'number'" class="bet-option-group">
        <label for="number-select">Número (0-36):</label>
        <input
          id="number-select"
          type="number"
          v-model.number="currentBet.chosenNumber"
          min="0"
          max="36"
          class="bet-option-input"
        />
      </div>

      <div v-if="betType === 'condition'" class="bet-option-group">
        <label for="condition-select">Condición:</label>
        <select id="condition-select" v-model="currentBet.chosenCondition" class="bet-option-select">
          <option :value="ECondition.Even">Par</option>
          <option :value="ECondition.Odd">Impar</option>
        </select>
      </div>

      <div class="bet-option-group">
        <label for="color-select">Color:</label>
        <select id="color-select" v-model="currentBet.chosenColor" class="bet-option-select">
          <option :value="EColor.Red">Rojo</option>
          <option :value="EColor.Black">Negro</option>
        </select>
      </div>
    </div>

    <button @click="placeBet" :disabled="!canPlaceBet" class="place-bet-button">Realizar Apuesta</button>

    <div v-if="errorMessage" class="error-message">
      <p>{{ errorMessage }}</p>
    </div>

    <div v-if="betPlaced" class="bet-results">
      <p>Apuesta realizada.</p>

      <p v-if="lastBet">
        <strong>Tu apuesta:</strong>
        <span v-if="lastBet.chosenNumber !== null"> Número: {{ lastBet.chosenNumber }}</span>
        <span v-if="lastBet.chosenCondition !== null">
          Condición: {{ lastBet.chosenCondition === ECondition.Even ? 'Par' : 'Impar' }}</span
        >
        <span> Color: {{ lastBet.chosenColor === EColor.Red ? 'Rojo' : 'Negro' }}</span>
        <span> Monto: ${{ lastBet.bet }}</span>
      </p>

      <p v-if="betResult">
        <strong>Resultado:</strong>
        <span> Número: {{ betResult.generatedNumber }}</span>
        <span> Color: {{ betResult.generatedColor === EColor.Red ? 'Rojo' : 'Negro' }}</span>
        <span>
          Condición: {{ betResult.generatedCondition === ECondition.Even ? 'Par' : 'Impar' }}</span
        >
        <br />
        <strong>{{ betResult.gainsType }}</strong>
        <span v-if="betResult.gains > 0"> Has ganado: ${{ betResult.gains }}</span>
      </p>
    </div>
  </div>
</template>

<style scoped>
.ruleta-game-container {
  display: flex;
  flex-direction: column;
  gap: 20px;
  padding: 20px;
  border: 1px solid #eee;
  border-radius: 8px;
  background-color: #fdfdfd;
  width: 100%;
  max-width: 500px;
  box-shadow: 0 1px 5px rgba(0, 0, 0, 0.05);
}

h3 {
  text-align: center;
  color: #333;
  margin-bottom: 15px;
}

.bet-input-group,
.bet-option-group {
  display: flex;
  flex-direction: column;
  gap: 5px;
}

.bet-input,
.bet-option-input,
.bet-option-select {
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
  width: 100%;
  box-sizing: border-box;
  font-size: 1em;
}

.bet-type-selection {
  display: flex;
  justify-content: space-around;
  flex-wrap: wrap;
  gap: 10px;
}

.radio-label {
  display: flex;
  align-items: center;
  gap: 5px;
  cursor: pointer;
}

.bet-options {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

.place-bet-button {
  padding: 12px 20px;
  background-color: #28a745; /* Green for action */
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  font-size: 1.1em;
  transition: background-color 0.3s ease;
  width: 100%;
}

.place-bet-button:hover:not(:disabled) {
  background-color: #218838;
}

.place-bet-button:disabled {
  background-color: #cccccc;
  cursor: not-allowed;
}

.error-message {
  color: #dc3545; /* Red for errors */
  text-align: center;
  font-size: 0.95em;
}

.bet-results {
  background-color: #e2f0ff; /* Light blue for results */
  border: 1px solid #b3d9ff;
  border-radius: 5px;
  padding: 15px;
  font-size: 0.95em;
  line-height: 1.6;
}

.bet-results strong {
  color: #0056b3;
}

.bet-results span {
  margin-right: 10px;
}
</style>