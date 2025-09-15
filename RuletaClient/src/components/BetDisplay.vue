<script setup lang="ts">
import { useBetStore } from '@/stores/bet'

const betStore = useBetStore()
</script>

<template>
  <div><strong>Apuesta actual:</strong> ${{ betStore.bet.amount ?? 'N/A' }}</div>
</template>
