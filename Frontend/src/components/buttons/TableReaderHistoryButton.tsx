import type { ReaderHistoryButtonProps } from "./Props"

export const TableReaderHistoryButton = ({ onReaderHistoryOpen }: ReaderHistoryButtonProps) => {
    return (
        <button
            onClick={onReaderHistoryOpen}
            className="button button-rose"
        >
            Формуляр
        </button>
    )
}