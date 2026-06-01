import { CgAdd } from "react-icons/cg"
import { useReserveService } from "../../api/services/reservationService";
import { useState } from "react";
import type { ReserveCreateDto, ReserveDto } from "../../api/entities/Entity.Types";
import { useAuth } from "../../hooks/useAuth";
import toast from "react-hot-toast";

interface BookProps {
    id: number,
    title: string,
    description?: string,
    publishedYear?: number,
    quantity?: number,
    author?: string,
}

export const BookCard = ({id, title, description, publishedYear, author, quantity = 0 }: BookProps) => {
    const { reserve } = useReserveService();
    const { isAuthenticated } = useAuth();

    const handleReserve = async () => {
        if (!isAuthenticated)
        {
            toast.success("Войдите, чтобы зарезервировать книгу");
            return;
        }

        const data: ReserveCreateDto = {
            libraryItemId: id
        }
        const response = await reserve(data)
        console.log(response);
    }

    return (
        <div className="flex flex-col justify-between bg-slate-100 hover:bg-slate-200 transition-colors border border-slate-300 rounded-xl p-5 shadow-sm min-h-[220px]">
            <div className="space-y-2 w-full">
                <div className="flex justify-between items-start gap-2 w-full">
                    <h3 className="font-bold text-lg text-slate-800 line-clamp-2 leading-snug" title={title}>
                        {title}
                    </h3>
                    {quantity > 0 && (
                        <button
                            className="book-card-available"
                            title="Зарезервировать"
                            onClick={handleReserve}
                        >
                            <CgAdd size={20}></CgAdd>
                        </button>
                    )}
                </div>

                {author && (
                    <p className="text-sm font-medium text-slate-500 truncate w-full">
                        {author}
                    </p>
                )}

                <p className="text-sm font-normal text-slate-600 line-clamp-3 leading-relaxed w-full">
                    {description?.trim() ? description : "Описание отсутствует"}
                </p>
            </div>

            <div className="mt-4 pt-3 border-t border-slate-200 flex flex-col items-start gap-2 w-full text-xs font-semibold text-slate-500">
                {(publishedYear ?? 0) > 0 && (
                    <span className="whitespace-nowrap">
                        Год издания: {publishedYear}
                    </span>
                )}
                <span>
                    {quantity > 0 ? (
                        <span className="book-card-available">
                            В наличии: {quantity} шт.
                        </span>
                    ) : (
                        <span className="book-card-not-available">
                            Нет в наличии
                        </span>
                    )}
                </span>
            </div>
        </div>
    )
}