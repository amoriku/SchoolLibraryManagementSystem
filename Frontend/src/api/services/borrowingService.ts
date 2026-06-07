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
        }),
        getActive: (): Promise<BorrowingDto[]> => request({
            type: "Auth",
            method: "get",
            endpoint: "/Borrowings/active"
        }),
        returnBook: (itemCopyId: number): Promise<boolean> => request({
            type: "Auth",
            method: "post",
            endpoint: "/Borrowings/return",
            config: {
                params: {
                    itemCopyId: itemCopyId
                }
            }
        })
    }
}