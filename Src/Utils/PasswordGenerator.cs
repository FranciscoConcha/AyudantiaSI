namespace TecnoFix.Src.Utils;

public static class PasswordGenerator
{
    const string Caracteres  = "ABCDEFGHJKMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789";

    public static string GenerateRandomPassword(int length = 8)
    {    
        var random = Random.Shared;
        var chars = new char[length];
        for (int i = 0; i < length; i++)
        {
            chars[i] = Caracteres[random.Next(Caracteres.Length)];
        }
        return new string(chars);
    }
}