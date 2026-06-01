import { FiRefreshCcw } from "react-icons/fi"
import { MainSectionHeader } from "../components/MainSectionHeader"
import { Table } from "../components/Table"
import { useEffect, useState } from "react"
import type { GradeDto, ReaderDto } from "../api/entities/Entity.Types"
import { useDataService } from "../api/services/dataService"
import { RefreshButton } from "../components/buttons/RefreshButton"
import { CreateButton } from "../components/buttons/CreateButton"
import { CgAdd } from "react-icons/cg"
import Modal from "../components/Modal"
import { CreateReaderForm } from "../components/forms/CreateReaderForm"
import { TableEditButton } from "../components/buttons/TableEditButton"

export const ReadersPage = () => {
    const [createModalOpen, setCreateModalOpen] = useState<boolean>(false);

    const columnNames = [
        "Фамилия",
        "Имя",
        "Отчество",
        "Класс",
        "Никнейм"
    ]

    const [readers, setReaders] = useState<ReaderDto[]>([]);
    const { getAllReaders } = useDataService();

    const handleCreateReader = () => {
        setCreateModalOpen(false);
        fetchData();
    }

    const fetchData = async () => {
        setReaders(await getAllReaders());
    }

    useEffect(() => {
        fetchData();
    }, [])

    return (
        <>
            {createModalOpen && (
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
            )}

            <MainSectionHeader title="Читатели" desc="Просмотр информации о читателях">
                <div className="flex items-center gap-4">
                    <CreateButton
                        icon={<CgAdd />}
                        handleCreate={() => setCreateModalOpen(true)}
                    >

                    </CreateButton>
                    <RefreshButton onRefresh={() => fetchData()}>

                    </RefreshButton>
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
                                <TableEditButton handleEdit={() => console.log("edit user")}>

                                </TableEditButton>
                            </div>
                        </td>
                    </tr>
                ))}
            </Table>
        </>
    )
}