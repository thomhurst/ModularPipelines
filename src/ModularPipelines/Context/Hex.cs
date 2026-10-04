using System.Text;

namespace ModularPipelines.Context;

internal class Hex : IHexContext
{
    public string ToHex(string input, Encoding encoding) => ToHex(encoding.GetBytes(input));

    public string ToHex(byte[] bytes) => Convert.ToHexStringLower(bytes);

    public byte[] FromHex(string hexInput) => Convert.FromHexString(hexInput);

    public string FromHex(string hexInput, Encoding encoding) => encoding.GetString(FromHex(hexInput));
}
