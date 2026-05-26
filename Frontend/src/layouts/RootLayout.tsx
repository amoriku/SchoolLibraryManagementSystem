import { Outlet } from "react-router"
import { Header } from "../components/header/Header"

export const RootLayout = () => {
    
    return (
        <> 
            <Header></Header>

            <div>
                <Outlet></Outlet>
            </div>
        </>
    )
}