import axios from "axios";

export interface Book{
    id: number,
    receiptDate: Date,
    title: string,
    // description?: string,
    publishedYear?: number,
    price?: number
}

const BASE_API_URL: string = import.meta.env.VITE_API_URL;

export const GetBookCoverByTitle = async (title: string) => {
    const encodedTitle: string = encodeURIComponent(title);
    const url: string = "https://www.googleapis.com/books"

    const options = {
        method: "GET",
        url: url
    }
}

export const GetAllBooks = async (title?: string) =>{
    const options = {
        method: "GET",
        url: `${BASE_API_URL}/Books`,
        params: {
            Search: title
        },
        headers:{
            accept: "*/*"
        }
    }

    try{
        const response = await axios.request(options)
        // console.log(response.data);
        return response.data;
    }
    catch (error){
        console.error(error)
    }
}