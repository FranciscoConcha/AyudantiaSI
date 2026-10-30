using TecnoFix.Src.Model;

namespace TecnoFix.Src.Services.Interfaces;

public interface IBitacoraPdfService
{
    byte[] Generar(OrdenReparacion orden);
}