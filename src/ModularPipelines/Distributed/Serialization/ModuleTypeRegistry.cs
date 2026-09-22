using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using ModularPipelines.Engine;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Serialization;

internal class ModuleTypeRegistry
{
    private const string WireSchemaVersion = "4";
    private readonly Lock _schemaLock = new();
    private string? _schemaVersion;
    private readonly ConcurrentDictionary<ModuleId, (Type ModuleType, Type ResultType)> _registry = new();

    public void Register(Type moduleType)
    {
        var resultType = GetResultType(moduleType);
        if (resultType is null)
        {
            return;
        }

        var moduleId = ModuleId.FromType(moduleType);
        lock (_schemaLock)
        {
            if (_registry.TryGetValue(moduleId, out var entry))
            {
                if (entry.ModuleType != moduleType)
                {
                    throw new InvalidOperationException(
                        $"Module identifier '{moduleId}' is already registered for '{entry.ModuleType}'. " +
                        $"It cannot also identify '{moduleType}'.");
                }

                return;
            }

            _registry[moduleId] = (moduleType, resultType);
            _schemaVersion = null;
        }
    }

    public (Type ModuleType, Type ResultType)? Resolve(ModuleId moduleId)
    {
        return _registry.TryGetValue(moduleId, out var entry) ? entry : null;
    }

    public IReadOnlyList<Type> GetRegisteredModuleTypes()
    {
        return _registry.Values.Select(entry => entry.ModuleType).ToArray();
    }

    public string GetPipelineSchemaVersion()
    {
        lock (_schemaLock)
        {
            return _schemaVersion ??= CreatePipelineSchemaVersion();
        }
    }

    private string CreatePipelineSchemaVersion()
    {
        var moduleSchema = string.Join(
            "\n",
            _registry
                .OrderBy(static entry => entry.Key.Value, StringComparer.Ordinal)
                .Select(static entry => $"{entry.Key.Value}\0{ModuleTypeIdentifier.Get(entry.Value.ResultType)}\0{entry.Value.ModuleType.Module.ModuleVersionId}"));
        var schema = $"wire={WireSchemaVersion}\n{moduleSchema}";
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(schema)));
    }

    private static Type? GetResultType(Type moduleType)
    {
        // Walk the inheritance chain to find Module<T> and extract T
        var current = moduleType;
        while (current is not null)
        {
            if (current.IsGenericType && current.GetGenericTypeDefinition() == typeof(Module<>))
            {
                return current.GetGenericArguments()[0];
            }

            current = current.BaseType;
        }

        return null;
    }
}
