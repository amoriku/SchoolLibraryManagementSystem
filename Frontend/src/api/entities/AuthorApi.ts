import axios from "axios"
import { type AuthorCreateDto } from "./Entity.Types"
import { api } from "../api"


const BASE_API_URL = import.meta.env.VITE_API_URL

const CreateAuthor = async ({ firstName, lastName, middleName }: AuthorCreateDto) => {
    const options = {
        method: "POST",
        url: `${BASE_API_URL}/Authors/create`,
        headers: {
            'Content-Type': 'application/json',
            Accept: '*/*'
        },
        data: {
            firstName: firstName,
            lastName: lastName,
            middleName: middleName,
        }
    }

    try {
        const response = await axios.request(options);
        return response.data;
    }
    catch (error) {
        console.log(error);
    }
}

const GetAllAuthors = async () => {
    try {
        const response = await api.get("/Authors")
        return response.data
    }
    catch (error) {
        console.log(error);
    }
}

export default GetAllAuthors;