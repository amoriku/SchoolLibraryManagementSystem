import { useEffect, useState } from "react";
import { useBorrowingService } from "../api/services/borrowingService"
import { MainSectionHeader } from "../components/MainSectionHeader";
import { Table } from "../components/Table";
import type { BorrowingDto } from "../api/entities/Entity.Types";
import dayjs from "dayjs";

export const MyBooksPage = () => {
    const [activeBooks, setActiveBooks] = useState<BorrowingDto[]>([]);

    const columnNames: string[] = [
        "Книга",
        "Дата выдачи",
        "Вернуть до"
    ]
    const { getActive } = useBorrowingService();
    const fetchData = async () => {
        setActiveBooks(await getActive());
    }

    useEffect(() => {
        fetchData();
    }, [])

    return (
        <>
            <MainSectionHeader title="Мои книги" desc="Раздел выданных книг">

            </MainSectionHeader>
            <Table
                columnNames={columnNames}
                includeOperations={false}
            >
                {activeBooks.map(activeBook => (
                    <tr
                        className="table-tr"
                        key={activeBook.id}
                    >
                        <td
                            className="table-td"
                        >
                            {activeBook.libraryItem}
                        </td>
                        <td
                            className="table-td"
                        >
                            {new Date(activeBook.borrowedDate).toLocaleString()}
                        </td>
                        <td
                            className={`${dayjs(activeBook.dueDate).diff(dayjs(), 'day') < 0 ? 'table-td text-rose-600' : 'table-td'}`}
                        >
                            {activeBook.dueDate ? new Date(activeBook.dueDate).toLocaleString() : "-"}
                        </td>
                    </tr>
                ))}
            </Table>
        </>
    )
}