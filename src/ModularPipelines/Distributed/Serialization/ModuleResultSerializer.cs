using System.Text.Json;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Models;

namespace ModularPipelines.Distributed.Serialization;

internal class ModuleResultSerializer
{
    private readonly ModuleTypeRegistry _typeRegistry;
    private readonly JsonSerializerOptions _options;

    public ModuleResultSerializer(ModuleTypeRegistry typeRegistry)
    {
        _typeRegistry = typeRegistry;
        _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = false,
            Converters = { new ModuleResultJsonConverterFactory() },
        };
    }

    public ModularPipelines.Distributed.SerializedModuleResult Serialize(IModuleResult result, string moduleTypeName, string resultTypeName, int workerIndex)
    {
        // Serialize as the ModuleResult<T> base type so the custom converter writes the $type discriminator.
        // Using the concrete type (e.g. Success) would bypass the converter since it's registered for ModuleResult<T>.
        var resolved = _typeRegistry.Resolve(moduleTypeName);
        var serializeAsType = resolved is not null
            ? typeof(ModuleResult<>).MakeGenericType(resolved.Value.ResultType)
            : result.GetType();
        var json = JsonSerializer.Serialize(result, serializeAsType, _options);
        return new ModularPipelines.Distributed.SerializedModuleResult(
            ModuleTypeName: moduleTypeName,
            ResultTypeName: resultTypeName,
            WorkerIndex: workerIndex,
            SerializedJson: json,
            CompletedAt: DateTimeOffset.UtcNow
        );
    }

    public IModuleResult? Deserialize(ModularPipelines.Distributed.SerializedModuleResult serialized)
    {
        var resolved = _typeRegistry.Resolve(serialized.ModuleTypeName) ?? throw new InvalidOperationException(
                $"Cannot deserialize result for module '{serialized.ModuleTypeName}': type not found in registry.");
        var resultType = typeof(ModuleResult<>).MakeGenericType(resolved.ResultType);
        return JsonSerializer.Deserialize(serialized.SerializedJson, resultType, _options) as IModuleResult;
    }
}
