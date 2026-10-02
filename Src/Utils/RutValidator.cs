using System.Text.RegularExpressions;

namespace TecnoFix.Src.Utils;

public static class ValidatorRut
{
    public static string? ValidarRut(string rutCrudo)
    {
        var limpio = Regex.Replace(rutCrudo, "[^0-9kK]", "").ToUpperInvariant();
        if (limpio.Length < 2) return null;

        var cuerpo = limpio[..^1];
        var dvIngresado = limpio[^1];

        if (!long.TryParse(cuerpo, out _)) return null;

        int suma = 0, multiplicador = 2;
        for (int i = cuerpo.Length - 1; i >= 0; i--)
        {
            suma += (cuerpo[i] - '0') * multiplicador;
            multiplicador = multiplicador == 7 ? 2 : multiplicador + 1;
        }

        int resto = 11 - (suma % 11);
        char dvCalculado = resto switch { 11 => '0', 10 => 'K', _ => (char)('0' + resto) };

        return dvCalculado == dvIngresado ? limpio : null;
    }
}