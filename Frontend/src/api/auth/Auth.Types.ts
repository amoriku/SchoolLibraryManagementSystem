export type UserRole = "Admin" | "Librarian" | "Guest" | "Reader";

export interface User {
    id?: string,
    username?: string,
    email?: string,
    role?: UserRole 
}

export interface LoginResponse{
    accessToken: string,
    refreshToken: string
}