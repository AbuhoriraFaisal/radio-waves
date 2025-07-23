using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using radio_waves.Models;

namespace radio_waves.Reports
{
    public class InsuranceReport : IDocument
    {
        private readonly List<Insurance> _insurances;
        private readonly string? _filter;

        public InsuranceReport(List<Insurance> insurances, string? filter = null)
        {
            _insurances = insurances;
            _filter = filter;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(20);
                page.Size(PageSizes.A4);

                // Header
                page.Header().Column(col =>
                {
                    col.Item().Text("AFA Lab - Insurance Report")
                        .FontSize(20)
                        .Bold()
                        .AlignCenter();

                    if (!string.IsNullOrWhiteSpace(_filter))
                    {
                        col.Item().PaddingTop(5).Text($"Filter: {_filter}")
                            .FontSize(10)
                            .AlignLeft();
                    }
                });

                // Table content
                page.Content().PaddingTop(15).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(30); // #
                        columns.RelativeColumn(2);  // Patient
                        columns.RelativeColumn();   // Policy #
                        columns.RelativeColumn();   // Paid Amount
                        columns.RelativeColumn();   // Insurance Amount
                        columns.RelativeColumn();   // Technician Share
                        columns.RelativeColumn();   // Is Complete
                        columns.RelativeColumn();   // Is Shared
                    });

                    // Header
                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("#").Bold();
                        header.Cell().Element(CellStyle).Text("Patient").Bold();
                        header.Cell().Element(CellStyle).Text("Policy #").Bold();
                        header.Cell().Element(CellStyle).Text("Paid").Bold();
                        header.Cell().Element(CellStyle).Text("Insurance").Bold();
                        header.Cell().Element(CellStyle).Text("Tech Share").Bold();
                        header.Cell().Element(CellStyle).Text("Complete").Bold();
                        header.Cell().Element(CellStyle).Text("Shared").Bold();
                    });

                    int index = 1;
                    foreach (var i in _insurances)
                    {
                        table.Cell().Element(CellStyle).Text((index++).ToString());
                        table.Cell().Element(CellStyle).Text(i.Patient?.FullName ?? "-");
                        table.Cell().Element(CellStyle).Text(i.PolicyNumber ?? "-");
                        table.Cell().Element(CellStyle).Text($"{i.PaidAmount:C}");
                        table.Cell().Element(CellStyle).Text($"{i.InsuranceAmount:C}");
                        table.Cell().Element(CellStyle).Text($"{i.TechnicianShare:C}");
                        table.Cell().Element(CellStyle).Text(i.IsComplete ? "✔" : "✘");
                        table.Cell().Element(CellStyle).Text(i.IsTechnicianShared ? "✔" : "✘");
                    }

                    // Style for table cells
                    IContainer CellStyle(IContainer container) =>
                        container.Border(0.5f)
                                 .BorderColor(Colors.Grey.Lighten2)
                                 .Padding(5);
                });

                // Footer
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
