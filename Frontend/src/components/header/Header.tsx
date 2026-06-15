import { Link } from "react-router";
import { useAuth } from "../../hooks/useAuth";
import { FaAngleDown, FaAngleUp } from "react-icons/fa";
import { useState, type InputHTMLAttributes } from "react";
import { FiBell, FiBookOpen, FiLogOut } from "react-icons/fi";
import { BaseInput, SearchInput } from "../CustomInput";


export function Header({onSearchChange}: {onSearchChange?: (value: string) => void}) {
    const authContext = useAuth();
    const [profileOpen, setProfileOpen] = useState(false);
    const [notificationsOpen, setNotificationsOpen] = useState<boolean>(false);
    const [hasNotifications, setHasNotifications] = useState<boolean>(false);

    return (
        <header className="bg-white border-b border-slate-100 px-6 py-5 w-full shrink-0">
            <div className="flex items-center text-2xl gap-8 max-md:grid max-md:gap-4">
                <div className=" font-bold text-slate-800">
                    <Link className="flex items-center max-md:justify-center gap-3" to="/home">
                        <FiBookOpen className=" text-slate-600 shrink-0" />
                        <h1 className="text-center">Библиотека</h1>
                    </Link>
                </div>

                <div className="flex justify-between items-center w-full">
                    <div className="">
                        {(!authContext.isAuthenticated) && (
                            <BaseInput
                                onChange={(e) => onSearchChange === undefined ? console.log() : onSearchChange(e.target.value)}
                                placeholder="Введите название...">

                            </BaseInput>
                        )}
                    </div>


                    {!authContext.user ? (
                        <div className="hover:bg-my-light-green hover:text-white px-4 transition-colors rounded">
                            <Link to="/login">Войти</Link>
                        </div>
                    ) : (
                        <div className="flex items-center gap-6 text-base font-medium relative max-md:justify-center w-full justify-end">

                            <div className="relative">
                                <button
                                    onClick={() => {
                                        setNotificationsOpen(!notificationsOpen);
                                        setProfileOpen(false); // Закрываем профиль, если открыли колокольчик
                                    }}
                                    className="p-2 text-slate-500 hover:text-slate-800 transition-colors relative flex items-center justify-center rounded-lg hover:bg-slate-50"
                                    title="Уведомления"
                                >
                                    <FiBell className="text-2xl" />
                                    {hasNotifications && (
                                        <span className="absolute top-1.5 right-1.5 w-2.5 h-2.5 bg-rose-500 rounded-full ring-2 ring-white animate-pulse" />
                                    )}
                                </button>

                                {notificationsOpen && (
                                    <div className="absolute right-0 top-12 max-md:w-56 max-md:p-3 max-md:-right-24 w-80 bg-white rounded-xl shadow-xl border border-slate-150 p-4 flex flex-col gap-2 z-50 text-sm">
                                        <h3 className="font-bold text-slate-850 border-b border-slate-100 pb-2 mb-1">Уведомления</h3>
                                    </div>
                                )}
                            </div>

                            <div
                                className="flex items-center gap-2 justify-center select-none"
                                onClick={() => {
                                    setProfileOpen(!profileOpen);
                                    setNotificationsOpen(false);
                                }}
                            >
                                <p className="text-xl text-slate-700 hover:text-slate-900 transition-colors font-medium">{authContext.user.username}</p>
                                {!profileOpen ? <FaAngleDown className="text-slate-400" /> : <FaAngleUp className="text-slate-400" />}

                                {profileOpen && (
                                    <div className="absolute right-0 top-12 max-md:w-32 max-md:right-24 w-48 bg-white rounded-xl shadow-xl border border-slate-150 p-2 z-50">
                                        <button
                                            onClick={authContext.logout}
                                            className="flex items-center gap-3 w-full text-slate-600 hover:text-red-600 hover:bg-red-50 p-2.5 rounded-lg text-sm font-medium transition-all"
                                        >
                                            <FiLogOut className="text-xl" />
                                            <span>Выйти</span>
                                        </button>
                                    </div>
                                )}
                            </div>
                        </div>
                    )}
                </div>

            </div>
        </header>
    );
}
