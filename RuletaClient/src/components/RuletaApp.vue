<script setup lang="ts">
import CurrentFunds from './CurrentFunds.vue'
import BetDisplay from './BetDisplay.vue'
import GamblerNameInput from './GamblerNameInput.vue'
import IdentifyButton from './IdentifyButton.vue'
import SaveFunds from './SaveFunds.vue'
import RuletaGame from './RuletaGame.vue'
import AddFunds from './AddFunds.vue'
import WithdrawFunds from './WithdrawFunds.vue'
</script>

<template>
  <CurrentFunds />
  <BetDisplay />
  <GamblerNameInput />
  <IdentifyButton />
  <hr />
  <AddFunds />
  <WithdrawFunds />
  <hr />
  <RuletaGame />
  <hr />
  <SaveFunds />
</template>
