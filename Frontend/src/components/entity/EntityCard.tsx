import { Link } from "react-router";

interface EntityProps {
    title: string,
    navigateTo: string;
}

export const EntityCard = ({ title, navigateTo }: EntityProps) => {
    return (
        <>
            <div className="">
                <button>
                    <Link to={navigateTo}>
                        <div className="ml-1 mr-1 p-4 border-2 bg-slate-500 rounded border-indigo-500 hover:border-slate-600 hover:bg-indigo-700 hover:text-white transition-all">
                            <h3 className="text-center mb-2">{title}</h3>
                            <span>Редактировать</span>
                        </div>
                    </Link>
                </button>

            </div>
        </>
    )
}