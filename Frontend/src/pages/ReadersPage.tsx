import { FiRefreshCcw } from "react-icons/fi"
import { MainSectionHeader } from "../components/MainSectionHeader"
import { Table } from "../components/Table"
import { useEffect, useState } from "react"
import type { ReaderDto } from "../api/entities/Entity.Types"
import { useReaderService } from "../api/services/readerService"

export const ReadersPage = () => {
    const columnNames = [
        "Фамилия",
        "Имя",
        "Отчество",
        "Класс"
    ]

    const [readers, setReaders] = useState<ReaderDto[]>([]);
    const { getAllReaders } = useReaderService();

    const fetchData = async () => {
        setReaders(await getAllReaders());
    }

    useEffect(() => {
        fetchData();
    }, [])

    return (
        <>
            <MainSectionHeader title="Читатели" desc="Просмотр информации о читателях">
                <div className="flex items-center gap-4">
                    <button
                        className="main-section-header-button main-section-header-button-slate"
                    >
                        <FiRefreshCcw></FiRefreshCcw>
                        <span>Обновить</span>
                    </button>
                </div>
            </MainSectionHeader>
            <Table columnNames={columnNames}>
                {readers.map(reader => (
                    <tr
                        key={reader.id}
                        className="table-tr"
                    >
                        <td
                            key={reader.lastName}
                            className="table-td-id"
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
                            {reader.middleName}
                        </td>
                        <td
                            key={reader.gradeName}
                            className="table-td"
                        >
                            {reader.gradeName}
                        </td>
                    </tr>
                ))}
            </Table>
        </>
    )
}