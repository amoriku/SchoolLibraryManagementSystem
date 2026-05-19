import axios from "axios"

export interface AuthorDto {
    id: number,
    firstName: string,
    lastName: string,
    middleName?: string,
    // fullName: string 
}

export interface AuthorCreateDto {
    firstName: string,
    lastName: string,
    middleName?: string
}

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
    const options = {
        method: "GET",
        url: `${BASE_API_URL}/Authors`,
        headers: {
            accept: "*/*"
        }
    }

    try {
        const response = await axios.request(options);
        return response.data
    }
    catch (error) {
        console.log(error);
    }
}

export default GetAllAuthors;