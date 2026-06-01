import type { CreateButtonProps } from "./Props"

export const CreateButton = ({ handleCreate, icon }: CreateButtonProps) => {
    return (
        <button
            className="main-section-header-button main-section-header-button-green"
            onClick={handleCreate}
        >
            {icon}
            Создать
        </button>
    )
}