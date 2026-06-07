interface SimpleCardProps {
    title: string,
    data: any,
    period?: boolean
}

export const DashboardSimpleCard = ({ title, data, period }: SimpleCardProps) => {
    return (
        <>
            <div className="flex-1/5 p-4 bg-white border rounded w-auto border-my-light-gray shadow flex flex-col gap-4">
                <h1 className="text-gray-400 text-2xl">{title}</h1>
                <span className="text-3xl font-semibold">{data}</span>
            </div>
        </>
    )
}