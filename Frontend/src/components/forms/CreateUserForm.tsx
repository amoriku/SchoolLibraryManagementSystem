import { useEffect, useState } from "react";
import type { CreateUserDto, RoleDto } from "../../api/entities/Entity.Types"
import { useForm } from "../../hooks/useForm"
import { BaseInput } from "../CustomInput";
import { useDataService } from "../../api/services/dataService";

export const CreateUserForm = () => {
    const [roles, setRoles] = useState<RoleDto[]>([]);
    const { getAllRoles } = useDataService();

    const { values, handleChange, resetForm } = useForm<CreateUserDto>({
        firstName: "",
        lastName: "",
        middleName: "",
        password: "",
        role: "None",
        username: "",
        email: ""
    });

    const fetchRoles = async () => {
        setRoles(await getAllRoles());
    }

    useEffect(() => {
        fetchRoles();
    }, [])

    return (
        <>
            <form
                action="POST"
                id="create-user-form"
                className="create-form"
                autoComplete="off"
            >
                <BaseInput
                    placeholder="Имя*"
                    onChange={handleChange}
                    required={true}
                >

                </BaseInput>

                <BaseInput
                    placeholder="Фамилия*"
                    onChange={handleChange}
                    required={true}
                >

                </BaseInput>

                <BaseInput
                    placeholder="Отчество"
                    onChange={handleChange}
                    required={false}
                >

                </BaseInput>

                <BaseInput
                    placeholder="Пароль (от 6 символов)"
                    onChange={handleChange}
                    required={true}
                    title="Пароль должен содержать не менее 1 специального символа, заглавной буквы, цифры и маленькой буквы."
                >

                </BaseInput>

                <BaseInput
                    placeholder="Почта (например: test@example.com)"
                    onChange={handleChange}
                    required={false}
                >

                </BaseInput>

                <BaseInput
                    placeholder="Никнейм"
                    onChange={handleChange}
                    required={false}
                >

                </BaseInput>

                <select
                    className="select-input"
                    name="role"
                    id="role"
                >
                    <option value="None">Выберите роль</option>
                    {roles.map(role => (
                        <option value={role.id}>{role.name}</option>
                    ))}
                </select>
            </form>
        </>
    )
}