namespace SchoolLibrary.Domain
{
    public enum OperationType
    {
        None = 0,

        Loan = 1,
        Return = 2,
        Borrowing = 3,
        Reserve = 4,
        ReserveCancel = 5,
        ReadyForPickup = 6,
        ReserveExpired = 7,
    }
}
