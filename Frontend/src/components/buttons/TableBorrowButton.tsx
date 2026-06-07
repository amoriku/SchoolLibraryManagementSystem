import type { BorrowButtonProps } from "./Props"

export const TableBorrowButton = ({onBorrow}: BorrowButtonProps) => {
    return (
        <button
            className="button button-green"
            onClick={onBorrow}
        >
            Выдать
        </button>
    )
}