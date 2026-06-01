import { useData } from "../../hooks/useData"
import type { ReserveDto, ReserveCreateDto } from "../entities/Entity.Types";

export const useReserveService = () => {
    const { request } = useData();

    return {
        reserve: (data: ReserveCreateDto): Promise<void> => request({
            type: "Auth",
            method: "post",
            endpoint: "Reservations/reserve",
            data: data
        }),
    }
}