import { Link } from "react-router"

interface NavigationProps{
    navigateTo: string,
    linkName: string
}

export const NavigationButton = ({navigateTo, linkName}: NavigationProps) => {
    return (
        <button className="hover:border hover:p-2 hover:rounded hover:bg-emerald-400 transition-all"><Link to={navigateTo}>{linkName}</Link></button>
    )
}