import { useEffect, useState, type ReactNode } from "react";
import { Link } from "react-router";
import Modal from "../Modal";

interface Props<T> {
    entityTitle: string
    fetchData: () => Promise<T[]>
    columnNames?: Record<string, string>,
    modalContent?: ReactNode,
}

const EntityCrud = <T extends Record<string, any>,>(
    { 
        entityTitle, 
        fetchData, 
        columnNames = {},
    }: Props<T>) => {
    const [data, setData] = useState<T[]>([]);
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [isModalOpen, setIsModalOpen] = useState<boolean>(false);

    useEffect(() => {
        fetchData()
            .then(result => {
                setData(result || []);
                setIsLoading(false);
            })
            .catch(err => {
                console.error(err);
                setData([]);
                setIsLoading(false);
            });
    }, [fetchData])

    const columns = data.length > 0 ? Object.keys(data[0]) : [];

    const getColumnDisplayName = (column: string): string => {
        return columnNames[column] || column;
    }

    const handleModalOpen = (): void => {
        setIsModalOpen(true);
    }

    const handleModalClose = (): void => {
        setIsModalOpen(false);
    }

    return (
        <>
            <div className="h-screen p-6 bg-slate-800 text-white">
                <div className="flex justify-between">
                    <h1 className="text-4xl">{entityTitle}</h1>
                    <Link className="text-3xl hover:text-emerald-500 transition-colors" to={"/admin"}>Вернуться в панель</Link>
                </div>

                {!isLoading && data.length === 0 && (
                    <p>Нет данных для отображения</p>
                )}

                {isLoading ? (
                    <p>Загрузка данных...</p>
                ) :
                    <div className="flex flex-col gap-6">
                        <div className="flex justify-between items-center">
                            <p>Найдено записей: {data.length}</p>
                            <button
                                className="text-xl hover:text-emerald-500 transition-colors"
                                onClick={handleModalOpen}
                            >
                                Добавить запись
                            </button>
                        </div>

                        <table className="w-full">
                            <thead>
                                {columns.map((column) => (
                                    <th
                                        className="border p-2 text-2xl"
                                        key={getColumnDisplayName(column)}
                                    >
                                        {getColumnDisplayName(column)}
                                    </th>
                                ))}
                            </thead>
                            <tbody>
                                {data.map((item, index) => (
                                    <tr
                                        className="text-center"
                                        key={index}
                                    >
                                        {columns.map((column) => (
                                            <td
                                                className="border p-2"
                                            >
                                                {item[column] ? item[column] : "-"}
                                            </td>
                                        ))}
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                }

                <Modal
                    isOpen={isModalOpen}
                    onClose={handleModalClose}
                >
                    
                </Modal>
            </div>
        </>
    )
}

export default EntityCrud;