import { useEffect, useState } from "react"
import { DashboardSimpleCard } from "./DashboardCard"
import type { BookDto, BookHistoryDto, BorrowingDto, ReaderDto, ReserveDto } from "../../api/entities/Entity.Types";
import { useDataService } from "../../api/services/dataService";
import { DashboardTableCard } from "./DashboardTableCard";
import { TableBorrowButton } from "../buttons/TableBorrowButton";
import { ActionsContainer } from "../ActionsContainer";
import { useBorrowingService } from "../../api/services/borrowingService";
import { TableReturnButton } from "../buttons/TableReturnButton";
import { ConfirmModal } from "../ConfirmationModal";
import toast from "react-hot-toast";
import dayjs from "dayjs";
import { TableCancelButton } from "../buttons/TableCancelButton";
import { useReserveService } from "../../api/services/reservationService";
import { useBookService } from "../../api/services/bookService";

export const LibrarianDashboard = () => {
    const [books, setBooks] = useState<BookDto[]>([]);
    const [readers, setReaders] = useState<ReaderDto[]>([]);
    const [reservations, setReservations] = useState<ReserveDto[]>([]);
    const [borrowings, setBorrowings] = useState<BorrowingDto[]>([]);
    const [borrowingHistores, setBorrowingHistores] = useState<BookHistoryDto[]>([]);
    const [returnHistories, setReturnHistories] = useState<BookHistoryDto[]>([]);

    const [isBorrowingConfirmOpen, setIsBorrowingConfirmOpen] = useState<boolean>(false);
    const [reservationId, setReservationId] = useState<number | null>(null);

    const [isReturnConfirmOpen, setIsReturnConfirmOpen] = useState<boolean>(false);
    const [itemCopyId, setItemCopyId] = useState<number | null>(null);

    const [isReserveCancelConfirmOpen, setIsReserveCancelConfirmOpen] = useState<boolean>(false);

    const { getAllBooks, getAllReaders, getAllReservations, getAllBorrowings } = useDataService();
    const { create, returnBook } = useBorrowingService();
    const { cancel } = useReserveService();
    const { getBorrowingHistories, getReturnHistories} = useBookService();

    const todayDate = new Date();

    const fetchData = async () => {
        setBooks(await getAllBooks())
        setReaders(await getAllReaders());
        setReservations(await getAllReservations());
        setBorrowings(await getAllBorrowings());
        setBorrowingHistores(await getBorrowingHistories());
        setReturnHistories(await getReturnHistories());
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

    const handleReserveCancel = (reservationId: number) => {
        setReservationId(reservationId);
        setIsReserveCancelConfirmOpen(true);
    }

    const totalBooksCount = books.reduce((acc, book) => acc + book.quantity, 0);

    const handleCancelReserveConfirm = async () => {
        if (reservationId === null) return;

        try {
            await cancel(reservationId);
            toast.success("Бронь отменена");
        }
        catch {
            toast.error("Не удалось отменить бронь");
            setIsReserveCancelConfirmOpen(false);
            setReservationId(null);
        }
        finally {
            setIsReserveCancelConfirmOpen(false);
            setReservationId(null);
        }
    }

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
                isOpen={isReserveCancelConfirmOpen}
                confirmText="Отменить бронь"
                message="Отмена брони читателя"
                onCancel={() => setIsReserveCancelConfirmOpen(false)}
                onConfirm={handleCancelReserveConfirm}
            >

            </ConfirmModal>

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
                <DashboardSimpleCard title="Выдано книг" data={borrowingHistores.length}></DashboardSimpleCard>
                <DashboardSimpleCard title="Возвращено книг" data={returnHistories.length}></DashboardSimpleCard>
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
                                <TableCancelButton onCancel={() => handleReserveCancel(reservation.reserveId)}></TableCancelButton>
                            </ActionsContainer>
                        </tr>
                    ))}
                </DashboardTableCard>
                <div className="flex flex-col md:grid md:grid-cols-2 gap-6 md:gap-4 w-full">

                    <DashboardTableCard title="Активные читатели" columnNames={["Читатель", "Книга", "Дата выдачи"]}>
                        {borrowings.map(borrowing => (
                            <tr
                                className="table-tr"
                                key={borrowing.id}
                            >
                                <td
                                    key={borrowing.reader.id}
                                    className="table-td py-2 md:py-3 px-3 text-xs md:text-sm">
                                    {`
                    ${borrowing.reader.firstName}
                    ${borrowing.reader.lastName}
                    ${borrowing.reader.middleName}
                `}
                                </td>

                                <td className="table-td py-2 md:py-3 px-3 text-xs md:text-sm">
                                    {borrowing.libraryItem}
                                </td>

                                <td className="table-td py-2 md:py-3 px-3 text-xs md:text-sm">
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
                                        className="table-td py-2 md:py-3 px-3 text-xs md:text-sm">
                                        {`
                            ${borrowing.reader.firstName}
                            ${borrowing.reader.lastName}
                            ${borrowing.reader.middleName}
                        `}
                                    </td>

                                    <td className="table-td py-2 md:py-3 px-3 text-xs md:text-sm">
                                        {borrowing.libraryItem}
                                    </td>

                                    <td className="table-td py-2 md:py-3 px-3 text-xs md:text-sm text-red-500 font-medium">
                                        {`${-dayjs(borrowing.dueDate).diff(dayjs(), "day")} дн`}
                                    </td>

                                    <td className="table-td py-2 md:py-3 px-3">
                                        { }
                                    </td>
                                </tr>
                            ))}
                    </DashboardTableCard>

                </div>

            </div>
        </>
    )
}