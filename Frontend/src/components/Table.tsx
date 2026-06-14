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
            <div className={`w-full ${isModalVersion ? 'mt-2' : 'mt-6 md:mt-8'}`}>
                {title && (
                    <h1 className="text-lg md:text-xl text-slate-800 font-semibold mb-3 md:mb-4 text-center md:text-left">
                        {title}
                    </h1>
                )}

                <div className="w-full overflow-x-auto bg-white border border-slate-200 shadow-sm rounded-xl p-3 md:p-4">

                    <table className="w-full min-w-[700px] md:min-w-full border-collapse text-left table-auto md:table-fixed">
                        <thead>
                            <tr className="border-b border-slate-200 text-sm md:text-base font-bold text-slate-700">
                                {columnNames.map((name, index) => (
                                    <th key={index} className="table-th py-3 px-4">{name}</th>
                                ))}

                                {includeOperations && (
                                    <th className="table-th text-right py-3 px-4">Операции</th>
                                )}
                            </tr>
                        </thead>

                        <tbody className="text-sm md:text-base">
                            {children}
                        </tbody>
                    </table>

                </div>
            </div>
        </>
    )
}