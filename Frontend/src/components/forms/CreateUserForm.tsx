import { useEffect, useState } from "react";
import type { CreateUserDto, RoleDto } from "../../api/entities/Entity.Types"
import { useForm } from "../../hooks/useForm"
import { BaseInput } from "../CustomInput";
import { useDataService } from "../../api/services/dataService";
import type { MyFormProps } from "./Props";
import { useUserService } from "../../api/services/userService";
import toast from "react-hot-toast";

export const CreateUserForm = ({handleSubmit}: MyFormProps) => {
    const [roles, setRoles] = useState<RoleDto[]>([]);
    const { getAllRoles } = useDataService();
    const { create } = useUserService();

    const { values, handleChange, resetForm } = useForm<CreateUserDto>({
        firstName: "",
        lastName: "",
        middleName: "",
        password: "",
        role: "None",
        username: "",
        email: ""
    });

    const handleCreate = async (e: React.SubmitEvent) => {
        e.preventDefault();

        const result = await create(values);
        if (result.id){
            toast.success("Пользователь создан");
            handleSubmit();
        }
    }

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
                onSubmit={handleCreate}
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
                    placeholder="Пароль (от 4 символов)"
                    onChange={handleChange}
                    required={true}
                    minLength={4}
                    maxLength={32}
                >

                </BaseInput>

                <BaseInput
                    placeholder="Почта (например: test@example.com)"
                    type="email"
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