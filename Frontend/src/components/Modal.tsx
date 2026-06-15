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
        <div
            onClick={onClose}
            className="fixed inset-0 bg-black/60 flex items-center justify-center z-50 p-4 animate-fade-in"
        >
            <div
                onClick={(e) => e.stopPropagation()}

                className="w-full max-w-md max-h-[85dvh] relative border border-slate-100 bg-slate-50 rounded-2xl flex flex-col shadow-2xl overflow-hidden"
            >

                <div className="flex items-center justify-between p-5 border-b border-slate-100">
                    {modalTitle ? (
                        <h2 className="text-xl md:text-2xl font-bold text-slate-800">{modalTitle}</h2>
                    ) : (
                        <div></div>
                    )}
                    <button
                        onClick={onClose}
                        className="text-slate-400 hover:text-slate-600 transition-colors text-2xl p-1 leading-none"
                        type="button"
                    >
                        &times;
                    </button>
                </div>

                <div className="flex-1 overflow-y-auto p-5 text-slate-600 text-sm md:text-base">
                    {children}
                </div>

                <div className="w-full flex flex-col-reverse sm:flex-row justify-end items-center gap-3 p-5 border-t border-slate-100 bg-slate-50">
                    <button
                        type="button"
                        onClick={onClose}
                        className="w-full sm:w-auto bg-slate-200 hover:bg-slate-300 text-slate-700 transition-colors rounded-xl px-5 py-2.5 text-base font-medium"
                    >
                        Отмена
                    </button>

                    {includeOperations && (
                        <button
                            type="submit"
                            form={formId}
                            className="w-full sm:w-auto bg-my-light-green hover:opacity-90 text-white transition-opacity rounded-xl px-6 py-2.5 text-base font-medium"
                        >
                            Сохранить
                        </button>
                    )}
                </div>
            </div>
        </div>
    )
}

export default Modal;