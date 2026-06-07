import type { CancelButtonProps } from "./Props";

export const TableCancelButton = ({onCancel}: CancelButtonProps) => {
    return (
        <button
            className="button button-rose"
            onClick={onCancel}
        >
            Отменить бронь
        </button>
    )
}