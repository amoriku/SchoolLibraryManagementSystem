import { FiRefreshCcw, FiUser } from "react-icons/fi"
import { MainSectionHeader } from "../components/MainSectionHeader"
import { Table } from "../components/Table"
import { useEffect, useState } from "react"
import type { AuthorDto } from "../api/entities/Entity.Types"
import { useDataService } from "../api/services/dataService"
import Modal from "../components/Modal"
import { CreateAuthorForm } from "../components/forms/CreateAuthorForm"
import { RefreshButton } from "../components/buttons/RefreshButton"
import { GetAllUsers } from "../api/entities/UserApi"

export const AuthorsPage = () => {
    const { getAllAuthors } = useDataService();
    const [createModalOpen, setCreateModalOpen] = useState<boolean>(false);

    const [authors, setAuthors] = useState<AuthorDto[]>([])
    const columnNames: string[] = [
        "Идентификатор",
        "Фамилия",
        "Имя",
        "Отчество"
    ]

    const fetchData = async () => {
        setAuthors(await getAllAuthors())
    }

    useEffect(() => {
        fetchData();
    }, [])

    return (
        <>
            {createModalOpen && (
                <Modal
                    modalTitle="Создание автора"
                    isOpen={createModalOpen}
                    formId="create-author-form"
                    onClose={() => {setCreateModalOpen(false)}}
                >
                    <CreateAuthorForm>

                    </CreateAuthorForm>
                </Modal>
            )}

            <div className="flex flex-col gap-2">
                <MainSectionHeader title="Авторы" desc="Управление информацией о авторах">
                    <div className="flex items-center gap-4">
                        <button
                            className="main-section-header-button main-section-header-button-green"
                            onClick={() => { setCreateModalOpen(!createModalOpen) }}
                        >
                            <FiUser></FiUser>
                            <span>Создать</span>
                        </button>
                        <RefreshButton onRefresh={() => fetchData()}></RefreshButton>
                    </div>
                </MainSectionHeader>
                <Table columnNames={columnNames} includeOperations={false}>
                    {authors.map(author => (
                        <tr
                            key={author.id}
                            className="table-tr"
                        >
                            <td
                                key={author.id}
                                className="table-td"
                            >
                                {author.id}
                            </td>
                            <td
                                key={author.lastName}
                                className="table-td"
                            >
                                {author.lastName}
                            </td>
                            <td
                                key={author.firstName}
                                className="table-td"
                            >
                                {author.firstName}
                            </td>
                            <td
                                key={author.middleName}
                                className="table-td"
                            >
                                {author.middleName ? author.middleName : "-"}
                            </td>
                        </tr>
                    ))}
                </Table>
            </div>
        </>
    )
}