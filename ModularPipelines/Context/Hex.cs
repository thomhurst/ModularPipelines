using System.Text;

namespace ModularPipelines.Context;

public class Hex : IHex
{
    public string ToHex(string input, Encoding encoding)
    {
        var bytes = encoding.GetBytes(input);

        return ToHex(bytes);
    }

    public string ToHex(IEnumerable<byte> bytes)
    {
        var builder = new StringBuilder();

        foreach (var b in bytes)
        {
            builder.Append(b.ToString("x2"));
        }

        return builder.ToString();
    }

    public string FromHex(string hexInput, Encoding encoding)
    {
        hexInput = hexInput.Replace("-", "");

        var raw = new byte[hexInput.Length / 2];

        for (var i = 0; i < raw.Length; i++)
        {
            raw[i] = Convert.ToByte(hexInput.Substring(i * 2, 2), 16);
        }

        return encoding.GetString(raw);
    }
}
