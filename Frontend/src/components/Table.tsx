interface TableProps {
    columnNames: string[],
    children?: React.ReactNode
}

export const Table = ({ columnNames, children }: TableProps) => {
    return (
        <>
            {/* Сделали подложку белой/светлой, чтобы данные хорошо читались */}
            <div className="mt-8 bg-white border border-slate-200 rounded-xl p-4 shadow-sm overflow-x-auto w-full">
                <table className="w-full border-collapse text-left table-fixed">
                    <thead>
                        <tr className="border-b border-slate-300 text-xl font-bold text-slate-700">
                            {columnNames.map((name, index) => (
                                <th key={index} className="table-th">{name}</th>
                            ))}
                            {/* Выравниваем заголовок операций по правому краю */}
                            <th className="table-th text-right">Операции</th>
                        </tr>
                    </thead>

                    <tbody>
                        {children}
                    </tbody>
                </table>
            </div>
        </>
    )
}