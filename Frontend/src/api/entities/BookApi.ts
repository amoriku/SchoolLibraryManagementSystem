import { api } from "../api";
import type { BookDto } from "./Entity.Types";


export const GetBookCoverByTitle = async (title: string) => {
    const encodedTitle: string = encodeURIComponent(title);
    const url: string = "https://www.googleapis.com/books"

    const options = {
        method: "GET",
        url: url
    }
}

export const GetAllBooks = async (title?: string): Promise<BookDto[]> =>{
    try{
        const response = await api.get<BookDto[]>(
            "/Books",
            {
                params: {
                    Search: title
                }
            }
        )
        console.log(response.data);
        return response.data;
    }
    catch (error){
        console.error(error)
        return [];
    }
}