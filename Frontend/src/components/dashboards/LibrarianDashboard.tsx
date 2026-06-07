import { useEffect, useState } from "react"
import { DashboardSimpleCard } from "./DashboardCard"
import type { BookDto, BorrowingDto, ReaderDto, ReserveDto } from "../../api/entities/Entity.Types";
import { useDataService } from "../../api/services/dataService";
import { DashboardTableCard } from "./DashboardTableCard";
import { TableBorrowButton } from "../buttons/TableBorrowButton";
import { ActionsContainer } from "../ActionsContainer";
import { useBorrowingService } from "../../api/services/borrowingService";
import { TableReturnButton } from "../buttons/TableReturnButton";
import { ConfirmModal } from "../ConfirmationModal";
import toast from "react-hot-toast";
import dayjs from "dayjs";

export const LibrarianDashboard = () => {
    const [books, setBooks] = useState<BookDto[]>([]);
    const [readers, setReaders] = useState<ReaderDto[]>([]);
    const [reservations, setReservations] = useState<ReserveDto[]>([]);
    const [borrowings, setBorrowings] = useState<BorrowingDto[]>([]);

    const [isBorrowingConfirmOpen, setIsBorrowingConfirmOpen] = useState<boolean>(false);
    const [reservationId, setReservationId] = useState<number | null>(null);

    const [isReturnConfirmOpen, setIsReturnConfirmOpen] = useState<boolean>(false);
    const [itemCopyId, setItemCopyId] = useState<number | null>(null);

    const { getAllBooks, getAllReaders, getAllReservations, getAllBorrowings } = useDataService();
    const { create, returnBook } = useBorrowingService();

    const todayDate = new Date();

    const fetchData = async () => {
        setBooks(await getAllBooks())
        setReaders(await getAllReaders());
        setReservations(await getAllReservations());
        setBorrowings(await getAllBorrowings());
    }

    const handleReturn = (itemCopyId: number | null) => {
        setItemCopyId(itemCopyId);
        setIsReturnConfirmOpen(true);
    }

    const handleReturnConfirm = async () => {
        if (itemCopyId === null) return;

        try {
            await returnBook(itemCopyId);
            toast.success("Книга возвращена");
        }
        catch (error) {
            toast.error("Что-то пошло не так")
            setIsReturnConfirmOpen(false);
            setItemCopyId(null);
        }
        finally {
            setIsReturnConfirmOpen(false);
            setItemCopyId(null);
        }
    }

    const handleBorrowing = (reservationId: number) => {
        setReservationId(reservationId);
        setIsBorrowingConfirmOpen(true)
    }

    const totalBooksCount = books.reduce((acc, book) => acc + book.quantity, 0);

    const handleBorrowingConfirm = async () => {
        if (reservationId === null) return;

        try {
            await create({ reservationId: reservationId })
        }
        catch (error) {
            setIsBorrowingConfirmOpen(false);
            setReservationId(null);
        }
        finally {
            setIsBorrowingConfirmOpen(false);
            setReservationId(null);
        }
    }

    useEffect(() => {
        fetchData();
    }, [])

    return (
        <>
            <ConfirmModal
                isOpen={isReturnConfirmOpen}
                confirmText="Вернуть"
                message="Возврат книги от читателя"
                onCancel={() => setIsReturnConfirmOpen(false)}
                onConfirm={handleReturnConfirm}
            >

            </ConfirmModal>

            <ConfirmModal
                isOpen={isBorrowingConfirmOpen}
                confirmText="Выдать"
                message="Выдача книги читателю"
                onCancel={() => setIsBorrowingConfirmOpen(false)}
                onConfirm={handleBorrowingConfirm}
            >

            </ConfirmModal>

            <div className="flex flex-wrap gap-4">
                <DashboardSimpleCard title="Всего читателей" data={readers.length}></DashboardSimpleCard>
                <DashboardSimpleCard title="Выдано книг" data={borrowings.length}></DashboardSimpleCard>
                <DashboardSimpleCard title="Возвращено книг" data={0}></DashboardSimpleCard>
                <DashboardSimpleCard title="Утерянные книги" data={0}></DashboardSimpleCard>
                <DashboardSimpleCard title="Всего книг" data={totalBooksCount}></DashboardSimpleCard>
                <DashboardSimpleCard title="Активные читатели" data={borrowings.length}></DashboardSimpleCard>
                <DashboardTableCard title="Забронированные книги" columnNames={["Читатель", "Книга", "Дата запроса"]}>
                    {reservations.map(reservation => (
                        <tr
                            className="table-tr"
                            key={reservation.reserveId}
                        >
                            <td
                                key={reservation.reader.id}
                                className="table-td">
                                {`
                                    ${reservation.reader.firstName}
                                    ${reservation.reader.lastName}
                                    ${reservation.reader.middleName}
                                `}
                            </td>

                            <td className="table-td">
                                {reservation.libraryItem}
                            </td>

                            <td className="table-td">
                                {new Date(reservation.reservationDate).toLocaleString()}
                            </td>

                            <ActionsContainer>
                                <TableBorrowButton onBorrow={() => handleBorrowing(reservation.reserveId)}></TableBorrowButton>
                            </ActionsContainer>
                        </tr>
                    ))}
                </DashboardTableCard>
                <div className="flex-1/2 gap-4 flex-wrap">
                    <DashboardTableCard title="Активные читатели" columnNames={["Читатель", "Книга", "Дата выдачи"]}>
                        {borrowings.map(borrowing => (
                            <tr
                                className="table-tr"
                                key={borrowing.id}
                            >
                                <td
                                    key={borrowing.reader.id}
                                    className="table-td">
                                    {`
                                    ${borrowing.reader.firstName}
                                    ${borrowing.reader.lastName}
                                    ${borrowing.reader.middleName}
                                `}
                                </td>

                                <td className="table-td">
                                    {borrowing.libraryItem}
                                </td>

                                <td className="table-td">
                                    {new Date(borrowing.borrowedDate).toLocaleString()}
                                </td>

                                <ActionsContainer>
                                    <TableReturnButton onReturn={() => handleReturn(borrowing.libraryItemId)}></TableReturnButton>
                                </ActionsContainer>
                            </tr>

                        ))}
                    </DashboardTableCard>

                    <DashboardTableCard
                        columnNames={["Читатель", "Книга", "Срок задолженности"]}
                        title="Список должников"
                        includeOperations={false}
                    >
                        {borrowings
                            .filter(borrowing => borrowing.returnDate == null && new Date(borrowing.dueDate) < new Date())
                            .map(borrowing => (
                                <tr
                                    className="table-tr"
                                    key={borrowing.id}
                                >
                                    <td
                                        key={borrowing.reader.id}
                                        className="table-td">
                                        {`
                                            ${borrowing.reader.firstName}
                                            ${borrowing.reader.lastName}
                                            ${borrowing.reader.middleName}
                                        `}
                                    </td>

                                    <td className="table-td">
                                        {borrowing.libraryItem}
                                    </td>

                                    <td className="table-td">
                                        {`${-dayjs(borrowing.dueDate).diff(dayjs(), "day")} дн`}
                                    </td>

                                    <td className="table-td">
                                        {}
                                    </td>
                                </tr>
                            ))}
                    </DashboardTableCard>
                </div>
            </div>
        </>
    )
}