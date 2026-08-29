using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Engine;
using ModularPipelines.Models;

namespace ModularPipelines.Distributed.Serialization;

internal class ModuleResultSerializer
{
    private readonly ModuleTypeRegistry _typeRegistry;
    private readonly ICommandExecutionCounter? _commandExecutionCounter;
    private readonly JsonSerializerOptions _options;

    public ModuleResultSerializer(
        ModuleTypeRegistry typeRegistry,
        ICommandExecutionCounter? commandExecutionCounter = null)
    {
        _typeRegistry = typeRegistry;
        _commandExecutionCounter = commandExecutionCounter;
        _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = false,
            Converters = { new ModuleResultJsonConverterFactory() },
        };

        // Add portable path converters so File/Folder objects serialize as git-root-relative paths.
        // This enables cross-platform distributed mode (e.g., Windows worker → Linux master).
        var gitRoot = GitRootFinder.Find();

        if (gitRoot is not null)
        {
            _options.Converters.Add(new PortableFilePathJsonConverter(gitRoot));
            _options.Converters.Add(new PortableFolderPathJsonConverter(gitRoot));
        }
    }

    [UnconditionalSuppressMessage(
        "AOT",
        "IL3050",
        Justification = "Distributed type-erased result serialization is explicitly unsupported in Native AOT.")]
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Distributed type-erased result serialization is explicitly unsupported in trimmed applications.")]
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
            CompletedAt: DateTimeOffset.UtcNow)
        {
            CommandCount = resolved is null
                ? 0
                : _commandExecutionCounter?.GetCount(resolved.Value.ModuleType) ?? 0,
        };
    }

    [UnconditionalSuppressMessage(
        "AOT",
        "IL3050",
        Justification = "Distributed type-erased result serialization is explicitly unsupported in Native AOT.")]
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Distributed type-erased result serialization is explicitly unsupported in trimmed applications.")]
    public IModuleResult? Deserialize(ModularPipelines.Distributed.SerializedModuleResult serialized)
    {
        var resolved = _typeRegistry.Resolve(serialized.ModuleTypeName) ?? throw new InvalidOperationException(
                $"Cannot deserialize result for module '{serialized.ModuleTypeName}': type not found in registry.");
        var resultType = typeof(ModuleResult<>).MakeGenericType(resolved.ResultType);
        var result = JsonSerializer.Deserialize(serialized.SerializedJson, resultType, _options) as ModuleResult;
        return result is null
            ? null
            : result with
            {
                ModuleType = resolved.ModuleType,
                TypeName = ModuleTypeIdentifier.Get(resolved.ModuleType),
            };
    }
}
