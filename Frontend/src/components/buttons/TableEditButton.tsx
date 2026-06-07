import type { EditButtonProps } from "./Props";

export const TableEditButton = ({ onEdit }: EditButtonProps) => {
    return (
        <button
            className="button button-green"
            onClick={onEdit}
        >
            Изменить
        </button>
    )
}