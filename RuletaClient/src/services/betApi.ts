import type { Bet, BetResponse } from '@/types/bet'

export interface ApiResponse<T> {
  data?: T
  success: boolean
  message?: string
  status: number
}

const BASE_URL = '/api/Bet'

export class BetApiService {
  static async postBet(bet: Bet): Promise<ApiResponse<BetResponse>> {
    try {
      const response = await fetch(BASE_URL, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(bet),
      })

      if (response.ok) {
        const data = await response.json()

        return {
          data,
          success: true,
          message: 'Apuesta procesada correctamente',
          status: response.status,
        }
      } else {
        const errorMessage = await response.text()

        return {
          success: false,
          message: errorMessage || 'Error al procesar la apuesta',
          status: response.status,
        }
      }
    } catch (error) {
      console.error('Error posting bet:', error)

      return {
        success: false,
        message: 'Ocurrió un error de red al realizar la apuesta.',
        status: 0,
      }
    }
  }
}

export const betApi = {
  post: BetApiService.postBet,
}
