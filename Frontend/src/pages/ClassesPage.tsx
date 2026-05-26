import { useEffect, useState } from "react"
import { MainSectionHeader } from "../components/MainSectionHeader"
import { Table } from "../components/Table"
import { FiRefreshCcw } from "react-icons/fi"
import { MdClass } from "react-icons/md"
import type { GradeDto } from "../api/entities/Entity.Types"
import { useDataService } from "../api/services/dataService"

export const ClassesPage = () => {
    const { getAllGrades } = useDataService();

    const [grades, setGrades] = useState<GradeDto[]>([]);
    const columnNames: string[] = [
        "Идентификатор",
        "Наименование"
    ]

    const fetchData = async () => {
        setGrades(await getAllGrades());
    }
    
    useEffect(() => {
        fetchData();
    }, [])

    return (
        <>
            <div className="flex flex-col gap-2">
                <MainSectionHeader title="Классы" desc="Управление информацией о классах">
                    <div className="flex items-center gap-4">
                        <button
                            className="main-section-header-button main-section-header-button-green"
                        >
                            <MdClass></MdClass>
                            <span>Создать</span>
                        </button>
                        <button
                            className="main-section-header-button main-section-header-button-slate"
                        >
                            <FiRefreshCcw></FiRefreshCcw>
                            <span>Обновить</span>
                        </button>
                    </div>
                </MainSectionHeader>
                <Table columnNames={columnNames}>
                    {grades.map(grade => (
                        <tr
                            key={grade.id}
                            className="table-tr"
                        >
                            <td
                                className="table-td-id"
                            >
                                {grade.id}
                            </td>

                            <td
                                className="table-td"
                            >
                                {grade.displayName}
                            </td>
                        </tr>
                    ))}
                </Table>
            </div>
        </>
    )
}