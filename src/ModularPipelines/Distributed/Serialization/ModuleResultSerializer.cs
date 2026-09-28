using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Text.Json;
using ModularPipelines.Engine;
using ModularPipelines.Models;

namespace ModularPipelines.Distributed.Serialization;

internal class ModuleResultSerializer(
    ModuleTypeRegistry typeRegistry,
    ICommandExecutionCounter? commandExecutionCounter = null)
{
    private readonly ModuleTypeRegistry _typeRegistry = typeRegistry;
    private readonly ICommandExecutionCounter? _commandExecutionCounter = commandExecutionCounter;
    private readonly JsonSerializerOptions _options = CreateOptions();
    private readonly ConditionalWeakTable<Type, JsonSerializerOptions> _deserializationOptions = [];

    internal static JsonSerializerOptions CreateOptions(AssemblyLoadContext? loadContext = null)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = false,
            Converters = { new ModuleResultJsonConverterFactory { LoadContext = loadContext } },
        };

        // Add portable path converters so FilePath/FolderPath objects serialize as git-root-relative paths.
        // This enables cross-platform distributed mode (e.g., Windows worker → Linux master).
        var gitRoot = GitRootFinder.Find();

        if (gitRoot is not null)
        {
            options.Converters.Add(new PortableFilePathJsonConverter(gitRoot));
            options.Converters.Add(new PortableFolderPathJsonConverter(gitRoot));
        }

        return options;
    }

    [UnconditionalSuppressMessage(
        "AOT",
        "IL3050",
        Justification = "Distributed type-erased result serialization is explicitly unsupported in Native AOT.")]
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Distributed type-erased result serialization is explicitly unsupported in trimmed applications.")]
    public SerializedModuleResult Serialize(IModuleResult result, ModuleId moduleId, WorkerId workerId)
    {
        // Serialize as the ModuleResult<T> base type so the custom converter writes the $type discriminator.
        // Using the concrete type (e.g. Success) would bypass the converter since it's registered for ModuleResult<T>.
        var resolved = _typeRegistry.Resolve(moduleId);
        var serializeAsType = resolved is not null
            ? typeof(ModuleResult<>).MakeGenericType(resolved.Value.ResultType)
            : result.GetType();
        var json = JsonSerializer.Serialize(result, serializeAsType, _options);
        return new SerializedModuleResult
        {
            ModuleId = moduleId,
            WorkerId = workerId,
            Payload = json,
            CompletedAt = DateTimeOffset.UtcNow,
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
    public IModuleResult? Deserialize(SerializedModuleResult serialized)
    {
        var (moduleType, valueType) = _typeRegistry.Resolve(serialized.ModuleId) ?? throw new InvalidOperationException(
                $"Cannot deserialize result for module '{serialized.ModuleId}': type not found in registry.");
        var resultType = typeof(ModuleResult<>).MakeGenericType(valueType);
        var options = _deserializationOptions.GetValue(moduleType, type =>
            CreateOptions(ModuleResultJsonConverterFactory.GetLoadContext(type, valueType)));
        var result = JsonSerializer.Deserialize(serialized.Payload, resultType, options) as ModuleResult;
        if (result?.ExceptionOrDefault is RemoteModuleException remoteException)
        {
            remoteException.AttachWorkerId(serialized.WorkerId);
        }

        return result is null
            ? null
            : result with
            {
                ModuleType = moduleType,
                TypeName = ModuleTypeIdentifier.Get(moduleType),
                WorkerId = serialized.WorkerId,
            };
    }

    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Distributed result serialization is unsupported in Native AOT.")]
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Distributed result serialization is unsupported in trimmed applications.")]
    public SerializedModuleResult SerializeFailure(ModuleId moduleId, Exception exception, WorkerId workerId)
    {
        var now = DateTimeOffset.UtcNow;
        ModuleResult failure = new ModuleResult.Failure(exception)
        {
            Name = moduleId.Value,
            Duration = TimeSpan.Zero,
            StartTime = now,
            EndTime = now,
            Status = ModuleStatus.Failed,
        };
        // Failure payloads contain no result value and do not require the remote module's type.
        return new SerializedModuleResult
        {
            ModuleId = moduleId,
            WorkerId = workerId,
            Payload = JsonSerializer.Serialize(failure, _options),
            CompletedAt = now,
        };
    }
}
