import type React from "react"
import { Link, NavLink } from "react-router"

interface Props {
    name?: string,
    icon?: React.ReactNode,
    to: string
}

export const SideBarItem = ({ name, icon, to }: Props) => {
    return (
        <>
            <NavLink
                to={to}
                className={({ isActive }) =>
                    `flex rounded-lg font-medium transition-all duration-200
                
                flex-col items-center justify-center gap-1 px-2 py-1 text-xs w-full text-center
                
                md:flex-row md:items-center md:justify-start md:gap-4 md:px-4 md:py-3 md:text-base md:w-auto
                
                ${isActive
                        ? "bg-my-light-green text-white shadow-sm" // Активная кнопка
                        : "text-slate-400 hover:bg-slate-50 hover:text-slate-700" // Обычная кнопка
                    }`
                }
            >
                <span className="text-xl md:text-lg shrink-0">{icon}</span>
        
                <span className="text-[10px] md:text-sm font-normal md:font-medium leading-none md:leading-normal truncate max-w-[65px] md:max-w-none">
                    {name}
                </span>
            </NavLink>
        </>
    )
}