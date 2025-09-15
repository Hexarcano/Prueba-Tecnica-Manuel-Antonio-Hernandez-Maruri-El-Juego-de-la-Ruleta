<script setup lang="ts">
import { useGamblerStore } from '@/stores/gambler'

const gamblerStore = useGamblerStore()
</script>

<template>
  <div>
    <p><strong>Fondos actuales:</strong> ${{ gamblerStore.currentFunds }}</p>
  </div>
</template>
