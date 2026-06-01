import { useEffect, useState } from "react";
import type { CreateReaderDto, GradeDto } from "../../api/entities/Entity.Types"
import { useForm } from "../../hooks/useForm";
import { BaseInput } from "../CustomInput";
import type { MyFormProps } from "./Props"
import { useReaderService } from "../../api/services/readerService";
import { useDataService } from "../../api/services/dataService";
import toast from "react-hot-toast";

export const CreateReaderForm = ({ handleSubmit }: MyFormProps,) => {
    const { values, handleChange, resetForm } = useForm<CreateReaderDto>({
        firstName: "",
        lastName: "",
        middleName: "",
        gradeId: 3
    });

    const [grades, setGrades] = useState<GradeDto[]>([]);
    const { getAllGrades } = useDataService();
    const { create } = useReaderService();

    const handleCreate = async (e: React.SubmitEvent) => {
        e.preventDefault();

        const result = await create(values);
        if (result.id)
        {
            toast.success("Читатель создан");
        }

        handleSubmit();
    }

    const fetchData = async () => {
        setGrades(await getAllGrades());
    }

    useEffect(() => {
        fetchData();
    }, [])

    return (
        <form
            action="post"
            className="create-form"
            id="create-reader-form"
            onSubmit={handleCreate}
        >
            <BaseInput
                name="lastName"
                onChange={handleChange}
                value={values.lastName}
                required={true}
                placeholder="Введите фамилию*"
            >
            </BaseInput>
            <BaseInput
                name="firstName"
                onChange={handleChange}
                value={values.firstName}
                required={true}
                placeholder="Введите имя*"
            >
            </BaseInput>
            <BaseInput
                name="middleName"
                onChange={handleChange}
                value={values.middleName}
                required={false}
                placeholder="Введите отчество"
            >
            </BaseInput>

            <select
                name="gradeId"
                id=""
                className="select-input"
                value={values.gradeId}
                onChange={handleChange}
            >
                <option value="None">Выберите класс...</option>
                {grades.map(grade => (
                    <option value={grade.id}>{grade.displayName}</option>
                ))}
            </select>
        </form>
    )


}