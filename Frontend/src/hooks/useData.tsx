import { createContext, useContext } from "react";
import { api, authApi } from "../api/api";
import type { AxiosRequestConfig } from "axios";

type RequestType = "Auth" | "Public"
type RequestMethod = "get" | "post" | "delete" | "put"

export interface RequestProps {
    endpoint: string,
    method: RequestMethod,
    type?: RequestType,
    data?: any,
    config?: AxiosRequestConfig
}

interface DataContextProps {
    request: <T = any>(props: RequestProps) => Promise<T>;
}

const DataContext = createContext<DataContextProps | null>(null);

export const DataProvider = ({ children }: { children: React.ReactNode }) => {
    const request = async <T = any,>({ endpoint, type = "Public", method, data, config }: RequestProps): Promise<T> => {
        const apiInstance = type === "Public" ? api : authApi;

        try {
            const response = await apiInstance.request<T>({
                url: endpoint,
                method: method,
                data: data,
                ...config
            })

            return response.data;
        }
        catch (error) {
            console.error("Ошибка вызова API: \n", error);
            throw error;
        }
    }

    return (
        <DataContext.Provider value={{request}}>
            {children}
        </DataContext.Provider>
    )
}

export const useData = () => {
    const context = useContext(DataContext)
    if (!context){
        throw new Error("useData должен использоваться внутри DataProvider")
    }
    return context;
}