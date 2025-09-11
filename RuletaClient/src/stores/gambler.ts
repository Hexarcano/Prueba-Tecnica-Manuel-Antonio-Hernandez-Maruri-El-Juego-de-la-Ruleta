import { computed, reactive } from 'vue'
import { defineStore } from 'pinia'
import type { Gambler } from '@/types/gambler'

export const useGamblerStore = defineStore('gambler', () => {
  const gambler = reactive<Gambler>({
    id: '',
    name: '',
    funds: 0,
  })

  const currentFunds = computed(() => gambler.funds)
  const canWithdraw = computed(() => (funds: number) => gambler.funds >= funds)

  function addFunds(funds: number): boolean {
    if (funds <= 0) {
      return false
    }

    gambler.funds += funds
    return true
  }

  function withdrawFunds(funds: number): boolean {
    if (funds <= 0 || funds > gambler.funds) return false

    gambler.funds -= funds
    return true
  }

  return {
    gambler,
    currentFunds,
    canWithdraw,
    addFunds,
    withdrawFunds,
  }
})
