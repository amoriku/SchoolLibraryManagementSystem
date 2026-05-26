import toast from "react-hot-toast";
import type { AuthorCreateDto } from "../../api/entities/Entity.Types";
import { useAuthorService } from "../../api/services/authorService";
import { useForm } from "../../hooks/useForm"
import { BaseInput } from "../CustomInput";

export const CreateAuthorForm = () => {
    const { values, handleChange, resetForm } = useForm<AuthorCreateDto>({
        firstName: "",
        lastName: "",
        middleName: ""
    });
    const { create } = useAuthorService();

    const handleCreate = async (e: React.SubmitEvent) => {
        e.preventDefault();
        const result = await create(values)
        if (result.id)
        {
            toast.success("Автор успешно создан");
        }
    }

    return (
        <>
            <form
                action="post"
                onSubmit={handleCreate}
                id="create-author-form"
                className="create-form"
                autoComplete="off"
            >
                <BaseInput
                    value={values.lastName}
                    required={true}
                    name="lastName"
                    onChange={handleChange}
                    placeholder="Введите фамилию*"
                >

                </BaseInput>

                <BaseInput
                    value={values.firstName}
                    required={true}
                    name="firstName"
                    onChange={handleChange}
                    placeholder="Введите имя*"
                >

                </BaseInput>

                <BaseInput
                    value={values.middleName}
                    required={false}
                    name="middleName"
                    onChange={handleChange}
                    placeholder="Введите отчество"
                >

                </BaseInput>
            </form>
        </>
    )
}