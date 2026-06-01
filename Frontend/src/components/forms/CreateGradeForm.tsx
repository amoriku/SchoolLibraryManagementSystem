import toast from "react-hot-toast";
import type { CreateGradeDto } from "../../api/entities/Entity.Types";
import { useGradeService } from "../../api/services/gradeService";
import { useForm } from "../../hooks/useForm"
import { BaseInput } from "../CustomInput";
import type { MyFormProps } from "./Props";
import { validator } from "../../utils/validator";

export const CreateGradeForm = ({ handleSubmit }: MyFormProps) => {
    const { create } = useGradeService();
    const { validateGrade } = validator();


    const handleCreate = async (e: React.SubmitEvent) => {
        e.preventDefault();
        const formData = {
            "number": values.number,
            "letter": values.letter.toUpperCase()
        }

        const displayName: string = `${formData.number}-${formData.letter}`

        if (validateGrade(displayName)) {
            const result = await create(formData);
            if (result.id) {
                toast.success(`Класс ${result.displayName} создан`)
                handleSubmit();
            }
            else {
                toast.error("Что-то пошло не так");
            }
        }
        else {
            toast.error("Класс несоответствует шаблону (11-А, 5-Б)")
        }

    }

    const { values, handleChange, resetForm } = useForm<CreateGradeDto>({
        letter: "",
        number: 1
    });

    return (
        <>
            <form
                action="POST"
                id="create-grade-form"
                className="create-form"
                onSubmit={handleCreate}
            >
                <BaseInput
                    name="number"
                    type="number"
                    placeholder="Введите год обучения*"
                    value={values.number}
                    onChange={handleChange}
                    min={1}
                    max={11}
                    minLength={1}
                    maxLength={2}
                    required={true}
                >
                </BaseInput>
                <BaseInput
                    name="letter"
                    placeholder="Введите букву класса*"
                    maxLength={2}
                    value={values.letter}
                    onChange={handleChange}
                    required={true}
                >
                </BaseInput>
            </form>
        </>
    )
}