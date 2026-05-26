import { useData } from "../../hooks/useData"
import type { AuthorDto, BookDto, RoleDto, UserDto, GradeDto, ReaderDto } from "../entities/Entity.Types";

export const useDataService = () => {
    const { request } = useData();

    return {
       getAllUsers: () => request<UserDto[]>({type: "Auth", method: "get", endpoint: "/Users"}),
       getAllAuthors: () => request<AuthorDto[]>({type: "Auth", method: "get", endpoint: "/Authors"}),
       getAllRoles: () => request<RoleDto[]>({type: "Auth", method: "get", endpoint: "/Roles"}),
       getAllBooks: () => request<BookDto[]>({type: "Auth", method: "get", endpoint: "/Books"}),
       getAllGrades: () => request<GradeDto[]>({type: "Auth", method: "get", endpoint: "/Grades"}), 
       getAllReaders: () => request<ReaderDto[]>({type: "Auth", method: "get", endpoint: "/Readers"}) 
    }
}