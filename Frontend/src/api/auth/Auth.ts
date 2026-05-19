import axios from "axios";
import toast from "react-hot-toast";

const BASE_API_URL: string = import.meta.env.VITE_API_URL;

export interface User {
    id?: string,
    username?: string,
    roles?: [string]
}
export interface LoginResponse {
    accessToken: string,
    refreshToken: string,
}


// const cancelToken = axios.CancelToken;
// const source = cancelToken.source();

export const GetCurrentUser = async () => {
    const options = {
        method: "GET",
        url: `${BASE_API_URL}/Auth/current`,
        withCredentials: true,
        headers: {
            accept: "*/*"
        }
    }

    try {
        const response = await axios.request(options);
        return response.data;
    }
    catch (error) {
        console.error(error);
    }
}

export const Login = async (identifier: string, password: string): Promise<LoginResponse> => {
    const options = {
        method: "POST",
        url: `${BASE_API_URL}/Auth/login`,
        data: {
            identifier: identifier,
            password: password
        },
        headers: {
            accept: '*/*'
        }
    }

    const loginPromise = axios.request<LoginResponse>(options);

    toast.promise(loginPromise, {
        loading: "Проверка данных...",
        success: "Вы успешно вошли!",
        error: (err) => {
            const status = err.response?.status;
            switch (status) {
                case 415:
                    return "Ошибка формата данных 415"
                case 400:
                case 401:
                    return "Неверный пароль или логин"
                default:
                    return "Что-то пошло не так"
            }
        }
    })

    try {
        const response = await loginPromise;
        return response.data; // Возвращает accessToken и refreshToken
    }
    catch (error) {
        console.error(error);
        throw error; 
    }
}