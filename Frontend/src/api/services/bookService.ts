import { SiParamountplus } from "react-icons/si";
import { useData } from "../../hooks/useData"
import type { BookDto, BookHistoryDto, CreateBookDto } from "../entities/Entity.Types";

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
        }),
        getHistory: (bookId: number): Promise<BookHistoryDto[]> => request<BookHistoryDto[]>({
            type: "Auth",
            method: "get",
            endpoint: "Books/history",
            config: {
                params: {
                    bookId: bookId
                }
            }
        }),
        getBorrowingHistories: (): Promise<BookHistoryDto[]> => request<BookHistoryDto[]>({
            type: "Auth",
            method: "get",
            endpoint: "Books/borrowing-histories",
        }),
        getReturnHistories: (): Promise<BookHistoryDto[]> => request<BookHistoryDto[]>({
            type: "Auth",
            method: "get",
            endpoint: "Books/return-histories"
        }),
        getNearestAvailabilityDate: (bookId: number): Promise<string> => request<string>({
            type: "Public",
            method: "get",
            endpoint: "Books/nearest-availability-date",
            config: {
                params: {
                    bookId: bookId
                }
            }
        })
    }
}