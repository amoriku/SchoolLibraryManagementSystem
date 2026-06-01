export const ActionsContainer = ({children}: {children?: React.ReactNode}) => {
    return (
        <td className="table-td table-td-actions">
            <div className="actions-container">
                {children}
            </div>
        </td>
    )
}