import { Outlet } from "react-router"
import { Header } from "../components/header/Header"
import { useState } from "react"

export const RootLayout = () => {
    const [searchQuery, setSearchQuery] = useState<string>("");

    return (
        <> 
            <Header onSearchChange={setSearchQuery}>

            </Header>

            <div>
                <Outlet context={{filterResult: searchQuery}}></Outlet>
            </div>
        </>
    )
}