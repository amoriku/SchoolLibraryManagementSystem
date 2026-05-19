import { type ReactNode } from "react"

interface ModalProps {
    isOpen: boolean,
    children?: ReactNode,
    modalTitle?: string,
    onSave?: (data: any) => Promise<void>,
    onClose?: () => void;
}

const Modal = (
    {
        children,
        isOpen,
        modalTitle,
        onSave,
        onClose
    }: ModalProps) => {
        
    if (!isOpen) return null;

    return (
        <>
            <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
                <div className="relative">
                    <div className="flex gap-8 text-2xl">
                        <h2>{modalTitle}</h2>
                        <button
                            onClick={onClose}
                            className="text-indigo-500 transition-colors hover:text-indigo-400 z-10"
                        >
                            X
                        </button>
                    </div>
                    {children}
                </div>
            </div>
        </>
    )
}

export default Modal;