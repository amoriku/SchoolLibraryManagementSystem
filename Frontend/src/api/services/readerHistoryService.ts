import { useData } from "../../hooks/useData"
import type { ReaderHistoryDto } from "../entities/Entity.Types";

export const useReaderHistoryService = () => {
    const { request } = useData();

    return {
        getHistory: (readerId: string): Promise<ReaderHistoryDto[]> => request({
            type: "Auth",
            method: "get",
            endpoint: "Readers/history",
            config: {
                params: {
                    readerId: readerId
                }
            }
        })
    }
}