using System.Text.Json.Serialization;

namespace ModularPipelines.Distributed.Models;

/// <summary>
/// Represents a file to be transferred between orchestrator and worker nodes.
/// Used to transfer files generated during module execution (e.g., test coverage reports).
/// </summary>
public sealed class FileTransferInfo
{
    /// <summary>
    /// Gets the relative path of the file (preserves directory structure).
    /// </summary>
    [JsonPropertyName("relativePath")]
    public required string RelativePath { get; init; }

    /// <summary>
    /// Gets the file content as bytes.
    /// </summary>
    [JsonPropertyName("content")]
    public required byte[] Content { get; init; }

    /// <summary>
    /// Gets the SHA256 hash of the content for integrity verification.
    /// </summary>
    [JsonPropertyName("contentHash")]
    public string? ContentHash { get; init; }

    /// <summary>
    /// Gets the size of the file in bytes.
    /// </summary>
    [JsonPropertyName("size")]
    public long Size => Content.Length;
}
