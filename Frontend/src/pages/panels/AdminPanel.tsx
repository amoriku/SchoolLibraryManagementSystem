import { EntityCard } from "../../components/entity/EntityCard";
import { NavigationButton } from "../../utils/components/NavigationButton";

export default function AdminPanel() {
    return (
        <>
            <div className="mt-4 flex flex-col gap-2 items-center justify-center">
                {/* <h1 className="text-center uppercase text-2xl">Панель администратора</h1> */}
                <NavigationButton navigateTo="/home" linkName="Вернуться на главную"></NavigationButton>
            </div>
            <div className="flex items-center justify-center h-screen">
                <EntityCard title="Авторы" navigateTo="/admin/authors"></EntityCard>
                <EntityCard title="Книги" navigateTo="/admin/books"></EntityCard>
                <EntityCard title="Фонды" navigateTo="/admin"></EntityCard>
                <EntityCard title="Классы" navigateTo="/admin"></EntityCard>
            </div>
        </>
    )
}