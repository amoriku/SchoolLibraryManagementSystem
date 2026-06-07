import { Table } from "../Table"

interface Props {
    columnNames: string[]
    title?: string,
    children?: React.ReactNode,
    includeOperations?: boolean
}

export const DashboardTableCard = ({ columnNames, title, children, includeOperations = true }: Props) => {
    return (
        <div>
            <Table
                columnNames={columnNames}
                title={title}
                includeOperations={includeOperations}
            >
                {children}
            </Table>
        </div>
    )
}