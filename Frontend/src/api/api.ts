import axios from "axios";

const BASE_API_URL: string = import.meta.env.VITE_API_URL;

export const api = axios.create({
    baseURL: BASE_API_URL,
    headers: {
        Accept: "*/*"
    }
})

export const authApi = axios.create({
    baseURL: BASE_API_URL,
    headers: {
        Accept: "*/*"
    },
    withCredentials: true
})

export const authApiJson = axios.create({
    baseURL: BASE_API_URL,
    headers: {
        "Content-Type": "application/json",
        Accept: "*/*"
    },
    withCredentials: true
})
