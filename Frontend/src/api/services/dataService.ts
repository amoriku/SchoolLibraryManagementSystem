import { useData } from "../../hooks/useData"
import type { AuthorDto, BookDto, RoleDto, UserDto, GradeDto, ReaderDto, ReserveDto, BorrowingDto } from "../entities/Entity.Types";

export const useDataService = () => {
    const { request } = useData();

    return {
       getAllUsers: (): Promise<UserDto[]> => request<UserDto[]>({type: "Auth", method: "get", endpoint: "/Users"}),
       getAllAuthors: (): Promise<AuthorDto[]> => request<AuthorDto[]>({type: "Auth", method: "get", endpoint: "/Authors"}),
       getAllRoles: (): Promise<RoleDto[]> => request<RoleDto[]>({type: "Auth", method: "get", endpoint: "/Roles"}),
       getAllBooks: (): Promise<BookDto[]> => request<BookDto[]>({type: "Public", method: "get", endpoint: "/Books"}),
       getAllGrades: (): Promise<GradeDto[]> => request<GradeDto[]>({type: "Auth", method: "get", endpoint: "/Grades"}), 
       getAllReaders: (): Promise<ReaderDto[]> => request<ReaderDto[]>({type: "Auth", method: "get", endpoint: "/Readers"}), 
       getAllReservations: (): Promise<ReserveDto[]> => request<ReserveDto[]>({type: "Auth", method: "get", endpoint: "/Reservations"}), 
       getAllBorrowings: (): Promise<BorrowingDto[]> => request<BorrowingDto[]>({type: "Auth", method: "get", endpoint: "/Borrowings"}), 
    }
}