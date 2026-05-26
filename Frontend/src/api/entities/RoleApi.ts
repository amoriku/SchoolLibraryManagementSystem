import { api } from "../api"
import type { RoleDto } from "./Entity.Types";



export const GetAllRoles = async (): Promise<RoleDto[]> => {
    try{
        const response = await api.get<RoleDto[]>("Roles/get-all");
        return response.data;
    }
    catch (error){
        console.error("Ошибка при получении всех ролей: \n", error);
        return [];
    }
}