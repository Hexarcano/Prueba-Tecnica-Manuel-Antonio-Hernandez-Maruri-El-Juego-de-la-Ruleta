export interface Gambler {
  name: string
  funds: number
}

export interface RegisterGamblerRequest {
  name: string
  funds: number | null
}

export interface UpdateFundsRequest {
  funds: number
  transaction: ETransaction
}

export enum ETransaction {
  Add = 0,
  Withdraw = 1,
}

export interface GamblerResponse {
  name: string
  funds: number | null
}
