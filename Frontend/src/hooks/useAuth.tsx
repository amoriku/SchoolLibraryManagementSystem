import { createContext, useContext, useEffect, useState, type ReactNode } from "react";
import { getCurrentUser, Login, Logout } from "../api/auth/Auth";
import type { User, LoginResponse, UserRole } from "../api/auth/Auth.Types";

interface AuthContextType {
    user: User | null,
    role: UserRole | null,
    isLoading: boolean,
    login: ({ identifier, password }: { identifier: string, password: string }) => Promise<LoginResponse>,
    logout: () => void,
    isAuthenticated: boolean,
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
    const [user, setUser] = useState<User | null>(null);
    const [isLoading, setIsLoading] = useState<boolean>(true);

    useEffect(() => {
        const checkUser = async () => {
            try {
                const userData = await getCurrentUser();
                if (userData) {
                    setUser(userData);
                }
            }
            catch (error) {
                console.error(`Auth error \n${error}`);
            }
            finally {
                setIsLoading(false);
            } 
        }
        checkUser();
    }, [])

    const login = async ({ identifier, password }: { identifier: string, password: string }) => {
        const response = await Login(identifier, password);
        if (response) {
            const userData = await getCurrentUser();
            if (userData) {
                setUser(userData);
            }
        }

        return response;
    }

    const logout = async () => {
        await Logout();
        setUser(null);
    }


    if (isLoading){
        return
    }

    return (
        <AuthContext.Provider value={{
            user,
            role: user?.role || null,
            isLoading,
            login,
            logout,
            isAuthenticated: !!user,
        }}>
            {children}
        </AuthContext.Provider>
    )
}

export const useAuth = () => {
    const context = useContext(AuthContext)
    if (context === undefined) {
        throw new Error('useAuth must be used within AuthProvider');
    }
    return context;
}