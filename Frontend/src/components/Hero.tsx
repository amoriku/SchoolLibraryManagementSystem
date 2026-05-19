import { useState } from "react";
import { GetAllBooks, type Book } from "../api/entities/BookApi"
import { SearchInput } from "./CustomInput"
import { BookCard } from "./book/BookCard";

export default function Hero() {
    const [books, setBooks] = useState<Book[]>([]);

    const GetBooks = async (title?: string) => {
        setBooks(await GetAllBooks(title));
        // console.log(books);
    }

    return (
        <div className="bg-slate-800 h-screen">
            <div className="flex flex-col justify-center items-center">
                <div className="flex justify-center pt-4">
                    <SearchInput
                        placeholder="Введите название книги..."
                        onSearch={GetBooks}
                    >
                    </SearchInput>

                </div>
                <div>
                    {books.map((book) => (
                        <BookCard BookData={book}></BookCard>
                    ))}
                </div>
            </div>

        </div>
    )
}