import type { Book } from "../../api/entities/BookApi"

interface BookProps{
    BookData: Book;
}

export const BookCard = ({BookData}: BookProps) => {
    return (
        <div className="text-white border p-3 rounded mt-4">
            <ul className="list-none">
                <img 
                    className=""
                    width={320}
                    height={320}
                    alt="Обложка книги" />
                <li>
                   Название: {BookData.title} 
                </li>
                <li>
                   Дата публикации: {BookData.publishedYear} 
                </li>
                <li>
                   Статус: "Укажите статус" 
                </li>
            </ul>
        </div>
    )
}