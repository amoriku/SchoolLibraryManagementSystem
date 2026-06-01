import axios, { type AxiosInstance } from "axios";

const BASE_API_URL: string = import.meta.env.VITE_API_URL;

let isRefreshing: boolean = false;
let failedQueue: any[] = [];

let refreshPromise: Promise<any> | null = null;

function setupRefreshInterceptor(axiosInstance: AxiosInstance) {
    axiosInstance.interceptors.response.use(
        (response) => response,
        async (error) => {
            const originalRequest = error.config;

            // 1. ЗАЩИТА: Если упал сам запрос рефреша, прерываемся.
            // Сессия мертва, либо это гость. Возвращаем ошибку в AuthProvider.
            if (originalRequest?.url?.includes("Auth/refresh")) {
                refreshPromise = null; // сбрасываем промис
                return Promise.reject(error);
            }

            // 2. Обрабатываем только 401 ошибку и только если этот запрос еще не повторялся
            if (error.response?.status === 401 && !originalRequest._retry) {
                originalRequest._retry = true;

                // Если рефреш ЕЩЕ НЕ идет, запускаем его
                if (!refreshPromise) {
                    refreshPromise = authApiJson.post("Auth/refresh")
                        .then((res) => {
                            refreshPromise = null; // Успешно сбросили после выполнения
                            return res;
                        })
                        .catch((err) => {
                            refreshPromise = null; // Сбросили при ошибке
                            return Promise.reject(err);
                        });
                }

                try {
                    // Все параллельные запросы, упавшие с 401, будут ЖДАТЬ один и тот же промис рефреша
                    await refreshPromise;

                    // Как только рефреш завершился (куки обновились), 
                    // просто повторяем исходный упавший запрос
                    return axiosInstance(originalRequest);
                } catch (refreshError) {
                    // Если рефреш сдох — прокидываем ошибку дальше в AuthProvider (включается режим гостя)
                    return Promise.reject(refreshError);
                }
            }

            return Promise.reject(error);
        }
    );
}


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

setupRefreshInterceptor(authApi);
setupRefreshInterceptor(authApiJson);