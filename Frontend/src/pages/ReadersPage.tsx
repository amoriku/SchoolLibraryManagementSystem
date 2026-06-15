import { MainSectionHeader } from "../components/MainSectionHeader"
import { Table } from "../components/Table"
import { useEffect, useState } from "react"
import type { BookDto, CreateBorrowingDto, ReaderDto, ReaderHistoryDto } from "../api/entities/Entity.Types"
import { useDataService } from "../api/services/dataService"
import { RefreshButton } from "../components/buttons/RefreshButton"
import { CreateButton } from "../components/buttons/CreateButton"
import { CgAdd } from "react-icons/cg"
import Modal from "../components/Modal"
import { CreateReaderForm } from "../components/forms/CreateReaderForm"
import { TableBorrowButton } from "../components/buttons/TableBorrowButton"
import { ConfirmModal } from "../components/ConfirmationModal"
import { useBorrowingService } from "../api/services/borrowingService"
import toast from "react-hot-toast"
import { BaseInput } from "../components/CustomInput"
import { validator } from "../utils/validator"
import { TableReaderHistoryButton } from "../components/buttons/TableReaderHistoryButton"
import { useReaderHistoryService } from "../api/services/readerHistoryService"
import { translator } from "../utils/translator"

export const ReadersPage = () => {
    const [createModalOpen, setCreateModalOpen] = useState<boolean>(false);
    const [isReaderHistoryOpen, setIsReaderHistoryOpen] = useState<boolean>(false);
    const [isBorrowDirectOpen, setIsBorrowDirectOpen] = useState<boolean>(false);
    const [isBorrowConfirmOpen, setIsBorrowConfirmOpen] = useState<boolean>(false);
    const [books, setBooks] = useState<BookDto[]>([]);

    const [readerId, setReaderId] = useState<string>("");
    const [readerFullName, setReaderFullName] = useState<string>("")
    const [bookId, setBookId] = useState<number | null>(null);
    const [dueDate, setDueDate] = useState<string>("");
    const [isDueDateCorrect, setIsDueDateCorrect] = useState<boolean>(false);

    const columnNames = [
        "Фамилия",
        "Имя",
        "Отчество",
        "Класс",
        "Никнейм"
    ]

    const [filterResult, setFilterResult] = useState<string>("");
    const [readers, setReaders] = useState<ReaderDto[]>([]);
    const [readerHistory, setReaderHistory] = useState<ReaderHistoryDto[]>([])
    const { getAllReaders, getAllBooks } = useDataService();
    const { create } = useBorrowingService();
    const { getHistory } = useReaderHistoryService()
    const { validateDate } = validator();

    const handleCreateReader = () => {
        setCreateModalOpen(false);
        fetchData();
    }

    const handleBorrowDirectConfirm = () => {

    }

    const handleBorrow = (readerId: string, readerFullName: string) => {
        setReaderFullName(readerFullName);
        setReaderId(readerId);
        setIsBorrowDirectOpen(true);
    }

    const handleBorrowSubmit = async (e: React.SubmitEvent) => {
        e.preventDefault();
        if (!readerId || bookId == null || isDueDateCorrect) return;

        try {
            const data: CreateBorrowingDto = {
                readerId: readerId,
                bookId: bookId,
                dueDate: dueDate
            }

            await create(data);
            toast.success(`Книга успешно выдана`)
        }
        catch (error) {
            toast.error("Что-то пошло не так")
        }
    }

    const handleDueDateChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setIsDueDateCorrect(validateDate(e.currentTarget.value));
    }

    const handleReaderFilter = (e: React.ChangeEvent<HTMLInputElement>) => {
        setFilterResult(e.currentTarget.value);
    }

    const fetchData = async () => {
        setReaders(await getAllReaders());
        setBooks(await getAllBooks());
    }

    const handleReaderHistoryOpen = async () => {
        setReaderId(readerId);
        setReaderHistory([]);
        setIsReaderHistoryOpen(true);

        try {
            setReaderHistory(await getHistory(readerId))
        }
        catch (ex) {
            setReaderHistory([]);
            setIsReaderHistoryOpen(false);
        }
    }

    useEffect(() => {
        fetchData();
    }, [])

    return (
        <>
            <Modal
                modalTitle="Читательский формуляр"
                isOpen={isReaderHistoryOpen}
                onClose={() => setIsReaderHistoryOpen(false)}                
                includeOperations={false}
            >
                <Table
                    columnNames={["Дата", "Книга", "Действие"]}
                    includeOperations={false}
                    isModalVersion={true}
                >

                    {readerHistory.map(record => (
                        <tr
                            className="table-tr"
                            key={record.readerId}
                        >
                            <td
                                key={record.date}
                                className="table-td"
                            >
                                {new Date(record.date).toLocaleString("ru-RU")}
                            </td>
                            <td
                                key={record.libraryItemTitle}
                                className="table-td"
                            >
                                {record.libraryItemTitle}
                            </td>
                            <td
                                key={record.operationType}
                                className="table-td"
                            >
                                {translator(record.operationType)}
                            </td>
                        </tr>
                    ))}

                </Table>
            </Modal>

            <ConfirmModal
                isOpen={isBorrowConfirmOpen}
                confirmText="Выдать книгу"
                message={`Выдача книги читателю ${readerFullName}`}
                onCancel={() => setIsBorrowConfirmOpen(false)}
                onConfirm={handleBorrowDirectConfirm}
            >

            </ConfirmModal>

            <Modal
                modalTitle="Выдача книги"
                formId="borrow-direct-form"
                isOpen={isBorrowDirectOpen}
                onClose={() => setIsBorrowDirectOpen(false)}
            >
                <form
                    id="borrow-direct-form"
                    onSubmit={handleBorrowSubmit}
                    className="create-form"
                >
                    <label htmlFor="dueDate">Выберите дату возврата</label>
                    <BaseInput
                        type="date"
                        id="dueDate"
                        placeholder="Выберите дату возврата..."
                        value={dueDate}
                        error={!isDueDateCorrect}
                        onChange={(e) => handleDueDateChange(e)}
                    >

                    </BaseInput>
                    <select
                        className="select-input"
                        onChange={(e) => setBookId(Number(e.currentTarget.value))}
                    >
                        <option value="0">Выберите книгу...</option>
                        {books
                            .filter(book => book.quantity > 0)
                            .map(book => (
                                <option key={book.title} value={book.id}>{book.title}</option>
                            ))
                        }
                    </select>
                </form>
            </Modal>

            <Modal
                modalTitle="Создание читателя"
                formId="create-reader-form"
                isOpen={createModalOpen}
                onClose={() => setCreateModalOpen(false)}
            >
                <CreateReaderForm
                    handleSubmit={() => handleCreateReader}
                >

                </CreateReaderForm>
            </Modal>

            <MainSectionHeader
                title="Читатели"
                desc="Просмотр информации о читателях"
                includeFilter={true}
                onFilter={handleReaderFilter}
                filterText="ФИО, класс"

            >
                <div className="flex items-center gap-4">
                    <CreateButton
                        icon={<CgAdd />}
                        onCreate={() => setCreateModalOpen(true)}
                    >

                    </CreateButton>
                    <RefreshButton onRefresh={() => fetchData()}>

                    </RefreshButton>
                </div>
            </MainSectionHeader>
            <Table
                columnNames={columnNames}
            >
                {
                    readers.filter(reader => {
                        const query: string = filterResult.toLowerCase().trim();

                        const matchGrade: boolean = reader.gradeName.toLowerCase().trim().includes(query);
                        const matchReader: boolean = `${reader.lastName} ${reader.firstName} ${reader.middleName}`.toLowerCase().trim().includes(query);

                        return matchGrade || matchReader
                    })
                        .map(reader => (
                            <tr
                                key={reader.id}
                                onMouseEnter={() => setReaderId(reader.id)}
                                className="table-tr"
                            >
                                <td
                                    key={reader.lastName}
                                    className="table-td"
                                >
                                    {reader.lastName}
                                </td>
                                <td
                                    key={reader.firstName}
                                    className="table-td"
                                >
                                    {reader.firstName}
                                </td>
                                <td
                                    key={reader.middleName}
                                    className="table-td"
                                >
                                    {reader.middleName ? reader.middleName : "-"}
                                </td>
                                <td
                                    key={reader.gradeName}
                                    className="table-td"
                                >
                                    {reader.gradeName}
                                </td>
                                <td
                                    key={reader.username}
                                    className="table-td"
                                >
                                    {reader.username}
                                </td>
                                <td className="table-td table-td-actions">
                                    <div className="actions-container">
                                        <TableReaderHistoryButton
                                            onReaderHistoryOpen={() => handleReaderHistoryOpen()}
                                        >

                                        </TableReaderHistoryButton>
                                        <TableBorrowButton
                                            onBorrow={() => handleBorrow(reader.id, `${reader.lastName} ${reader.firstName}`)}
                                        >

                                        </TableBorrowButton>
                                    </div>
                                </td>
                            </tr>
                        ))}
            </Table>
        </>
    )
}