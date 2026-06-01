import { useData } from "../../hooks/useData"
import type { BookDto, CreateBookDto } from "../entities/Entity.Types";

export const useBookService = () => {
    const { request } = useData();

    return {
        getAll: () => request<BookDto[]>({ type: "Public", method: "get", endpoint: "/Books" }),
        create: (data: CreateBookDto) => request<BookDto>(
            {
                type: "Auth",
                method: "post",
                endpoint: "/Books/create",
                data: {
                    ...data,
                    authorIds: [Number(data.authorIds)]
                },
                config: { headers: { "Content-Type": "application/json" } }
            }
        ),
        getByTitle: (title: string): Promise<BookDto> => request<BookDto>({
            type: "Public",
            method: "get",
            endpoint: "Books/get-by-title",
            config: {
                params: {
                    title: title
                }
            }
        })
    }
}