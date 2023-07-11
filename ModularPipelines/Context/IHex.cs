using System.Text;

namespace ModularPipelines.Context;

public interface IHex
{
    /// <summary>
    /// Converts a string to a hex encoded string
    /// </summary>
    /// <param name="input">The string to convert to a hex</param>
    /// <returns></returns>
    string ToHex(string input) => ToHex(input, Encoding.UTF8);

    /// <summary>
    /// Converts a string to a hex encoded string
    /// </summary>
    /// <param name="input">The string to convert to a hex</param>
    /// <param name="encoding">The string encoding</param>
    /// <returns></returns>
    string ToHex(string input, Encoding encoding);

    /// <summary>
    /// Converts a byte sequence to a hex encoded string
    /// </summary>
    /// <param name="bytes">The byte array to convert to a hex</param>
    /// <returns></returns>
    string ToHex(IEnumerable<byte> bytes);

    /// <summary>
    /// Converts a hex encoded string to a decoded standard string
    /// </summary>
    /// <param name="hexInput">The hex string to decode</param>
    /// <returns></returns>
    string FromHex(string hexInput) => FromHex(hexInput, Encoding.UTF8);


    /// <summary>
    /// Converts a hex encoded string to a decoded standard string
    /// </summary>
    /// <param name="hexInput">The hex string to decode</param>
    /// <param name="encoding">The string encoding</param>
    /// <returns></returns>
    string FromHex(string hexInput, Encoding encoding);
}
