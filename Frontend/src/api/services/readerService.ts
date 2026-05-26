import { useData } from "../../hooks/useData"
import type { ReaderDto } from "../entities/Entity.Types"

export const useReaderService = () => {
    const { request } = useData();

    return {
        getAllReaders: () => request<ReaderDto[]>(
            {
                type: "Auth",
                method: "get",
                endpoint: "/Readers"
            }
        )
    }
}