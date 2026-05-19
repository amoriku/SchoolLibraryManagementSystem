import EntityCrud from "../entity/EntityCrud"
import { GetAllBooks, type Book } from "../../api/entities/BookApi";

const BookCrud = () => {
    return (
        <>
            <EntityCrud<Book>
                fetchData={GetAllBooks}
                entityTitle="Раздел книг"
                columnNames={{
                    id: "ID",
                    receiptDate: "Дата поступления",
                    title: "Название",
                    // description: "Описание",
                    publishedYear: "Дата публикации",
                    price: "Цена"
                }}
            >

            </EntityCrud>
        </>
    )
}

export default BookCrud;