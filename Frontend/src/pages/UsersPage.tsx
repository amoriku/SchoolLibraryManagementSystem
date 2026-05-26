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

    const createUser = async (e: React.SubmitEvent) => {
        e.preventDefault();

        const userData: CreateUserDto = {
            firstName: createUserFormData.firstName,
            lastName: createUserFormData.lastName,
            username: createUserFormData.username,
            middleName: createUserFormData.middleName,
            role: createUserFormData.role,
            password: createUserFormData.password,
            email: createUserFormData.email,
        }

        console.log(userData);

        if (userData.role == "None") {
            toast.error("Выберите роль пользователя");
            return;
        }

        try {

            await CreateUser(userData);
            getAllUsers();
        }
        catch (error) {
            console.error("Ошибка при создании пользователя: \n", error)
        }
    }

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
                        <CreateUserForm></CreateUserForm>
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
                                    {user.email}
                                </td>
                                <td className="table-td">
                                    {user.role}
                                </td>
                                {/* Блок операций */}
                                <td className="table-td text-right">
                                    <div className="flex justify-end gap-4">
                                        <button className="button button-green">
                                            Изменить
                                        </button>
                                        <button
                                            onClick={() => handleUserDelete(user.id)}
                                            className="button button-red">
                                            Удалить
                                        </button>
                                    </div>
                                </td>
                            </tr>
                        ))}
                    </Table>
                </div>
            </div>
        </>
    )
}