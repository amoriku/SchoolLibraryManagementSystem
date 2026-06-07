import { useEffect, useState } from "react"
import type { ReserveWithoutUserDto } from "../api/entities/Entity.Types"
import { useReserveService } from "../api/services/reservationService"
import { MainSectionHeader } from "../components/MainSectionHeader"
import { Table } from "../components/Table"
import { ActionsContainer } from "../components/ActionsContainer"
import { TableCancelButton } from "../components/buttons/TableCancelButton"
import toast from "react-hot-toast"
import { ConfirmModal } from "../components/ConfirmationModal"

export const MyReservationsPage = () => {
    const [activeReservations, setActiveReservations] = useState<ReserveWithoutUserDto[]>([])
    const [isConfirmModalOpen, setIsConfirmModalOpen] = useState<boolean>(false);
    const [selectedReserveId, setSelectedReserveId] = useState<number | null>();
    const { active, cancel } = useReserveService();

    const fetchData = async () => {
        setActiveReservations(await active());
    }

    const handleCancel = (reserveId: number) => {
        setIsConfirmModalOpen(true);
        setSelectedReserveId(reserveId);
    }

    const handleConfirmCancel = async () => {
        if (selectedReserveId == null) {
            return
        }

        try {
            await cancel(selectedReserveId);
            toast.success("Бронь отменена");
        }
        catch (error) {
            toast.error("При отмене брони произошла ошибка")
            setIsConfirmModalOpen(false);
            setSelectedReserveId(null)
        }
    }

    useEffect(() => {
        fetchData();
    }, [])

    const columnNames: string[] = [
        "Книга",
        "Дата брони",
    ]

    return (
        <>
            <ConfirmModal
                isOpen={isConfirmModalOpen}
                message="Вы уверены, что хотите отменить бронь?"
                onCancel={() => setIsConfirmModalOpen(false)}
                onConfirm={handleConfirmCancel}
                isDanger={true}
                title="Отмена брони"
            >

            </ConfirmModal>

            <MainSectionHeader title="Мои брони" desc="Раздел забронированных книг">

            </MainSectionHeader>
            <Table columnNames={columnNames}>
                {activeReservations.map(activeReservation => (
                    <tr className="table-tr" key={activeReservation.reserveId}>
                        <td
                            className="table-td"
                        >
                            {activeReservation.libraryItem}
                        </td>
                        <td
                            className="table-td"
                        >
                            {activeReservation.reservationDate 
                            ? new Date(activeReservation.reservationDate).toLocaleString() : "-"}
                        </td>
                        <ActionsContainer>
                            <TableCancelButton onCancel={() => handleCancel(activeReservation.reserveId)}></TableCancelButton>
                        </ActionsContainer>
                    </tr>
                ))}
            </Table>
        </>
    )
}