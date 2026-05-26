import type { UserRole } from "../api/auth/Auth.Types"
import { useAuth } from "../hooks/useAuth"

interface RoleBasedProps{
    children?: React.ReactNode,
    role: UserRole
    fallback?: React.ReactNode
}

export const RoleBased = ({children, role, fallback}: RoleBasedProps) =>{
    const { user } = useAuth();
    // console.log(user);

    // if (user === null || !user.role) {
    //     return <>{fallback}</>
    // }

    const hasAccess = role === user?.role;
    // console.log("Has access: ", hasAccess);

    return hasAccess ? <>{children}</> : <>{fallback}</>
} 