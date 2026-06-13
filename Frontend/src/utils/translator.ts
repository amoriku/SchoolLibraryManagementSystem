export const translator = (word: string): string => {
    const translations: Record<string, string> = {
        "Borrowing": "Выдача",
        "Return": "Возврат",
        "ReserveExpired": "Бронь аннулирована",

        "Librarian": "Библиотекарь",
        "Reader": "Читатель",
        "Admin": "Администратор"
    } 


    return translations[word] ? translations[word] : word
}