import { useData } from "../../hooks/useData"
import type { CreateUserDto, UserDto } from "../entities/Entity.Types";

export const useUserService = () => {
    const { request } = useData();

    return {
        create: (data: CreateUserDto): Promise<UserDto> => request({
            type: "Auth",
            method: "post",
            endpoint: "Users/create",
            data: data
        }),
        remove: (userId: string): Promise<void> => request({
            type: "Auth",
            method: "delete",
            endpoint: "Users/remove",
            config: {
                params: {
                    userId: userId
                }
            }
        })
    }
}