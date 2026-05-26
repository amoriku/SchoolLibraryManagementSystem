import { useEffect, useState } from "react"
import { DashboardSimpleCard } from "./DashboardCard"
import type { BookDto, ReaderDto } from "../../api/entities/Entity.Types";
import { useDataService } from "../../api/services/dataService";

export const LibrarianDashboard = () => {
    const [books, setBooks] = useState<BookDto[]>([]);
    const [readers, setReaders] = useState<ReaderDto[]>([]);

    const { getAllBooks, getAllReaders } = useDataService();

    const fetchData = async () => {
        setBooks(await getAllBooks())
        setReaders(await getAllReaders());
    }

    useEffect(() => {
        fetchData();
    }, [])

    return (
        <>
            <div className="flex flex-wrap gap-4">
                <DashboardSimpleCard title="Всего книг" data={books.length}></DashboardSimpleCard>
                <DashboardSimpleCard title="Всего читателей" data={readers.length}></DashboardSimpleCard>
                <DashboardSimpleCard title="Возвращено книг" data={0}></DashboardSimpleCard>
                <DashboardSimpleCard title="Утерянные книги" data={0}></DashboardSimpleCard>
                <DashboardSimpleCard title="Активные читатели" data={0}></DashboardSimpleCard>
            </div>
        </>
    )
}