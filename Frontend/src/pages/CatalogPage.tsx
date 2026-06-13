import { useEffect, useState } from "react"
import type { BookDto } from "../api/entities/Entity.Types"
import { useBookService } from "../api/services/bookService";
import { BookCard } from "../components/cards/BookCard";

export const CatalogPage = ({filterResult}: {filterResult?: string}) => {
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
                {
                    books.filter(book => {
                        if (!filterResult) return books;

                        const query: string = filterResult.toLowerCase().trim();

                        const matchTitle: boolean = book.title.toLowerCase().trim().includes(query);
                        const matchAuthor: boolean = book.authors?.some(author => {
                            const fullName = `${author.lastName} ${author.firstName} ${author.middleName || ""}`;
                            return fullName.toLowerCase().trim().includes(query);
                        })
                        
                        return matchTitle || matchAuthor
                    })
                    .map(book => (
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