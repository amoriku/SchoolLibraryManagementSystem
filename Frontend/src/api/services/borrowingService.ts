import { useData } from "../../hooks/useData"
import type { CreateBorrowingDto, BorrowingDto } from "../entities/Entity.Types"

export const useBorrowingService = () => {
    const { request } = useData()

    return {
        create: (data: CreateBorrowingDto): Promise<BorrowingDto> => request({
            type: "Auth",
            method: "post",
            endpoint: "/Borrowings/create",
            data: data
        })
    }
}