import { useState } from "react";

type InputElement = HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement

interface UseFormOptions {
    onSuccess?: () => void
}

export function useForm<T>(initialValues: T, options?: UseFormOptions) {
    const [values, setValues] = useState<T>(initialValues);

    const handleChange = (e: React.ChangeEvent<InputElement>) => {
        const { name, value } = e.target;
        setValues(prev => ({
            ...prev,
            [name]: value
        }))

        console.log(values);
    }

    const handleSubmit = (submitCallback: (formData: T) => Promise<any>) => {
        return async (e: React.SubmitEvent<HTMLFormElement>) => {
            e.preventDefault();
            try {
                const result = await submitCallback(values);
                if (result) {
                    resetForm();
                    options?.onSuccess?.()
                }
            }
            catch (error) {
                console.error("Ошибка формы: \n", error);
            }
        }
    }

    const resetForm = () => setValues(initialValues);

    return { values, setValues, handleChange, handleSubmit, resetForm }
}

