import React, { useEffect, useState } from "react"
import type { AuthorDto, CreateBookDto } from "../../api/entities/Entity.Types"
import { useDataService } from "../../api/services/dataService";
import { BaseInput } from "../CustomInput";
import { useForm } from "../../hooks/useForm";
import { useBookService } from "../../api/services/bookService";
import toast from "react-hot-toast";

export const CreateBookForm = () => {
    const [authors, setAuthors] = useState<AuthorDto[]>([]);
    const { getAllAuthors } = useDataService();
    const { create } = useBookService();

    const fetchData = async () => {
        setAuthors(await getAllAuthors());
    }

    const { values, handleChange, resetForm, handleSubmit } = useForm<CreateBookDto>({
        title: "",
        authorIds: [],
        description: "",
        publishedYear: 0,
        price: 0
    }, { onSuccess: fetchData });

    const handleCreate = async (e: React.SubmitEvent) => {
        e.preventDefault();
        const result = await create(values)

        if (result.id)
        {
            toast.success("Книга успешно создана")
        }
    }

    useEffect(() => {
        fetchData();
    }, [])

    return (
        <>
            <form
                action="POST"
                onSubmit={handleCreate}
                id="create-book-form"
                className="create-form"
                autoComplete="off"
            >
                <BaseInput
                    required={true}
                    placeholder="Введите название книги*"
                    onChange={handleChange}
                    name="title"
                    value={values.title}
                >
                </BaseInput>

                <BaseInput
                    required={false}
                    placeholder="Введите описание"
                    onChange={handleChange}
                    name="description"
                    value={values.description}
                >
                </BaseInput>

                <BaseInput
                    required={false}
                    placeholder="Введите год издания"
                    onChange={handleChange}
                    name="publishedYear"
                    value={values.publishedYear}
                >
                </BaseInput>

                <select
                    name="authorIds"
                    id="author"
                    className="select-input"
                    onChange={handleChange}
                >
                    <option value="None">Выберите автора...</option>
                    {authors.map(author => (
                        <option value={author.id}>{`${author.lastName} ${author.firstName} ${author.middleName}`}</option>
                    ))}
                </select>
            </form>
        </>
    )
}