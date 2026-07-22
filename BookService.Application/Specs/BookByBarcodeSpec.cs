using Ardalis.Specification;

namespace BookService.Application.Specs
{
    public class BookByBarcodeSpec : Specification<Domain.Entities.Book>
    {
        public BookByBarcodeSpec(string barcode)
        {
            var b = (barcode ?? string.Empty).Trim();
            Query.Where(x => x.Barcode == b);
        }
    }
}
