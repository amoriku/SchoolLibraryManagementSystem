interface Props {
    title: string,
    desc?: string,
    children?: React.ReactNode
}

export const MainSectionHeader = ({title, desc, children}: Props) => {
    return (
        <>
            <div className="w-full flex justify-between">
                <div className="flex flex-col gap-2.5">
                    <h1 className="text-2xl font-bold">{title}</h1>
                    <span className="text-slate-500 font-semibold">{desc}</span>
                </div>
                {children}
            </div>
        </>
    )
}