namespace ModularPipelines.Context;

/// <summary>
/// Provides checksum calculation functionality for files.
/// </summary>
public interface IChecksum
{
    /// <summary>
    /// Calculates the MD5 hash of a file.
    /// </summary>
    /// <param name="filePath">The path to the file to calculate the checksum for.</param>
    /// <returns>The MD5 hash as a hexadecimal string.</returns>
    string Md5(string filePath);
}