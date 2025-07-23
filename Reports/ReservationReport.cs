using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using radio_waves.Models;

namespace radio_waves.Reports
{
    public class ReservationReport : IDocument
    {
        private readonly List<Reservation> _reservations;
        private readonly string? _searchString;
        private readonly DateTime? _fromDate;
        private readonly DateTime? _toDate;

        public ReservationReport(List<Reservation> reservations, string? searchString, DateTime? fromDate, DateTime? toDate)
        {
            _reservations = reservations;
            _searchString = searchString;
            _fromDate = fromDate;
            _toDate = toDate;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(20);
                page.Size(PageSizes.A4);

                page.Header().Column(col =>
                {
                    col.Item()
                        .Text("AFA Lab")
                        .FontSize(20)
                        .Bold()
                        .AlignCenter();
                    col.Item()
                       .Text("Reservation Report")
                       .FontSize(20)
                       .Bold()
                       .AlignCenter();

                    col.Item().PaddingTop(10).Text(text =>
                    {
                        text.AlignLeft();

                        text.Span("Filters: ").Bold();

                        if (!string.IsNullOrEmpty(_searchString))
                            text.Span($"Name contains '{_searchString}'  ");

                        if (_fromDate.HasValue)
                            text.Span($"From {_fromDate:yyyy-MM-dd}  ");

                        if (_toDate.HasValue)
                            text.Span($"To {_toDate:yyyy-MM-dd}");
                    });
                });

                static IContainer CellStyle(IContainer container) =>
    container.Border(1)
             .BorderColor(Colors.Grey.Darken1)
             .Padding(5)
             .AlignMiddle()
             .AlignCenter();

                page.Content().PaddingTop(15).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(30); // #
                        columns.RelativeColumn(2);  // Patient
                        columns.RelativeColumn(2);  // Type
                        columns.RelativeColumn(2);  // Technician
                        columns.RelativeColumn(2);  // PaiedAmount
                        columns.RelativeColumn(2);  // Date
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("#").Bold();
                        header.Cell().Element(CellStyle).Text("Patient").Bold();
                        header.Cell().Element(CellStyle).Text("Type").Bold();
                        header.Cell().Element(CellStyle).Text("Technician").Bold();
                        header.Cell().Element(CellStyle).Text("Paid Amount").Bold();
                        header.Cell().Element(CellStyle).Text("Date").Bold();
                    });

                    int index = 1;
                    foreach (var r in _reservations)
                    {
                        table.Cell().Element(CellStyle).Text(index++.ToString());
                        table.Cell().Element(CellStyle).Text(r.Patient?.FullName ?? "-");
                        table.Cell().Element(CellStyle).Text(r.RadiologyType?.Name ?? "-");
                        table.Cell().Element(CellStyle).Text(r.Technician?.FullName ?? "-");
                        table.Cell().Element(CellStyle).Text(r.PaiedAmount.ToString() ?? "-");
                        table.Cell().Element(CellStyle).Text(r.AppointmentDate.ToString("yyyy-MM-dd"));
                    }
                });

                page.Footer()
                    .AlignCenter()
                    .Text("Made by AFA")
                    .FontSize(10)
                    .Italic()
                    .FontColor(Colors.Grey.Darken1);
            });
        }
    }
}
