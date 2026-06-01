import { FiRefreshCw, FiUser } from "react-icons/fi"
import { Table } from "../components/Table"
import React, { useEffect, useState } from "react"
import Modal from "../components/Modal";
import { CreateUser, RemoveUser } from "../api/entities/UserApi";
import toast from "react-hot-toast";
import type { RoleDto, CreateUserDto, UserDto } from "../api/entities/Entity.Types";
import { MainSectionHeader } from "../components/MainSectionHeader";
import { CreateUserForm } from "../components/forms/CreateUserForm";
import { useDataService } from "../api/services/dataService";
import { TableDeleteButton } from "../components/buttons/TableDeleteButton";
import { TableEditButton } from "../components/buttons/TableEditButton";
import { ActionsContainer } from "../components/ActionsContainer";

export const UsersPage = () => {
    const [users, setUsers] = useState<UserDto[]>([]);

    const { getAllUsers, getAllRoles } = useDataService();

    const [createModalOpen, setCreateModalOpen] = useState<boolean>(false);
    const [createUserFormData, setCreateUserFormData] = useState({
        firstName: "",
        lastName: "",
        middleName: "",
        email: "",
        username: "",
        role: "None",
        password: "",
    })

    const columnNames: string[] = [
        "Идентификатор",
        "Никнейм",
        "Почта",
        "Роль"
    ]

    const handleUserDelete = async (userId: string) => {
        // console.log(userId);

        // I think this will be enough for now
        if (window.confirm("Вы уверены?")) {
            await RemoveUser(userId)
            getAllUsers();
        }
    }

    const fetchInitialData = async () => {
        setUsers(await getAllUsers())
    }

    useEffect(() => {
        fetchInitialData()
    }, [])

    return (
        <>
            <div>
                {createModalOpen && (
                    <Modal onClose={() => setCreateModalOpen(false)} formId="create-user-form" isOpen={createModalOpen} modalTitle="Создание пользователя">
                        <CreateUserForm handleSubmit={() => fetchInitialData()}></CreateUserForm>
                    </Modal>
                )}

                <MainSectionHeader title="Пользователи" desc="Управление учетными записями">
                    <div className="flex items-center gap-2">
                        <button
                            className="main-section-header-button main-section-header-button-green"
                            onClick={() => setCreateModalOpen(true)}
                        >
                            <FiUser></FiUser>
                            <span>Создать</span>
                        </button>
                        <button
                            className="main-section-header-button main-section-header-button-slate"
                            onClick={() => getAllUsers()}
                        >
                            <FiRefreshCw></FiRefreshCw>
                            <span>Обновить</span>
                        </button>
                    </div>
                </MainSectionHeader>
                <div>
                    <Table columnNames={columnNames}>
                        {users.map(user => (
                            <tr
                                className="table-tr"
                                key={user.id}
                            >
                                <td className="table-td-id">
                                    {user.id}
                                </td>
                                <td className="table-td">
                                    {user.username}
                                </td>
                                <td className="table-td">
                                    {user.email ? user.email : "-"}
                                </td>
                                <td className="table-td">
                                    {user.role}
                                </td>
                                {/* Блок операций */}
                                <ActionsContainer>
                                    <TableEditButton handleEdit={() => console.log("edit user")}>

                                    </TableEditButton>
                                    <TableDeleteButton handleDelete={() => handleUserDelete(user.id)}>

                                    </TableDeleteButton>
                                </ActionsContainer>
                            </tr>
                        ))}
                    </Table>
                </div>
            </div>
        </>
    )
}