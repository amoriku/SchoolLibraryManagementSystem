import { useData } from "../../hooks/useData"
import type { CreateReaderDto, ReaderDto } from "../entities/Entity.Types"

export const useReaderService = () => {
    const { request } = useData();

    return {
        create: (data: CreateReaderDto): Promise<ReaderDto> => request({
            type: "Auth",
            method: "post",
            endpoint: "Readers/create",
            data: data
        })
    }
}