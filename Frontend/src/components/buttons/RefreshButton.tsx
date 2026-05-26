import { FiRefreshCcw } from "react-icons/fi"

interface Props {
    onRefresh: () => void
}

export const RefreshButton = ({ onRefresh }: Props) => {
    return (
        <button
            className="main-section-header-button main-section-header-button-slate"
            onClick={() => onRefresh()}
        >
            <FiRefreshCcw></FiRefreshCcw>
            <span>Обновить</span>
        </button>
    )
}