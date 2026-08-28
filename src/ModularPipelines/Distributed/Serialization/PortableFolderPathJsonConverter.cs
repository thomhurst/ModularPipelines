using System.Text.Json;
using System.Text.Json.Serialization;
using ModularPipelines.FileSystem;

namespace ModularPipelines.Distributed.Serialization;

/// <summary>
/// Serializes FolderPath paths relative to the git root for cross-platform distributed transfer.
/// On deserialization, resolves the relative path against the local git root.
/// </summary>
internal class PortableFolderPathJsonConverter : JsonConverter<FolderPath>
{
    private readonly string _gitRoot;

    public PortableFolderPathJsonConverter(string gitRoot)
    {
        _gitRoot = gitRoot;
    }

    public override FolderPath? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        var serializedPath = reader.GetString()!;

        // If it's already an absolute path for this platform, use it directly
        if (Path.IsPathRooted(serializedPath))
        {
            return new FolderPath(serializedPath);
        }

        // Relative path from another instance — resolve against local git root
        var localPath = Path.GetFullPath(Path.Combine(_gitRoot, serializedPath));
        return new FolderPath(localPath);
    }

    public override void Write(Utf8JsonWriter writer, FolderPath value, JsonSerializerOptions options)
    {
        var absolutePath = value.Path;

        // Try to make the path relative to git root for portability
        var relativePath = TryGetRelativePath(absolutePath);
        writer.WriteStringValue(relativePath);
    }

    private string TryGetRelativePath(string absolutePath)
    {
        // Normalize both paths to forward slashes for comparison
        var normalizedAbsolute = absolutePath.Replace('\\', '/');
        var normalizedRoot = _gitRoot.Replace('\\', '/').TrimEnd('/') + "/";

        if (normalizedAbsolute.StartsWith(normalizedRoot, StringComparison.OrdinalIgnoreCase))
        {
            return normalizedAbsolute[normalizedRoot.Length..];
        }

        // Path is outside git root — store as-is (will only work on same platform)
        return absolutePath;
    }
}
