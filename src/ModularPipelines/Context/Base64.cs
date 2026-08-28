using System.Text;
using ModularPipelines.Context;

namespace ModularPipelines.Context;

internal class Base64 : IBase64Context
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

    public byte[] FromBase64String(string base64Input) => Convert.FromBase64String(base64Input);
}
