import { useEffect, useState } from "react"
import { MainSectionHeader } from "../components/MainSectionHeader"
import { Table } from "../components/Table"
import { FiRefreshCcw } from "react-icons/fi"
import { MdClass } from "react-icons/md"
import type { GradeDto } from "../api/entities/Entity.Types"
import { useDataService } from "../api/services/dataService"
import Modal from "../components/Modal"
import { CreateGradeForm } from "../components/forms/CreateGradeForm"
import { RefreshButton } from "../components/buttons/RefreshButton"

export const GradesPage = () => {
    const { getAllGrades } = useDataService();
    const [createModalOpen, setCreateModalOpen] = useState<boolean>(false);

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
            {createModalOpen && (
                <Modal
                    modalTitle="Создание класса"
                    formId="create-grade-form"
                    isOpen={createModalOpen}
                    onClose={() => setCreateModalOpen(false)}
                >
                    <CreateGradeForm handleSubmit={fetchData}>

                    </CreateGradeForm>
                </Modal>
            )}

            <div className="flex flex-col gap-2">
                <MainSectionHeader title="Классы" desc="Управление информацией о классах">
                    <div className="flex items-center gap-4">
                        <button
                            className="main-section-header-button main-section-header-button-green"
                            onClick={() => setCreateModalOpen(true)}
                        >
                            <MdClass></MdClass>
                            <span>Создать</span>
                        </button>
                        <RefreshButton onRefresh={() => fetchData()}></RefreshButton>
                    </div>
                </MainSectionHeader>
                <Table columnNames={columnNames} includeOperations={false}>
                    {grades.map(grade => (
                        <tr
                            key={grade.id}
                            className="table-tr"
                        >
                            <td
                                className="table-td"
                            >
                                {grade.id}
                            </td>

                            <td
                                className="table-td"
                            >
                                {grade.displayName ? grade.displayName : ""}
                            </td>
                        </tr>
                    ))}
                </Table>
            </div>
        </>
    )
}