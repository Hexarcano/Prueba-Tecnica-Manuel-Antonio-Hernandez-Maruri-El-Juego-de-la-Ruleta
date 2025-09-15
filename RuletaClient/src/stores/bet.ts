import { reactive } from 'vue'
import { defineStore } from 'pinia'

export const useBetStore = defineStore('bet', () => {
  const bet = reactive({
    amount: 0,
  })

  function placeBet(amount: number): void {
    bet.amount = amount
  }

  return {
    bet,
    placeBet,
  }
})
