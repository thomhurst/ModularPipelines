using System.Text;

namespace ModularPipelines.Context;

public interface IBase64Context
{
    /// <summary>
    /// Converts a string to a base64 encoded string.
    /// </summary>
    /// <param name="input">The string to convert to base64.</param>
    /// <returns>The Base64 encoded string.</returns>
    string ToBase64String(string input) => ToBase64String(input, Encoding.UTF8);

    /// <summary>
    /// Converts a string to a base64 encoded string.
    /// </summary>
    /// <param name="input">The string to convert to base64.</param>
    /// <param name="encoding">The string encoding.</param>
    /// <returns>The Base64 encoded string.</returns>
    string ToBase64String(string input, Encoding encoding);

    /// <summary>
    /// Converts a byte array to a base64 encoded string.
    /// </summary>
    /// <param name="bytes">The byte array to convert to base64.</param>
    /// <returns>The Base64 encoded string.</returns>
    string ToBase64String(byte[] bytes);

    /// <summary>
    /// Converts a base64 encoded string to bytes.
    /// </summary>
    /// <param name="base64Input">The base64 string to decode.</param>
    /// <returns>The decoded bytes.</returns>
    byte[] FromBase64String(string base64Input);
}
