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
                    `flex items-center gap-4 px-4 py-3 rounded-lg font-medium shadow-sm transition-all ${isActive
                        ? "bg-my-light-green text-white" // Стили для активной кнопки
                        : "text-slate-400 hover:bg-slate-50 hover:text-slate-700 shadow-none" // Стили для обычной кнопки
                    }`
                }
            >
                {icon}
                <span>{name}</span>
            </NavLink>
        </>
    )
}