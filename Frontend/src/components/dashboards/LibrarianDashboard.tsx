import { useEffect, useState } from "react"
import { DashboardSimpleCard } from "./DashboardCard"
import type { BookDto, BorrowingDto, ReaderDto, ReserveDto } from "../../api/entities/Entity.Types";
import { useDataService } from "../../api/services/dataService";
import { DashboardTableCard } from "./DashboardTableCard";
import { TableBorrowButton } from "../buttons/TableBorrowButton";
import { ActionsContainer } from "../ActionsContainer";
import { useBorrowingService } from "../../api/services/borrowingService";

export const LibrarianDashboard = () => {
    const [books, setBooks] = useState<BookDto[]>([]);
    const [readers, setReaders] = useState<ReaderDto[]>([]);
    const [reservations, setReservations] = useState<ReserveDto[]>([]);
    const [borrowings, setBorrowings] = useState<BorrowingDto[]>([]);

    const { getAllBooks, getAllReaders, getAllReservations, getAllBorrowings } = useDataService();
    const { create } = useBorrowingService();

    const fetchData = async () => {
        setBooks(await getAllBooks())
        setReaders(await getAllReaders());
        setReservations(await getAllReservations());
        setBorrowings(await getAllBorrowings());
    }

    const handleBorrowOperation = async (reservationId: number) => {
        await create({ReservationId: reservationId})
    }

    useEffect(() => {
        fetchData();
    }, [])

    return (
        <>
            <div className="flex flex-wrap gap-4">
                <DashboardSimpleCard title="Всего читателей" data={readers.length}></DashboardSimpleCard>
                <DashboardSimpleCard title="Выдано книг" data={borrowings.length}></DashboardSimpleCard>
                <DashboardSimpleCard title="Возвращено книг" data={0}></DashboardSimpleCard>
                <DashboardSimpleCard title="Утерянные книги" data={0}></DashboardSimpleCard>
                <DashboardSimpleCard title="Всего книг" data={books.length}></DashboardSimpleCard>
                <DashboardSimpleCard title="Активные читатели" data={0}></DashboardSimpleCard>
                <DashboardTableCard title="Зарезервированные книги" columnNames={["Читатель", "Книга", "Дата запроса"]}>
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
                                <TableBorrowButton handleBorrow={() => handleBorrowOperation(reservation.reserveId)}></TableBorrowButton>
                            </ActionsContainer>
                        </tr>
                    ))}
                </DashboardTableCard>
            </div>
        </>
    )
}