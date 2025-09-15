import type { GamblerResponse, RegisterGamblerRequest, UpdateFundsRequest } from '@/types/gambler'
import { ETransaction } from '@/types/gambler'

export interface ApiResponse<T> {
  data?: T
  success: boolean
  message?: string
  status: number
}

const BASE_URL = '/api/Gambler'

export class GamblerApiService {
  static async getGambler(name: string): Promise<ApiResponse<GamblerResponse>> {
    try {
      const response = await fetch(`${BASE_URL}/${encodeURIComponent(name)}`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
        },
      })

      if (response.ok) {
        const data = await response.json()

        return {
          data,
          success: true,
          message: 'Apostador recuperado exitosamente',
          status: response.status,
        }
      } else {
        const errorMessage = await response.text()

        return {
          success: false,
          message: errorMessage || 'Apostador no encontrado',
          status: response.status,
        }
      }
    } catch (error) {
      console.error('Error getting gambler:', error)

      return {
        success: false,
        message: 'Hay un problema de conexión',
        status: 0,
      }
    }
  }

  static async createGambler(
    request: RegisterGamblerRequest,
  ): Promise<ApiResponse<GamblerResponse>> {
    try {
      const response = await fetch(BASE_URL, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(request),
      })

      if (response.ok) {
        const data = await response.json()

        return {
          data,
          success: true,
          message: 'Apostador creado exitosamente',
          status: response.status,
        }
      } else if (response.status === 400) {
        const errorMessage = await response.text()

        return {
          success: false,
          message: errorMessage || 'Datos de solicitud no válidos',
          status: response.status,
        }
      } else {
        return {
          success: false,
          message: 'Error al crear el apostador',
          status: response.status,
        }
      }
    } catch (error) {
      console.error('Error creating gambler:', error)

      return {
        success: false,
        message: 'Ocurrió un error de red',
        status: 0,
      }
    }
  }

  static async updateGamblerFunds(
    name: string,
    request: UpdateFundsRequest,
  ): Promise<ApiResponse<GamblerResponse>> {
    try {
      const response = await fetch(`${BASE_URL}/${encodeURIComponent(name)}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(request),
      })

      if (response.ok) {
        const data = await response.json()
        const transactionType =
          request.transaction === ETransaction.Add ? 'agregados a' : 'retirados del1'

        return {
          data,
          success: true,
          message: `Fondos ${transactionType} apostador exitosamente`,
          status: response.status,
        }
      } else if (response.status === 404) {
        const errorMessage = await response.text()

        return {
          success: false,
          message: errorMessage || 'Apostador no encontrado',
          status: response.status,
        }
      } else if (response.status === 400) {
        const errorMessage = await response.text()

        return {
          success: false,
          message: errorMessage || 'Tipo de transacción no válida o fondos insuficientes',
          status: response.status,
        }
      } else {
        return {
          success: false,
          message: 'Error al actualizar los fondos del apostador',
          status: response.status,
        }
      }
    } catch (error) {
      console.error('Error updating gambler funds:', error)

      return {
        success: false,
        message: 'Ocurrió un error de red',
        status: 0,
      }
    }
  }

  static async gamblerExists(name: string): Promise<boolean> {
    const response = await this.getGambler(name)

    return response.success && response.data !== null
  }
}

export const gamblerApi = {
  get: GamblerApiService.getGambler,
  create: GamblerApiService.createGambler,
  updateFunds: GamblerApiService.updateGamblerFunds,
  exists: GamblerApiService.gamblerExists,
}

export { ETransaction }
