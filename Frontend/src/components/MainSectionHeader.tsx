import { BaseInput } from "./CustomInput"

interface Props {
    title: string,
    desc?: string,
    children?: React.ReactNode,
    includeFilter?: boolean,
    filterText?: string,
    onFilter?: (e: React.ChangeEvent<HTMLInputElement>) => void
}

export const MainSectionHeader = ({ title, desc, children, includeFilter = false, filterText, onFilter }: Props) => {
    return (
        <>
            <div className="w-full flex justify-between">
                <div className="flex flex-col gap-2.5">
                    <h1 className="text-2xl font-bold">{title}</h1>
                    <span className="text-slate-500 font-semibold">{desc}</span>
                </div>
                <div className="flex justify-center items-center gap-8">
                    {includeFilter && (
                        <BaseInput
                            onChange={onFilter}
                            placeholder={filterText}
                        >
                        </BaseInput>
                    )}
                    {children}
                </div>
            </div>
        </>
    )
}