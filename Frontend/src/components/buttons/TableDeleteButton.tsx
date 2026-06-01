import type { DeleteButtonProps } from "./Props"

export const TableDeleteButton = ({handleDelete}: DeleteButtonProps) => {
    return (
        <button
            className="button button-red"
            onClick={handleDelete}
        >
            Удалить
        </button>
    )
}