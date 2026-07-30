using System.Text.Json.Serialization;

namespace ModularPipelines.Caching;

[JsonSerializable(typeof(Dictionary<string, ModuleCacheFileHashRecord>))]
internal sealed partial class ModuleCacheJsonSerializerContext : JsonSerializerContext;
