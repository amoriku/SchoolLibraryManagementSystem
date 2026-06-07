import toast from "react-hot-toast";
import { FiRefreshCcw } from "react-icons/fi"

interface Props {
    onRefresh: () => void | Promise<any>
}

export const RefreshButton = ({ onRefresh }: Props) => {
    const handleRefresh = async () => {
        try {
            await onRefresh();
            toast.success("Данные обновлены")
        }
        catch (error) {
            toast.error("Не удалось обновить данные")
        }
    }

    return (
        <button
            className="main-section-header-button main-section-header-button-slate"
            onClick={() => handleRefresh()}
        >
            <FiRefreshCcw></FiRefreshCcw>
            <span>Обновить</span>
        </button>
    )
}