using System.Text;
using ModularPipelines.Context.Domains.Data;

namespace ModularPipelines.Context;

internal class Base64 : IBase64, IBase64Context
{
    public string ToBase64String(string input, Encoding encoding)
    {
        var bytes = encoding.GetBytes(input);
        return ToBase64String(bytes);
    }

    public string ToBase64String(byte[] bytes)
    {
        return Convert.ToBase64String(bytes);
    }

    public string FromBase64String(string base64Input, Encoding encoding)
    {
        var bytes = Convert.FromBase64String(base64Input);
        return encoding.GetString(bytes);
    }
}