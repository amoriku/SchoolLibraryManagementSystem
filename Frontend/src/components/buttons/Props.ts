export interface DeleteButtonProps{
    onDelete: () => void;
}

export interface EditButtonProps{
    onEdit: () => void;
}

export interface BorrowButtonProps{
    onBorrow: () => void;
}
export interface ReturnButtonProps{
    onReturn: () => void;
}

export interface CreateButtonProps{
    icon?: React.ReactNode
    onCreate: () => void;
}

export interface CancelButtonProps{
    onCancel: () => void;
}

export interface ReaderHistoryButtonProps{
    onReaderHistoryOpen: () => void;
}