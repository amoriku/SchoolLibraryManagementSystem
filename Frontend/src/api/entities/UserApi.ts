import toast from "react-hot-toast";
import { authApi, authApiJson } from "../api";
import type { CreateUserDto, UserDto } from "./Entity.Types";


export const GetAllUsers = async (): Promise<UserDto[]> => {
    const promise =  authApi.get<UserDto[]>("/Users/get-all");

    try {
        const response = await promise;

        toast.promise(promise, {
            loading: "Получение пользователей",
            success: "Пользователи успешно получены",
        })
        return response.data;
    }
    catch (error) {
        console.error(`Ошибка при получении пользователей: \n${error}`);
        return [];
    }
}

export const RemoveUser = async (userId: string): Promise<void> => {
    try {
        await authApi.delete(
            "/Users/remove",
            {
                params: {
                    userId: userId
                }
            }
        )

        toast.success("Пользователь успешно удален");
    }
    catch (error){
        console.error("Ошибка при удалении пользователя: \n", error)
    }
}

export const CreateUser = async (userData: CreateUserDto): Promise<void> => {
    try {
        await authApiJson.post(
            "/Users/create",
            {
                firstName: userData.firstName,
                lastName: userData.lastName,
                username: userData.username,
                password: userData.password,
                email: userData.email,
                middleName: userData.middleName,
                role: userData.role
            }
        )

        toast.success("Пользователь успешно добавлен");
    }
    catch (error) {
        console.error("Не удалось создать пользователя: \n", error);
    }
}