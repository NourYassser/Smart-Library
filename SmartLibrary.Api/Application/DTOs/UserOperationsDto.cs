namespace SmartLibrary.Api.Application.DTOs
{
    public record UserOperationsDto(List<BorrowingDto> Borrowings, List<FineDto> Fines);
    public record BorrowingDto(Guid Id, Guid BookId, string BookTitle, DateTime BorrowedOn, DateTime? ReturnedOn);
    public record FineDto(Guid Id, decimal Amount, DateTime IssuedOn, DateTime? PaidOn);
}
