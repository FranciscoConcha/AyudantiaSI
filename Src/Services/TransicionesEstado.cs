using TecnoFix.Src.Model;

namespace TecnoFix.Src.Services;
/// <summary>
/// Clase que representa un diccionario para pasar las etapadas de cada orden.
/// </summary>
public static class TransicionesEstado
{

    private static readonly Dictionary<EstadoOrden, EstadoOrden[]> Permitidas = new()
    {
        { EstadoOrden.Recibida,        new[] { EstadoOrden.EnDiagnostico } },
        { EstadoOrden.EnDiagnostico,   new[] { EstadoOrden.Presupuestada } },
        { EstadoOrden.EnReparacion,    new[] { EstadoOrden.ListaParaRetiro } },
        { EstadoOrden.ListaParaRetiro, new[] { EstadoOrden.Entregada } },
        { EstadoOrden.Rechazada,       new[] { EstadoOrden.Entregada } },
    };

    public static EstadoOrden[] Siguientes(EstadoOrden actual) =>
        Permitidas.TryGetValue(actual, out var lista) ? lista : Array.Empty<EstadoOrden>();

    public static bool EsValida(EstadoOrden actual, EstadoOrden nuevo) =>
        Siguientes(actual).Contains(nuevo);
}