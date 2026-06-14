import { useState } from "react";
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
        try {
            await login({ identifier, password });
            navigate("/home");
        }
        catch (error) {
            console.error(error);
        }
        finally {
            setIsLoading(false);
        }

    }

    return (
        <>
            <div className="flex items-center justify-center h-screen bg-slate-100">
                <div className="flex flex-col gap-4 max-md:gap-2 border shadow rounded-2xl p-6 bg-white border-my-light-green w-80 max-w-96 max-md:w-64">
                    <div className="flex flex-col">
                        <h2 className="text-center text-2xl max-md:text-xl font-semibold">Вход в систему</h2>
                    </div>
                    <div className="mt-6">
                        <form className="" onSubmit={handleSignIn}>
                            <div className="flex flex-col gap-2">
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
                                <button 
                                    type="submit"
                                    className="mt-6 w-full border border-slate-400 p-2 text-xl max-md:text-[0.8rem] rounded-lg hover:text-slate-50 hover:border-slate-600 hover:bg-my-light-green/80 transition-all ">Войти</button>
                            </div>
                        </form>
                    </div>
                    {/* <div className="flex flex-col text-center">
                        <HLine></HLine>
                        <span>Еще нет аккаунта? <Link to={"/sign-up"}>Зарегистрироваться</Link> </span>
                    </div> */}
                </div>
            </div>
        </>
    )
}