using QuestPDF.Fluent;
using TecnoFix.Src.Model;
using TecnoFix.Src.Services.Interfaces;

namespace TecnoFix.Src.Services;

public class BitacoraPdfService : IBitacoraPdfService
{
    public byte[] Generar(OrdenReparacion orden)
{
    return Document.Create(doc => doc.Page(page =>
    {
        page.Margin(40);
        page.Header().Text($"Bitácora ORD-{orden.Id:D6}").FontSize(18).Bold();
        page.Content().Column(col =>
        {
            col.Item().Text($"Cliente: {orden.Cliente.Name}");
            col.Item().Text($"Equipo: {orden.TipoEquipo} {orden.Marca} {orden.Modelo}");
            col.Item().PaddingTop(12).Table(t =>
            {
                t.ColumnsDefinition(c =>
                {
                    c.ConstantColumn(100); c.ConstantColumn(130);
                    c.ConstantColumn(110); c.RelativeColumn();
                });
                t.Header(h =>
                {
                    h.Cell().Text("Fecha").Bold();
                    h.Cell().Text("Responsable").Bold();
                    h.Cell().Text("Estado").Bold();
                    h.Cell().Text("Comentario").Bold();
                });
                foreach (var e in orden.Bitacora.OrderBy(b => b.FechaHora))
                {
                    t.Cell().Text(e.FechaHora.ToLocalTime().ToString("dd/MM/yyyy HH:mm"));
                    t.Cell().Text(e.Responsable.Name);
                    t.Cell().Text(e.EstadoNuevo.ToString());
                    t.Cell().Text(e.Comentario);
                }
            });
        });
    })).GeneratePdf();
}
}