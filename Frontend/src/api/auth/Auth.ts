import axios from "axios";
import toast from "react-hot-toast";
import type { LoginResponse, UserRole } from "./Auth.Types";
import { authApi } from "../api";

const BASE_API_URL: string = import.meta.env.VITE_API_URL;

// const cancelToken = axios.CancelToken;
// const source = cancelToken.source();

export const Logout = async () => {
    const logoutPromise = authApi.delete("/Auth/logout")

    toast.promise(logoutPromise, {
        loading: "Ожидание выхода...",
        success: "Вы вышли из аккаунта",
        error: "Что-то пошло не так"
    })

    try {
        const response = await logoutPromise;
        if (response.status === 200) {
            console.log("Successfully logout");
        }
    } catch (error) {
        console.error(error)
    }
}

export const getCurrentUser = async () => {
    try {
        const response = await authApi.get("Auth/current", {
            _skipRefresh: true
        } as any);
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
        withCredentials: true,
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
                    return "Ошибка формата данных"
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