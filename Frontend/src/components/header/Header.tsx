import { Link } from "react-router";
import { useAuth } from "../../hooks/useAuth";
import { FaAngleDown, FaAngleUp } from "react-icons/fa";
import { useState } from "react";
import { FiBookOpen, FiLogOut } from "react-icons/fi";
import { CgProfile } from "react-icons/cg";
import { BaseInput, SearchInput } from "../CustomInput";
import toast from "react-hot-toast";

export function Header() {
    const authContext = useAuth();
    const [profileOpen, setProfileOpen] = useState(false);

    const handleBookSearch = () => {
        toast.error("Временно не работает", {
            position: "top-right"
        })
    }

    return (
        <header className="bg-white border-b border-slate-100 px-6 py-5 w-full shrink-0">
            <div className="flex items-center w-full text-2xl">

                <div className="w-64 min-w-64 max-w-64 font-bold text-slate-800">
                    <Link className="flex items-center gap-3" to="/home">
                        <FiBookOpen className=" text-slate-600 shrink-0" />
                        <h1>Библиотека</h1>
                    </Link>
                </div>

                <div className="flex-1 flex justify-between items-center pl-6">
                    {/* TO-DO: Make this work */}
                    {!authContext.isAuthenticated ? (
                        <SearchInput onSearch={handleBookSearch} placeholder="Введите название..."></SearchInput>
                    ) : (
                        <button className="text-slate-400 flex items-center justify-center gap-1 hover:text-slate-400/80 transition-colors font-semibold">
                            <CgProfile></CgProfile>
                            <span className="text-xl">Профиль</span>
                        </button>
                    )}


                    {!authContext.user ? (
                        <div className="hover:bg-my-light-green hover:text-white px-4 transition-colors rounded">
                            <Link to="/login">Войти</Link>
                        </div>
                    ) : (
                        <div
                            className="flex items-center gap-2 justify-center"
                            style={{ cursor: "pointer" }}
                            title="Открыть профиль пользователя"
                            onClick={() => setProfileOpen(!profileOpen)}
                        >
                            <p className="">{authContext.user.username}</p>
                            {profileOpen && (
                                <div className="px-4 fixed top-16 right-0">
                                    <button onClick={authContext.logout} className="flex items-center gap-3 w-full text-slate-500 hover:text-red-500 py-3 rounded-lg font-medium transition-colors">
                                        <FiLogOut className="text-xl" />
                                        <span>Выйти из аккаунта</span>
                                    </button>
                                </div>
                            )}
                            {!profileOpen ? <FaAngleDown className="text-slate-400" /> : <FaAngleUp className="text-slate-400" />}
                        </div>
                    )}
                </div>

            </div>
        </header>
    );
}
