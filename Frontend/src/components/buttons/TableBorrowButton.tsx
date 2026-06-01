import type { BorrowButtonProps } from "./Props"

export const TableBorrowButton = ({handleBorrow}: BorrowButtonProps) => {
    return (
        <button
            className="button button-green"
            onClick={handleBorrow}
        >
            Выдать
        </button>
    )
}