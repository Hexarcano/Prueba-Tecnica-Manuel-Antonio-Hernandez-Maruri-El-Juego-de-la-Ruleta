<script setup lang="ts">
import { useGamblerStore } from '@/stores/gambler'

const gamblerStore = useGamblerStore()

function updateName(event: Event) {
  const target = event.target as HTMLInputElement
  gamblerStore.setGamblerName(target.value)
}
</script>

<template>
  <div>
    <label for="gamblerName">Nombre</label>
    <input
      id="gamblerName"
      :value="gamblerStore.gambler.name"
      @input="updateName"
      type="text"
      placeholder="Ingresa tu nombre"
    />
  </div>
</template>
