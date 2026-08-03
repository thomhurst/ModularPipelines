using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Modules;

namespace ModularPipelines.Validation;

/// <summary>
/// Validates statically declared producer and consumer artifact contracts.
/// </summary>
internal sealed class ArtifactContractValidator : IPipelineValidator
{
    /// <inheritdoc />
    public int Order => 250;

    /// <inheritdoc />
    public ValidationResult Validate(IServiceProvider services) =>
        ValidateModules(services.GetServices<IModule>());

    internal static ValidationResult ValidateModules(IEnumerable<IModule> modules)
    {
        var result = new ValidationResult();
        var moduleTypes = modules
            .Select(module => module.GetType())
            .ToHashSet();

        foreach (var consumerType in moduleTypes)
        {
            var consumedArtifacts = consumerType
                .GetCustomAttributes(typeof(ConsumesArtifactAttribute), inherit: true)
                .Cast<ConsumesArtifactAttribute>();

            foreach (var consumedArtifact in consumedArtifacts)
            {
                ValidateConsumedArtifact(consumerType, consumedArtifact, moduleTypes, result);
            }
        }

        return result;
    }

    private static void ValidateConsumedArtifact(
        Type consumerType,
        ConsumesArtifactAttribute consumedArtifact,
        IReadOnlySet<Type> registeredModuleTypes,
        ValidationResult result)
    {
        var producerType = consumedArtifact.ProducerModule;
        if (!registeredModuleTypes.Contains(producerType))
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Artifact,
                $"Module '{consumerType.Name}' consumes artifact '{consumedArtifact.ArtifactName}' " +
                $"from unregistered producer module '{producerType.Name}'.",
                consumerType));
            return;
        }

        var producedArtifacts = producerType
            .GetCustomAttributes(typeof(ProducesArtifactAttribute), inherit: true)
            .Cast<ProducesArtifactAttribute>()
            .ToArray();

        if (producedArtifacts.Any(producedArtifact =>
                string.Equals(
                    producedArtifact.Name,
                    consumedArtifact.ArtifactName,
                    StringComparison.Ordinal)))
        {
            return;
        }

        var availableArtifacts = producedArtifacts.Length == 0
            ? "none"
            : string.Join(", ", producedArtifacts.Select(artifact => $"'{artifact.Name}'"));
        result.AddError(new ValidationError(
            ValidationErrorCategory.Artifact,
            $"Module '{consumerType.Name}' consumes artifact '{consumedArtifact.ArtifactName}', " +
            $"but producer module '{producerType.Name}' does not declare it. " +
            $"Available artifacts: {availableArtifacts}.",
            consumerType));
    }
}
