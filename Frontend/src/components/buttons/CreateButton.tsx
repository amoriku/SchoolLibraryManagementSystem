import type { CreateButtonProps } from "./Props"

export const CreateButton = ({ onCreate, icon }: CreateButtonProps) => {
    return (
        <button
            className="main-section-header-button main-section-header-button-green"
            onClick={onCreate}
        >
            {icon}
            Создать
        </button>
    )
}