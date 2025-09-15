export enum EColor {
  Red = 0,
  Black = 1,
}

export enum ECondition {
  Even = 0,
  Odd = 1,
}

export interface Bet {
  bet: number
  chosenColor: EColor
  chosenNumber: number | null
  chosenCondition: ECondition | null
}

export interface BetResponse {
  gains: number
  generatedNumber: number
  generatedColor: EColor
  generatedCondition: ECondition
  gainsType: string
}
