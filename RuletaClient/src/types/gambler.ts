export interface Gambler {
  id: string
  name: string
  funds: number
}

export interface FundsTransaction {
  amount: number
  type: 'add' | 'withdraw'
  timestamp: Date
}
