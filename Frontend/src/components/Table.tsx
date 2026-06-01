interface TableProps {
    columnNames: string[],
    children?: React.ReactNode,
    title?: string,
}

export const Table = ({ columnNames, children, title }: TableProps) => {
    return (
        <>
            <div className="mt-8 overflow-x-auto w-full">
                <h1 className="text-xl text-slate-800 font-semibold mb-4">{title}</h1>
                <div className="bg-white border border-slate-200 shadow-sm rounded-xl p-4">
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
            </div>
        </>
    )
}