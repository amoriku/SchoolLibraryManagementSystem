import { FiLogOut, FiLayout, FiUsers } from "react-icons/fi";
import { useAuth } from "../hooks/useAuth";
import { SideBarItem } from "./SideBarItem";
import { RoleBased } from "./RoleBased";
import { Outlet, useOutletContext } from "react-router";
import { FaBookOpen, FaUserPlus } from "react-icons/fa";
import { CgGlass } from "react-icons/cg";
import { GrCatalog, GrUser } from "react-icons/gr";
import { BiBookContent, BiBookmark } from "react-icons/bi";
import { CatalogPage } from "../pages/CatalogPage";


interface RootContextType {
    filterResult: string
}

export default function Hero() {
    const authContext = useAuth();

    const { filterResult } = (useOutletContext<RootContextType>() || {}) as RootContextType;

    return (
        <>
            {authContext.user ? (
                <div className="w-full min-h-[calc(100dvh-76px)] md:h-[calc(100dvh-76px)] md:overflow-hidden bg-slate-50">
                    <div className="flex flex-col md:flex-row h-full w-full">
                        <aside className="hidden md:flex flex-col justify-between h-full w-64 min-w-64 max-w-64 bg-white py-6 pl-4 pr-4 border-r border-slate-100 shrink-0">
                            <nav className="flex flex-col gap-1">
                                <RoleBased role="Admin">
                                    <SideBarItem name="Дашборд" icon={<FiLayout />} to="/admin/dashboard" />
                                    <SideBarItem name="Пользователи" icon={<FiUsers />} to="/admin/users" />
                                    <SideBarItem name="Классы" icon={<CgGlass />} to="/admin/classes" />
                                </RoleBased>

                                <RoleBased role="Librarian">
                                    <SideBarItem name="Дашборд" icon={<FiLayout />} to="/librarian/dashboard"></SideBarItem>
                                    {/* <SideBarItem name="Каталог" icon={<GrCatalog />} to="/catalogue"></SideBarItem> */}
                                    <SideBarItem name="Книги" icon={<FaBookOpen />} to="/librarian/books"></SideBarItem>
                                    <SideBarItem name="Читатели" icon={<GrUser />} to="/librarian/readers"></SideBarItem>
                                    <SideBarItem name="Авторы" icon={<FaUserPlus />} to="/librarian/authors"></SideBarItem>
                                </RoleBased>

                                <RoleBased role="Reader">
                                    <SideBarItem name="Каталог" icon={<GrCatalog />} to="/catalogue"></SideBarItem>
                                    <SideBarItem name="Мои книги" icon={<BiBookContent />} to="/reader/my-books"></SideBarItem>
                                    <SideBarItem name="Мои брони" icon={<BiBookmark />} to="/reader/my-reservations"></SideBarItem>
                                </RoleBased>
                            </nav>

                            <div className="pt-4 px-4 border-t border-slate-100">
                                <button onClick={authContext.logout} className="flex items-center gap-3 w-full text-slate-500 hover:text-red-500 py-3 rounded-lg font-medium transition-colors">
                                    <FiLogOut className="text-xl" />
                                    <span>Выйти из аккаунта</span>
                                </button>
                            </div>
                        </aside>

                        <div className="md:hidden fixed bottom-0 left-0 right-0 bg-white border-t border-slate-200 z-50 flex justify-around py-2">
                            <nav className="flex justify-around items-center w-full">
                                <RoleBased role="Admin">
                                    <SideBarItem name="Дашборд" icon={<FiLayout />} to="/admin/dashboard" />
                                    <SideBarItem name="Пользователи" icon={<FiUsers />} to="/admin/users" />
                                    <SideBarItem name="Классы" icon={<CgGlass />} to="/admin/classes" />
                                </RoleBased>

                                <RoleBased role="Librarian">
                                    <SideBarItem name="Дашборд" icon={<FiLayout />} to="/librarian/dashboard"></SideBarItem>
                                    <SideBarItem name="Книги" icon={<FaBookOpen />} to="/librarian/books"></SideBarItem>
                                    <SideBarItem name="Читатели" icon={<GrUser />} to="/librarian/readers"></SideBarItem>
                                    <SideBarItem name="Авторы" icon={<FaUserPlus />} to="/librarian/authors"></SideBarItem>
                                </RoleBased>

                                <RoleBased role="Reader">
                                    <SideBarItem name="Каталог" icon={<GrCatalog />} to="/catalogue"></SideBarItem>
                                    <SideBarItem name="Мои книги" icon={<BiBookContent />} to="/reader/my-books"></SideBarItem>
                                    <SideBarItem name="Мои брони" icon={<BiBookmark />} to="/reader/my-reservations"></SideBarItem>
                                </RoleBased>
                            </nav>
                        </div>


                        <main className="flex-1 w-full p-4 md:p-6 md:pl-12 h-full overflow-y-auto pb-20 md:pb-6">
                            <Outlet context={filterResult}></Outlet>
                        </main>

                    </div>
                </div>
            ) : (
                <div>
                    <CatalogPage filterResult={filterResult}></CatalogPage>
                </div>
            )}
        </>
    );
}
