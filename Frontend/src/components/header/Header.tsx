import { Link } from "react-router";

export function Header() {
    return (
        <header className="bg-indigo-700 text-white shadow">
            <div className="container flex flex-row items-center justify-between">
                <h1 className="text-2xl p-4 header-logo">
                    Школьная библиотека
                </h1>
                <nav>
                    <ul className="flex gap-5 text-xl">
                        <li className="hover:underline hover:underline-offset-4 transition-all">
                            <Link to="/sign-in">Войти</Link>
                        </li>
                        {/* <li className="hover:underline hover:underline-offset-4 transition-all">
                                <Link to="/sign-up">Зарегистрироваться</Link>
                            </li> P.S. Maybe in future*/}
                    </ul>
                </nav>
            </div>
        </header>
    )
}