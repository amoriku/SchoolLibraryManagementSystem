import { useEffect, useState } from "react";
import { type AuthorDto, type BookDto } from "../api/entities/Entity.Types";
import { MainSectionHeader } from "../components/MainSectionHeader"
import { Table } from "../components/Table"
import { FiBook } from "react-icons/fi";
import Modal from "../components/Modal";
import { useDataService } from "../api/services/dataService";
import { CreateBookForm } from "../components/forms/CreateBookForm";
import { RefreshButton } from "../components/buttons/RefreshButton";

export const BooksPage = () => {
    const { getAllAuthors, getAllBooks } = useDataService();

    const [bookCreateModalOpen, setBookCreateModalOpen] = useState<boolean>(false);

    const [books, setBooks] = useState<BookDto[]>([]);
    const [authors, setAuthors] = useState<AuthorDto[]>([]);

    const columnNames: string[] = [
        "Идентификатор",
        "Название",
        "Год издания",
        "Количество копий",
        "Автор"
    ]

    const fetchData = async () => {
        setBooks(await getAllBooks())
        setAuthors(await getAllAuthors())
    }

    useEffect(() => {
        fetchData();
    }, [])

    return (
        <>
            {bookCreateModalOpen && (
                <Modal
                    modalTitle="Создание книги"
                    formId="create-book-form"
                    onClose={() => setBookCreateModalOpen(false)}
                    isOpen={bookCreateModalOpen}
                >
                    <CreateBookForm></CreateBookForm>
                </Modal>
            )}

            <div className="flex flex-col gap-2">
                <MainSectionHeader title="Книги" desc="Управление библиотечным фондом">
                    <div className="flex items-center gap-4">
                        <button
                            className="main-section-header-button main-section-header-button-green"
                            onClick={() => setBookCreateModalOpen(true)}
                        >
                            <FiBook></FiBook>
                            <span>Создать</span>
                        </button>
                        <RefreshButton onRefresh={() => fetchData()}></RefreshButton>
                    </div>
                </MainSectionHeader>
                <Table columnNames={columnNames}>
                    {books.map(book => (
                        <tr
                            key={book.id}
                            className="table-tr"
                        >
                            <td
                                key={book.id}
                                className="table-td-id"
                            >
                                {book.id}
                            </td>
                            <td
                                key={book.title}
                                className="table-td"
                            >
                                {book.title}
                            </td>
                            <td
                                key={book.publishedYear}
                                className="table-td"
                            >
                                {book.publishedYear ? book.publishedYear : "-"}
                            </td>
                            <td
                                key={book.quantity}
                                className="table-td"
                            >
                                {book.quantity ? book.quantity : "-"}
                            </td>
                            {book.authors.map(author => (
                                <td
                                    key={author.id}
                                    className="table-td"
                                >
                                    
                                    {`${author.lastName} ${author.firstName[0]}. ${author.middleName ? author.middleName[0] : ""}.`}
                                </td>
                            ))}
                        </tr>
                    ))}
                </Table>
            </div>
        </>
    )
}