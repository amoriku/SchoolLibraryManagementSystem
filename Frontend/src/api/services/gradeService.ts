import { useData } from "../../hooks/useData"
import type { CreateGradeDto, GradeDto } from "../entities/Entity.Types";

export const useGradeService = () => {
    const { request } = useData();

    return {
        create: (data: CreateGradeDto): Promise<GradeDto> => request<GradeDto>({
            type: "Auth",
            method: "post",
            endpoint: "/Grades/create",
            data: data
        }) 
    }
}