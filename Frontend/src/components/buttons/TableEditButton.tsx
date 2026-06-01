import type { EditButtonProps } from "./Props";

export const TableEditButton = ({ handleEdit }: EditButtonProps) => {
    return (
        <button
            className="button button-green"
            onClick={handleEdit}
        >
            Изменить
        </button>
    )
}