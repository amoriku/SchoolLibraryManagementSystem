// import { useEffect, useState } from "react";
// import type { BookDto, CreateBorrowingDirectDto, CreateBorrowingDto } from "../../api/entities/Entity.Types";
// import { useForm } from "../../hooks/useForm";
// import { BaseInput } from "../CustomInput";
// import { useDataService } from "../../api/services/dataService";

// export const BorrowBookForm = () => {
//     const [books, setBooks] = useState<BookDto[]>([]);
//     const { getAllBooks } = useDataService();

//     const fetchBooks = async () => {
//         setBooks(await getAllBooks());
//     }

//     useEffect(() => {
//         fetchBooks();
//     }, [])

//     const { values, handleChange, resetForm } = useForm<CreateBorrowingDirectDto>({
        
//     });

//     const handleBorrow = () => {

//     }

//     return (
//         <>
//             <form
//                 action="post"
//                 onSubmit={handleBorrow}
//                 id="borrow-book-form"
//                 className="create-form"
//                 autoComplete="off"
//             >
//                 <BaseInput
//                     value={values.lastName}
//                     required={true}
//                     name="lastName"
//                     onChange={handleChange}
//                     placeholder="Введите фамилию*"
//                 >

//                 </BaseInput>

//                 <BaseInput
//                     value={values.firstName}
//                     required={true}
//                     name="firstName"
//                     onChange={handleChange}
//                     placeholder="Введите имя*"
//                 >

//                 </BaseInput>

//                 <BaseInput
//                     value={values.middleName}
//                     required={false}
//                     name="middleName"
//                     onChange={handleChange}
//                     placeholder="Введите отчество"
//                 >

//                 </BaseInput>
//             </form>
//         </>
//     )
// }