import { useState } from "react"
import { Link } from "react-router";
import HLine from "../../utils/components/HLine";
import { BaseInput } from "../../components/CustomInput";


export default function SignUp() {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [email, setEmail] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");

    function HandleSignUp(e: React.SubmitEvent) {
        e.preventDefault();
        if (confirmPassword != password) {
            alert("Пароли не совпадают");
        }
    }

    return (
        <>
            <div className="flex items-center justify-center h-screen text-white bg-slate-800">
                <div className="flex flex-col gap-4 border-2 rounded-2xl p-6 bg-indigo-700 border-slate-500 w-80 max-w-96">
                    <div className="flex flex-col">
                        <h2 className="text-center text-2xl mb-2 ">Регистрация в системе</h2>
                        <HLine></HLine>
                    </div>
                    <form className="sign-up-form" onSubmit={HandleSignUp}>
                        <div className="sign-up-form__container flex flex-col items-center justify-center gap-2">
                            <BaseInput
                                type="text"
                                placeholder="Никнейм*"
                                value={username}
                                required={true}
                                onChange={setUsername}
                            >
                            </BaseInput>
                            <BaseInput
                                type="password"
                                placeholder="Пароль*"
                                value={password}
                                required={true}
                                onChange={setPassword}
                            >
                            </BaseInput>
                            <BaseInput
                                type="password"
                                placeholder="Подтвердите пароль*"
                                value={confirmPassword}
                                required={true}
                                onChange={setConfirmPassword}
                            >
                            </BaseInput>
                            <BaseInput
                                type="email"
                                placeholder="Почта"
                                value={email}
                                required={false}
                                onChange={setEmail}
                            >
                            </BaseInput>
                            <button className="w-full border p-2 bg-slate-500 rounded-lg hover:bg-slate-600 transition-all">Зарегистрироваться</button>
                        </div>
                    </form>
                    <div className="flex flex-col text-center">
                        <HLine></HLine>
                        <span>Уже есть аккаунт? <Link to={"/sign-in"}>Войти</Link> </span>
                    </div>
                </div>
            </div>
        </>
    )
}