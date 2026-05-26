import { useEffect, useState } from "react"
import { type UserDto } from "../../api/entities/Entity.Types";
import { DashboardSimpleCard } from "./DashboardCard";
import { useData } from "../../hooks/useData";

export const AdminDashboard = () => {
    const { request } = useData();
    const [users, setUsers] = useState<UserDto[]>([]);

    const fetchUsers = async () => {
        const users = await request<UserDto[]>({
            endpoint: "/Users",
            method: "get",
            type: "Auth"
        })
        setUsers(users);
    }

    useEffect(() => {
        fetchUsers()
    }, [])

    return (
        <>
            <DashboardSimpleCard title="Всего пользователей" data={users.length}></DashboardSimpleCard>
        </>
    )
}