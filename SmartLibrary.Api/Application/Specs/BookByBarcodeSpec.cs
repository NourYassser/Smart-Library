using Ardalis.Specification;
using SmartLibrary.Api.Domain.Entities;

namespace SmartLibrary.Api.Application.Specs
{
    public class BookByBarcodeSpec : Specification<Book>
    {
        public BookByBarcodeSpec(string barcode)
        {
            var b = (barcode ?? string.Empty).Trim();
            Query.Where(x => x.Barcode == b);
        }
    }
}
