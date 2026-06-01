export interface DeleteButtonProps{
    handleDelete: () => void;
}

export interface EditButtonProps{
    handleEdit: () => void;
}

export interface BorrowButtonProps{
    handleBorrow: () => void;
}

export interface CreateButtonProps{
    icon?: React.ReactNode
    handleCreate: () => void;
}