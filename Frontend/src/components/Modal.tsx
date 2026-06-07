import { useEffect } from "react";

interface ModalProps {
    children?: React.ReactNode,
    modalTitle?: string,
    isOpen?: boolean,
    includeOperations?: boolean;
    formId?: string
    // onSave?: (data: any) => Promise<void>,
    onClose?: () => void;
    onOpen?: () => void
}

const Modal = (
    {
        children,
        modalTitle,
        isOpen,
        includeOperations = true,
        formId,
        onOpen,
        onClose
    }: ModalProps) => {


    if (!isOpen) return null

    return (
        <>
            <div
                onClick={onClose}
                className="fixed inset-0 bg-black/50 flex items-center justify-center z-50 p-4"
            >
                <div
                    onClick={(e) => e.stopPropagation()}
                    className="w-full max-w-md relative border border-slate-100 bg-slate-200 p-6 rounded-xl flex flex-col gap-5 shadow-2xl"
                >
                    {modalTitle && (
                        <div className="flex items-center justify-between text-2xl font-semibold">
                            <h2 className="text-slate-700">{modalTitle}</h2>
                        </div>
                    )}

                    <div className="text-slate-600">
                        {children}
                    </div>

                    <div className="w-full flex justify-center text-lg gap-3 text-slate-50 mt-2">
                        {includeOperations && (
                            <button
                                type="submit"
                                form={formId}
                                className="flex-1 bg-my-light-green hover:opacity-90 transition-opacity rounded-xl px-5 py-2.5 font-medium"
                            >
                                Сохранить
                            </button>
                        )}
                        <button
                            type="button"
                            onClick={onClose}
                            className="flex-1 bg-slate-400 hover:bg-slate-500 transition-colors rounded-xl px-5 py-2.5 text-slate-50 font-medium"
                        >
                            Отмена
                        </button>
                    </div>
                </div>
            </div>
        </>
    )
}

export default Modal;