import React, { useEffect, useState } from "react";
import { type AuthorDto, type BookDto, type BookHistoryDto } from "../api/entities/Entity.Types";
import { MainSectionHeader } from "../components/MainSectionHeader"
import { Table } from "../components/Table"
import { FiBook } from "react-icons/fi";
import Modal from "../components/Modal";
import { useDataService } from "../api/services/dataService";
import { CreateBookForm } from "../components/forms/CreateBookForm";
import { RefreshButton } from "../components/buttons/RefreshButton";
import { TableBookHistoryButton } from "../components/buttons/TableBookHistoryButton";
import { ActionsContainer } from "../components/ActionsContainer";
import { useBookService } from "../api/services/bookService";
import { MdHdrEnhancedSelect } from "react-icons/md";

export const BooksPage = () => {
    const { getAllAuthors, getAllBooks } = useDataService();
    const { getHistory } = useBookService();

    const [bookHistory, setBookHistory] = useState<BookHistoryDto[]>([]);
    const [bookId, setBookId] = useState<number | null>(null);
    const [isBookFormularOpen, setIsBookFormularOpen] = useState<boolean>(false);

    const [filterResult, setFilterResult] = useState<string>("");
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

    const handleClick = async (bookId: number) => {
        setBookId(bookId);
        setIsBookFormularOpen(true);

        if (bookId === null) return;

        const history = await getHistory(bookId);
        console.log(history);
    }

    const handleBookFormularOpen = async () => {
        if (bookId === null) return;

        try {
            const history = await getHistory(bookId);
            console.log(history);
        }
        catch (ex) {
            console.error(ex);
        }
    }

    const handleFilter = (e: React.ChangeEvent<HTMLInputElement>) => {
        setFilterResult(e.currentTarget.value);
    }

    const fetchData = async () => {
        setBooks(await getAllBooks())
        setAuthors(await getAllAuthors())
    }

    useEffect(() => {
        fetchData();
    }, [])

    return (
        <>
            {isBookFormularOpen && (
                <Modal
                    modalTitle="Формуляр книги"
                    onClose={() => setIsBookFormularOpen(false)}
                    isOpen={isBookFormularOpen}
                >

                </Modal>
            )}

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
                <div className="flex">
                    <MainSectionHeader
                        title="Книги"
                        desc="Управление библиотечным фондом"
                        includeFilter={true}
                        onFilter={handleFilter}
                        filterText="Название, год, автор"
                    >
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
                </div>
                <Table columnNames={columnNames}>
                    {
                        books
                            .filter(book => {
                                if (!filterResult) return books;
                                const query: string = filterResult.toLowerCase().trim();

                                const matchTitle: boolean = book.title.toLowerCase().trim().includes(query);
                                const matchYear: boolean = String(book.publishedYear).toLowerCase().trim().includes(query);

                                const matchAuthor: boolean = book.authors?.some(author => {
                                    const fullName = `${author.lastName} ${author.firstName} ${author.middleName || ''}`.toLowerCase().trim()
                                    return fullName.includes(query);
                                })

                                return matchTitle || matchYear || matchAuthor
                            })
                            .map(book => (
                                <tr
                                    key={book.id}
                                    className="table-tr"
                                >
                                    <td
                                        key={book.id}
                                        className="table-td"
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
                                        {book.quantity ? book.quantity : "0"}
                                    </td>
                                    {book.authors && book.authors.length > 0 ? (
                                        book.authors?.map(author => (
                                            <td
                                                key={author.id}
                                                className="table-td"
                                            >
                                                {`${author.lastName} ${author.firstName[0]}. ${author.middleName ? author.middleName[0] : ""}.`};
                                            </td>)
                                        )
                                    ) :
                                        <td className="table-td">Автор не указан</td>
                                    }
                                    <ActionsContainer>
                                        <TableBookHistoryButton onClick={() => handleClick(book.id)}></TableBookHistoryButton>
                                    </ActionsContainer>
                                </tr>
                            ))}
                </Table>
            </div>
        </>
    )
}