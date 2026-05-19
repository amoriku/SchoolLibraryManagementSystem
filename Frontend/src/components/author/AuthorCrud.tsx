import type { AuthorDto } from "../../api/entities/AuthorApi";
import GetAllAuthors from "../../api/entities/AuthorApi";
import EntityCrud from "../entity/EntityCrud";

const AuthorCrud = () => {
    return (
        <>
            <EntityCrud<AuthorDto>
                fetchData={GetAllAuthors}
                entityTitle="Авторы"
                columnNames={{
                    id: "ID",
                    firstName: "Имя",
                    lastName: "Фамилия",
                    middleName: "Отчество"
                }}
            >

            </EntityCrud>
        </>
    )
}

export default AuthorCrud;