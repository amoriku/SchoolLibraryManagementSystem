import { Table } from "../Table"

interface Props {
    columnNames: string[]
    title?: string,
    children?: React.ReactNode
}

export const DashboardTableCard = ({ columnNames, title, children }: Props) => {    
    return (
        <div>
            <Table columnNames={columnNames} title={title}>     
                {children}
            </Table>
        </div>
    )
}