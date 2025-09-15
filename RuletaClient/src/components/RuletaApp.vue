<script setup lang="ts">
import CurrentFunds from './CurrentFunds.vue'

import GamblerNameInput from './GamblerNameInput.vue'
import IdentifyButton from './IdentifyButton.vue'
import SaveFunds from './SaveFunds.vue'
import RuletaGame from './RuletaGame.vue'
import AddFunds from './AddFunds.vue'
import WithdrawFunds from './WithdrawFunds.vue'
</script>

<template>
  <div class="ruleta-app-container">
    <div class="funds-bet-row">
      <CurrentFunds />
    </div>
    <GamblerNameInput />
    <IdentifyButton />
    <hr />
    <div class="funds-management-row">
      <AddFunds />
      <WithdrawFunds />
    </div>
    <RuletaGame />
    <SaveFunds />
  </div>
</template>

<style scoped>
.ruleta-app-container {
  display: flex;
  flex-direction: column;
  gap: 20px; /* Spacing between components */
  padding: 20px;
  background-color: #fff; /* White background for the app container */
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1); /* Subtle shadow */
  max-width: 600px;
  width: 100%;
  align-items: center;
}

hr {
  border: none;
  border-top: 1px solid #eee; /* Light gray separator */
  width: 100%;
  margin: 10px 0;
}

.funds-bet-row {
  display: flex;
  flex-direction: row;
  gap: 20px; /* Spacing between CurrentFunds and BetDisplay */
  justify-content: center;
  width: 100%;
  flex-wrap: wrap; /* Allow wrapping on smaller screens */
}

  .funds-management-row {
    display: flex;
    flex-direction: row;
    gap: 20px; /* Spacing between AddFunds and WithdrawFunds */
    justify-content: center;
    width: 100%;
  }</style>
