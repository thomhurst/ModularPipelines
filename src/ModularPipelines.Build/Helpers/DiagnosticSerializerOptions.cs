using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularPipelines.Build.Helpers;

/// <summary>
/// Shared JSON serializer options for diagnostic modules that print information to the log.
/// </summary>
internal static class DiagnosticSerializerOptions
{
    /// <summary>
    /// JSON serializer options configured for pretty-printing diagnostic information,
    /// with cycle handling for complex object graphs. Output is only written to logs, so
    /// relaxed escaping keeps characters such as '+' readable.
    /// </summary>
    public static readonly JsonSerializerOptions Instance = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        IgnoreReadOnlyFields = true,
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };
}
