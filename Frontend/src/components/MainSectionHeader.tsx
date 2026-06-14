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
            <div className="w-full flex flex-col md:flex-row md:justify-between items-center md:items-start gap-4 md:gap-0 mb-6">
                <div className="flex flex-col gap-1 md:gap-2.5 text-center md:text-left w-full md:w-auto">
                    <h1 className="text-xl md:text-2xl font-bold text-slate-800">{title}</h1>
                    {desc && <span className="text-sm md:text-base text-slate-500 font-semibold">{desc}</span>}
                </div>

                <div className="flex flex-col sm:flex-row items-center gap-3 md:gap-8 w-full md:w-auto">
                    {includeFilter && (

                        <div className="text-left md:text-center max-w-sm md:max-w-none md:w-64 mx-auto md:mx-0">
                            <BaseInput
                                onChange={onFilter}
                                placeholder={filterText}
                            />
                        </div>
                    )}
                    {children && (
                        <div className="flex justify-center md:justify-end w-full md:w-auto">
                            {children}
                        </div>
                    )}
                </div>

            </div>
        </>
    )
}