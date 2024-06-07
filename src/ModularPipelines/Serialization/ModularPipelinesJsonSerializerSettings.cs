using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularPipelines.Serialization;

internal static class ModularPipelinesJsonSerializerSettings
{
    public static readonly JsonSerializerOptions Default = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        WriteIndented = true,
    };
}