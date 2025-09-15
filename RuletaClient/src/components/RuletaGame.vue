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
  <div>
    <h3>Haz una apuesta</h3>

    <div>
      <label for="bet">Fondos apostados:</label>
      <input
        id="bet"
        type="number"
        v-model.number="currentBet.bet"
        placeholder="0.00"
        min="1"
        @input="cleanBet"
      />
    </div>

    <div>
      <label><input type="radio" value="color" v-model="betType" /> Solo color</label>
      <label><input type="radio" value="number" v-model="betType" /> Numero</label>
      <label
        ><input type="radio" value="condition" v-model="betType" /> Condición (Par/Impar)</label
      >
    </div>

    <div>
      <div v-if="betType === 'number'">
        <label for="number-select">Número (0-36):</label>
        <input
          id="number-select"
          type="number"
          v-model.number="currentBet.chosenNumber"
          min="0"
          max="36"
        />
      </div>

      <div v-if="betType === 'condition'">
        <label for="condition-select">Condición:</label>
        <select id="condition-select" v-model="currentBet.chosenCondition">
          <option :value="ECondition.Even">Par</option>
          <option :value="ECondition.Odd">Impar</option>
        </select>
      </div>

      <div>
        <label for="color-select">Color:</label>
        <select id="color-select" v-model="currentBet.chosenColor">
          <option :value="EColor.Red">Rojo</option>
          <option :value="EColor.Black">Negro</option>
        </select>
      </div>
    </div>

    <button @click="placeBet" :disabled="!canPlaceBet">Realizar Apuesta</button>

    <div v-if="errorMessage">
      <p style="color: red">{{ errorMessage }}</p>
    </div>

    <div v-if="betPlaced">
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
