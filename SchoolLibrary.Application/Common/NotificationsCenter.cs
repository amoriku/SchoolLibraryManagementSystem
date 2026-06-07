using SchoolLibrary.Domain.Entities;

namespace SchoolLibrary.Application.Common
{
    public static class NotificationsCenter
    {
        // Проверка на то, просрочена ли книга
        public static bool IsOverdue(this Borrowing borrowing)
        {
            return borrowing.ReturnedAt == null 
                && borrowing.DueDate < DateTime.UtcNow;
        }

        public static string GetOverdueMessage(string bookTitle, DateTime dueDate)
        {
            var days = (dueDate - DateTime.UtcNow).Days;
            return $"У книги {bookTitle} истек срок возврата от {dueDate}. Задолженность: {days} дн.";
        }
    }
}
