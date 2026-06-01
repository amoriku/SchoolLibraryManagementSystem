export interface AuthorDto {
    id: number,
    firstName: string,
    lastName: string,
    middleName?: string 
}

export interface CreateBorrowingDto{
    ReservationId: number,
    DueDate?: Date
}

export interface BorrowingDto{
    id: number,
    libraryItem: string,
    borrowedDate: string,
    dueDate: string,
    returnDate: string
}

export interface ReserveCreateDto{
    libraryItemId: number,
}

export interface ReaderWithoutGradeDto{
    id: number,
    firstName: string,
    lastName: string,
    middleName?: string
}

export interface ReserveDto{
    reserveId: number,
    reader: ReaderWithoutGradeDto
    libraryItem: string,
    reservationDate: string
}

export interface GradeDto{
    id: number,
    displayName: string
}

export interface CreateGradeDto{
    number: number,
    letter: string
}

export interface ReaderDto{
    id: number,
    firstName: string,
    lastName: string,
    middleName?: string,
    gradeName: string,
    username?: string
}

export interface CreateReaderDto{
    firstName: string,
    lastName: string,
    middleName?: string,
    gradeId: number
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
    quantity: number,
    authors: AuthorDto[]
}

export interface CreateBookDto{
    title: string,
    description?: string,
    publishedYear?: number,
    price?: number,
    quantity: number,
    isbn?: string,
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
