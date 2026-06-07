import { useData } from "../../hooks/useData"
import type { ReserveDto, ReserveCreateDto, ReserveWithoutUserDto } from "../entities/Entity.Types";

export const useReserveService = () => {
    const { request } = useData();

    return {
        reserve: (data: ReserveCreateDto): Promise<ReserveDto> => request({
            type: "Auth",
            method: "post",
            endpoint: "Reservations/reserve",
            data: data
        }),
        active: (): Promise<ReserveWithoutUserDto[]> => request({
            type: "Auth",
            method: "get",
            endpoint: "Reservations/active"
        }),
        cancel: (reserveId: number): Promise<ReserveWithoutUserDto> => request({
            type: "Auth",
            method: "post",
            endpoint: "Reservations/cancel",
            config: {
                params: {
                    reserveId: reserveId
                }
            }
        })
    }
}