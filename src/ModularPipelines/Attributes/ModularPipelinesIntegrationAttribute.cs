namespace ModularPipelines.Attributes;

/// <summary>
/// Marks a static service-registration method as a Modular Pipelines integration.
/// </summary>
/// <remarks>
/// The source generator creates assembly metadata that lets each pipeline register the
/// integration without module initializers or process-wide mutable state.
/// </remarks>
[AttributeUsage(AttributeTargets.Method)]
public sealed class ModularPipelinesIntegrationAttribute : Attribute;
