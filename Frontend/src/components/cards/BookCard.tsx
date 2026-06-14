import { CgAdd } from "react-icons/cg"
import { useReserveService } from "../../api/services/reservationService";
import { useState } from "react";
import type { BookHistoryDto, ReserveCreateDto, ReserveDto } from "../../api/entities/Entity.Types";
import { useAuth } from "../../hooks/useAuth";
import toast, { useToaster } from "react-hot-toast";
import { RoleBased } from "../RoleBased";
import { BsQuestion } from "react-icons/bs";
import { FaQuestion } from "react-icons/fa";
import Modal from "../Modal";
import { useBookService } from "../../api/services/bookService";

interface BookProps {
    id: number,
    title: string,
    description?: string,
    publishedYear?: number,
    quantity?: number,
    author?: string,
}

export const BookCard = ({ id, title, description, publishedYear, author, quantity = 0 }: BookProps) => {
    const { reserve } = useReserveService();
    const { getHistory, getNearestAvailabilityDate } = useBookService();
    const { isAuthenticated } = useAuth();

    const [isBookInfoOpened, setIsBookInfoOpened] = useState<boolean>(false);
    const [bookHistory, setBookHistory] = useState<BookHistoryDto[]>([]);

    const handleOpenBookInfo = async () => {
        setIsBookInfoOpened(true)
        setBookHistory(await getHistory(id));
        console.log(await getNearestAvailabilityDate(id));
    }

    const handleReserve = async () => {
        if (!isAuthenticated) {
            toast.error("Войдите, чтобы зарезервировать книгу")
            return;
        }

        const data: ReserveCreateDto = {
            libraryItemId: id
        }

        try {
            await reserve(data);
            toast.success(`Вы забронировали книгу ${title}`)
        }
        catch (error) {
            toast.error("Вы уже бронировали или у вас есть эта книга")
        }
    }

    return (
        <>
            <Modal
                isOpen={isBookInfoOpened}
                modalTitle="Информация по книге"
                onClose={() => setIsBookInfoOpened(false)}
                includeOperations={false}
            >
                <div>
                    <h1>В данный момент книги нет в наличии</h1>
                    <span>Книга будет доступна приблизительно </span>
                </div>

                <RoleBased role="Librarian">
                    <div>For librarian only</div>
                </RoleBased>
            </Modal>

            <div className="flex flex-col justify-between bg-slate-100 hover:bg-slate-200 transition-colors border border-slate-300 rounded-xl p-5 shadow-sm min-h-[220px]">
                <div className="space-y-2 w-full">
                    <div className="flex justify-between items-start gap-2 w-full">
                        <h3 className="font-bold text-lg text-slate-800 line-clamp-2 leading-snug" title={title}>
                            {title}
                        </h3>

                        {quantity > 0 ? (
                            <button
                                className="book-card-available"
                                title="Зарезервировать"
                                onClick={handleReserve}
                            >
                                <CgAdd size={20}></CgAdd>
                            </button>
                        ) : (
                            <button
                                className="book-card-available"
                                title="Узнать приблизительное время доступности книги"
                                onClick={handleOpenBookInfo}
                            >
                                <BsQuestion size={20}></BsQuestion>
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
        </>
    )
}