import { useEffect, useState } from "react"
import type { BookDto } from "../api/entities/Entity.Types"
import { useBookService } from "../api/services/bookService";
import { BookCard } from "../components/cards/BookCard";

export const CatalogPage = () => {
    const [books, setBooks] = useState<BookDto[]>([]);
    const { getAll } = useBookService();

    const fetchBooks = async () => {
        setBooks(await getAll())
    }

    useEffect(() => {
        fetchBooks();
    }, [])


    return (
        <div className="p-6">
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
                {books.map(book => (
                    <BookCard
                        key={book.id}
                        id={book.id}
                        title={book.title}
                        description={book.description}
                        publishedYear={book.publishedYear}
                        author={book.authors?.map(a => `${a.lastName} ${a.firstName[0]}. ${a.middleName ? a.middleName[0] : ""}.`).join(', ')}
                        quantity={book.quantity}
                    />
                ))}
            </div>
        </div>
    )
}