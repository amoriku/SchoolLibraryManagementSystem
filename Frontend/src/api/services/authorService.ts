import { useData } from "../../hooks/useData";
import type { AuthorCreateDto, AuthorDto } from "../entities/Entity.Types";

export const useAuthorService = () => {
    const { request } = useData();

    return {
        create: (data: AuthorCreateDto) => request<AuthorDto>(
            {
                type: "Auth",
                method: "post",
                endpoint: "/Authors/create",
                data: data,
                config: { headers: { 'Content-Type': 'application/json' } }
            })
    }
}