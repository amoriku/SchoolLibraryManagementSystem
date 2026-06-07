interface ConfirmModalProps {
    isOpen: boolean;
    title?: string;
    message: string;
    onConfirm: () => void;
    onCancel: () => void;
    confirmText?: string;
    cancelText?: string;
    isDanger?: boolean; // Если true, кнопка действия будет красной (для отмены/удаления)
}

export const ConfirmModal: React.FC<ConfirmModalProps> = ({
    isOpen,
    title = "Подтверждение действия",
    message,
    onConfirm,
    onCancel,
    confirmText = "Да, уверен",
    cancelText = "Отмена",
    isDanger = false
}) => {
    // Если модалка закрыта — ничего не рендерим
    if (!isOpen) return null;

    return (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4">
            {/* Задний фон с блюром (в твоем стиле) */}
            <div 
                className="absolute inset-0 bg-slate-900/40 backdrop-blur-sm transition-opacity"
                onClick={onCancel} // Клик мимо модалки закроет её
            />

            {/* Контентное окно */}
            <div className="relative bg-white rounded-xl shadow-xl max-w-md w-full p-6 border border-slate-250 animate-in fade-in zoom-in-95 duration-150">
                <h3 className="text-lg font-bold text-slate-900 mb-2">
                    {title}
                </h3>
                
                <p className="text-sm font-normal text-slate-600 mb-6 leading-relaxed">
                    {message}
                </p>

                {/* Кнопки управления */}
                <div className="flex justify-end gap-3 font-semibold text-sm">
                    <button
                        type="button"
                        onClick={onCancel}
                        className="px-4 py-2 text-slate-700 bg-slate-100 hover:bg-slate-200 active:bg-slate-300 transition-colors rounded-lg border border-slate-300"
                    >
                        {cancelText}
                    </button>
                    
                    <button
                        type="button"
                        onClick={onConfirm}
                        className={`px-4 py-2 text-white transition-colors rounded-lg shadow-sm ${
                            isDanger 
                                ? "bg-rose-600 hover:bg-rose-700 active:bg-rose-800" 
                                : "bg-indigo-600 hover:bg-indigo-700 active:bg-indigo-800"
                        }`}
                    >
                        {confirmText}
                    </button>
                </div>
            </div>
        </div>
    );
};