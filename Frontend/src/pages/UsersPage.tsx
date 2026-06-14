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
import { useUserService } from "../api/services/userService";
import { ConfirmModal } from "../components/ConfirmationModal";
import { RefreshButton } from "../components/buttons/RefreshButton";

export const UsersPage = () => {
    const [users, setUsers] = useState<UserDto[]>([]);
    const { getAllUsers, getAllRoles } = useDataService();
    const { remove } = useUserService();


    const [userId, setUserId] = useState<string | null>(null);
    const [isConfirmOpen, setIsConfirmOpen] = useState<boolean>(false);

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

    const handleDelete = (userId: string) => {
        setUserId(userId);
        setIsConfirmOpen(true);
    }

    const handleDeleteConfirm = async () => {
        if (userId === null) {
            return;
        }

        try {
            await remove(userId)
            toast.success("Пользователь успешно удален")
        }
        catch (error) {
            toast.error("При удалении пользователя произошла ошибка");
            setIsConfirmOpen(false)
            setUserId(null)
        }
        finally {
            setIsConfirmOpen(false)
            setUserId(null)
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
            <ConfirmModal
                isOpen={isConfirmOpen}
                confirmText="Это действие необратимо"
                isDanger={true}
                title="Удаление учетной записи"
                message="Вы уверены, что хотите удалить учетную запись?"
                onCancel={() => setIsConfirmOpen(false)}
                onConfirm={handleDeleteConfirm}
            >

            </ConfirmModal>

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
                        <RefreshButton onRefresh={fetchInitialData}></RefreshButton>
                    </div>
                </MainSectionHeader>
                <div>
                    <Table columnNames={columnNames}>
                        {users.map(user => (
                            <tr
                                className="table-tr"
                                key={user.id}
                            >
                                <td className="table-td">
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
                                    {/* <TableEditButton onEdit={() => console.log("edit user")}>

                                    </TableEditButton> */}
                                    <TableDeleteButton onDelete={() => handleDelete(user.id)}>

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