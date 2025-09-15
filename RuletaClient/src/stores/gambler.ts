import { computed, reactive } from 'vue'
import { defineStore } from 'pinia'
import type { Gambler } from '@/types/gambler'

export const useGamblerStore = defineStore('gambler', () => {
  const gambler = reactive({
    name: '',
    initialFunds: 0,
    currentFunds: 0,
  })

  const isIdentified = reactive({ value: false })

  const canWithdraw = computed(() => (funds: number) => gambler.currentFunds >= funds)

  function addFunds(funds: number): boolean {
    if (funds <= 0) {
      return false
    }

    gambler.currentFunds += funds
    return true
  }

  function withdrawFunds(funds: number): boolean {
    if (funds <= 0 || funds > gambler.currentFunds) return false

    gambler.currentFunds -= funds
    return true
  }

  function setGamblerName(name: string): void {
    gambler.name = name.trim()
  }

  function setGambler(newGambler: Gambler): void {
    gambler.name = newGambler.name
    gambler.initialFunds = newGambler.funds
    gambler.currentFunds = newGambler.funds
  }

  function setIdentified(identified: boolean): void {
    isIdentified.value = identified
  }

  function resetFunds(): void {
    gambler.initialFunds = gambler.currentFunds
  }

  return {
    gambler,
    isIdentified: computed(() => isIdentified.value),
    currentFunds: computed(() => gambler.currentFunds),
    canWithdraw,
    addFunds,
    withdrawFunds,
    setGamblerName,
    setGambler,
    setIdentified,
    resetFunds,
  }
})
