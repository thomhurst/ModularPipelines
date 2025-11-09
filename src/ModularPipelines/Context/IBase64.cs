using System.Text;

namespace ModularPipelines.Context;

public interface IBase64
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
    /// Converts a bas64 encoded string to a decoded standard string.
    /// </summary>
    /// <param name="base64Input">The base64 string to decode.</param>
    /// <returns>The unencoded string.</returns>
    string FromBase64String(string base64Input) => FromBase64String(base64Input, Encoding.UTF8);

    /// <summary>
    /// Converts a bas64 encoded string to a decoded standard string.
    /// </summary>
    /// <param name="base64Input">The base64 string to decode.</param>
    /// <param name="encoding">The string encoding.</param>
    /// <returns>The unencoded string.</returns>
    string FromBase64String(string base64Input, Encoding encoding);
}
