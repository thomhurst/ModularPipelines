using System.Text;

namespace ModularPipelines.Context;

public interface IHex
{
    string ToHex(string input) => ToHex(input, Encoding.UTF8);
    string ToHex(string input, Encoding encoding);
    string ToHex(IEnumerable<byte> bytes);

    string FromHex(string hexInput) => FromHex(hexInput, Encoding.UTF8);
    string FromHex(string hexInput, Encoding encoding);
}
