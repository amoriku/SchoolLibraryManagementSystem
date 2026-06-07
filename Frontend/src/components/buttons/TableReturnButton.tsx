import type { ReturnButtonProps } from "./Props"

export const TableReturnButton = ({onReturn}: ReturnButtonProps) => {
    return (
        <button
            className="button button-green"
            onClick={(onReturn)}
        >
            Возврат
        </button>
    )
}