interface TableProps {
    columnNames: string[],
    children?: React.ReactNode,
    title?: string,
    includeOperations?: boolean,
    isModalVersion?: boolean
}

export const Table = ({ columnNames, children, title, includeOperations = true, isModalVersion = false }: TableProps) => {
    return (
        <>
            <div className={isModalVersion ? "w-full mt-4" : "mt-8 overflow-x-auto w-full"}>
                {title && (
                    <h1 className={`${isModalVersion ? "text-base font-bold text-slate-800" : "text-xl text-slate-800 font-semibold"} mb-4`}>
                        {title}
                    </h1>
                )}

                {/* Если это модалка/дашборд, убираем лишние бордеры, тени и паддинги */}
                <div className={isModalVersion
                    ? "bg-white w-full"
                    : "bg-white border border-slate-200 shadow-sm rounded-xl p-4"
                }>
                    {/* table-auto адаптирует ширину колонок под длину слов, спасая от наездов текста */}
                    <table className="w-full border-collapse text-left table-auto">
                        <thead>
                            <tr className={isModalVersion
                                ? "border-b border-slate-200 text-xs font-bold uppercase tracking-wider text-slate-400" // Стильный мелкий шрифт для компактного UI
                                : "border-b border-slate-300 text-xl font-bold text-slate-770" // Твой стандартный крупный шрифт
                            }>
                                {columnNames.map((name, index) => (
                                    <th
                                        key={index}
                                        className={isModalVersion ? "pb-3 pr-3 font-bold" : "table-th"}
                                    >
                                        {name}
                                    </th>
                                ))}

                                {includeOperations && (
                                    <th className={isModalVersion ? "pb-3 text-right" : "table-th text-right"}>
                                        Операции
                                    </th>
                                )}
                            </tr>
                        </thead>

                        {/* Тонкие разделительные линии между строками */}
                        <tbody className={isModalVersion ? "divide-y divide-slate-100 text-sm" : ""}>
                            {children}
                        </tbody>
                    </table>
                </div>
            </div>
        </>
    )
}