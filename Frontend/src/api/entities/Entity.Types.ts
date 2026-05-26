export interface AuthorDto {
    id: number,
    firstName: string,
    lastName: string,
    middleName?: string,
    // fullName: string 
}

export interface GradeDto{
    id: number,
    displayName: string
}

export interface ReaderDto{
    id: number,
    firstName: string,
    lastName: string,
    middleName?: string,
    gradeName: string
}

export interface AuthorCreateDto {
    firstName: string,
    lastName: string,
    middleName?: string
}

export interface RoleDto{
    id: string,
    name: string
}

export interface BookDto{
    id: number,
    receiptDate: Date,
    title: string,
    description?: string,
    publishedYear?: number,
    price?: number,
    authors: AuthorDto[]
}

export interface CreateBookDto{
    title: string,
    description?: string,
    publishedYear?: number,
    price?: number,
    authorIds: number[]
}

export interface UserDto {
    id: string,
    username?: string,
    email?: string,
    role?: string
}

export interface CreateUserDto {
    firstName: string,
    lastName: string,
    role: string,
    password: string,
    middleName?: string,
    email?: string,
    username?: string
}
