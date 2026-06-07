import type { DeleteButtonProps } from "./Props"

export const TableDeleteButton = ({onDelete}: DeleteButtonProps) => {
    return (
        <button
            className="button button-red"
            onClick={onDelete}
        >
            Удалить
        </button>
    )
}