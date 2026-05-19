import { useState } from "react";
import { Login } from "../../api/auth/Auth";
import { useNavigate } from "react-router";
import HLine from "../../utils/components/HLine";
import { BaseInput } from "../../components/CustomInput";
import { useAuth } from "../../hooks/useAuth";

export default function SignIn() {
    const [formData, setFormData] = useState({
        identifier: "",
        password: ""
    })
    const [isLoading, setIsLoading] = useState<boolean>(true);

    const { login } = useAuth();
    const navigate = useNavigate();

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }))
    }

    async function handleSignIn(e: React.SubmitEvent) {
        e.preventDefault();
        const identifier: string = formData.identifier;
        const password: string = formData.password;
        
        setIsLoading(true)
        try{
            await login({identifier, password});
            navigate("/home");
        }
        catch(error)
        {
            console.error(error);
        }
        finally{
            setIsLoading(false);
        }

    }

    return (
        <>
            <div className="flex items-center justify-center h-screen text-white bg-slate-800">
                <div className="flex flex-col gap-4 border-2 rounded-2xl p-6 bg-indigo-700 border-slate-500 w-80 max-w-96">
                    <div className="flex flex-col">
                        <h2 className="mb-3 text-center text-2xl">Вход в систему</h2>
                        <HLine></HLine>
                    </div>
                    <form className="sign-up-form" onSubmit={handleSignIn}>
                        <div className="sign-up-form__container flex flex-col items-center justify-center gap-2">
                            <BaseInput
                                type="text"
                                name="identifier"
                                placeholder="Никнейм или почта"
                                required={true}
                                value={formData.identifier}
                                onChange={handleInputChange}
                            >

                            </BaseInput>
                            <BaseInput
                                type="password"
                                name="password"
                                placeholder="Пароль"
                                required={true}
                                value={formData.password}
                                onChange={handleInputChange}>

                            </BaseInput>
                            <button className="w-full border p-2 bg-slate-500 rounded-lg hover:bg-slate-600 transition-all ">Войти</button>
                        </div>
                    </form>
                    {/* <div className="flex flex-col text-center">
                        <HLine></HLine>
                        <span>Еще нет аккаунта? <Link to={"/sign-up"}>Зарегистрироваться</Link> </span>
                    </div> */}
                </div>
            </div>
        </>
    )
}