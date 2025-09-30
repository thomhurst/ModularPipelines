using System.Text.Json;
using Microsoft.Extensions.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Serialization;

namespace ModularPipelines.Distributed.Serialization;

/// <summary>
/// Handles serialization and deserialization of modules and module results for distributed execution.
/// </summary>
internal sealed class ModuleSerializer
{
    private readonly ILogger<ModuleSerializer> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public ModuleSerializer(ILogger<ModuleSerializer> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _jsonOptions = new JsonSerializerOptions(ModularPipelinesJsonSerializerSettings.Default)
        {
            WriteIndented = false,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never,
        };
    }

    /// <summary>
    /// Serializes a module to JSON string.
    /// </summary>
    /// <param name="module">The module to serialize.</param>
    /// <returns>The serialized module as JSON string.</returns>
    public string SerializeModule(ModuleBase module)
    {
        ArgumentNullException.ThrowIfNull(module);

        try
        {
            var json = JsonSerializer.Serialize(module, module.GetType(), _jsonOptions);

            _logger.LogDebug(
                "Serialized module {ModuleType}. Size: {Size} bytes",
                module.GetType().Name,
                json.Length);

            return json;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to serialize module {ModuleType}",
                module.GetType().Name);
            throw;
        }
    }

    /// <summary>
    /// Deserializes a module from JSON string.
    /// </summary>
    /// <param name="json">The JSON string containing the serialized module.</param>
    /// <param name="moduleType">The type of the module to deserialize.</param>
    /// <returns>The deserialized module.</returns>
    public ModuleBase DeserializeModule(string json, Type moduleType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(json);
        ArgumentNullException.ThrowIfNull(moduleType);

        if (!moduleType.IsAssignableTo(typeof(ModuleBase)))
        {
            throw new ArgumentException(
                $"Type {moduleType.FullName} is not a valid module type",
                nameof(moduleType));
        }

        try
        {
            var module = JsonSerializer.Deserialize(json, moduleType, _jsonOptions) as ModuleBase;

            if (module == null)
            {
                throw new InvalidOperationException($"Failed to deserialize module of type {moduleType.FullName}");
            }

            _logger.LogDebug(
                "Deserialized module {ModuleType}",
                moduleType.Name);

            return module;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to deserialize module {ModuleType}",
                moduleType.Name);
            throw;
        }
    }

    /// <summary>
    /// Serializes a module result to JSON string.
    /// </summary>
    /// <param name="result">The module result to serialize.</param>
    /// <returns>The serialized result as JSON string.</returns>
    public string SerializeResult(IModuleResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        try
        {
            var json = JsonSerializer.Serialize(result, result.GetType(), _jsonOptions);

            _logger.LogDebug(
                "Serialized module result for {ModuleName}. Size: {Size} bytes",
                result.ModuleName,
                json.Length);

            return json;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to serialize result for module {ModuleName}",
                result.ModuleName);
            throw;
        }
    }

    /// <summary>
    /// Deserializes a module result from JSON string.
    /// </summary>
    /// <param name="json">The JSON string containing the serialized result.</param>
    /// <returns>The deserialized module result.</returns>
    public IModuleResult DeserializeResult(string json)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(json);

        try
        {
            // Use the TypeDiscriminatorConverter to deserialize the correct type
            var result = JsonSerializer.Deserialize<ModuleResult>(json, _jsonOptions);

            if (result == null)
            {
                throw new InvalidOperationException("Failed to deserialize module result");
            }

            _logger.LogDebug(
                "Deserialized module result for {ModuleName}",
                result.ModuleName);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to deserialize module result");
            throw;
        }
    }

    /// <summary>
    /// Serializes a dictionary of dependency results.
    /// </summary>
    /// <param name="dependencyResults">The dependency results to serialize.</param>
    /// <returns>A dictionary mapping module type names to serialized results.</returns>
    public Dictionary<string, string> SerializeDependencyResults(
        IReadOnlyDictionary<Type, IModuleResult> dependencyResults)
    {
        ArgumentNullException.ThrowIfNull(dependencyResults);

        var serialized = new Dictionary<string, string>();

        foreach (var (type, result) in dependencyResults)
        {
            var key = type.AssemblyQualifiedName ?? type.FullName ?? type.Name;
            serialized[key] = SerializeResult(result);
        }

        _logger.LogDebug(
            "Serialized {Count} dependency results",
            serialized.Count);

        return serialized;
    }

    /// <summary>
    /// Deserializes a dictionary of dependency results.
    /// </summary>
    /// <param name="serializedResults">The serialized dependency results.</param>
    /// <returns>A dictionary mapping module types to deserialized results.</returns>
    public Dictionary<Type, IModuleResult> DeserializeDependencyResults(
        Dictionary<string, string> serializedResults)
    {
        ArgumentNullException.ThrowIfNull(serializedResults);

        var deserialized = new Dictionary<Type, IModuleResult>();

        foreach (var (typeName, json) in serializedResults)
        {
            var type = Type.GetType(typeName);
            if (type == null)
            {
                _logger.LogWarning(
                    "Could not resolve type {TypeName} for dependency result",
                    typeName);
                continue;
            }

            var result = DeserializeResult(json);
            deserialized[type] = result;
        }

        _logger.LogDebug(
            "Deserialized {Count} dependency results",
            deserialized.Count);

        return deserialized;
    }
}
