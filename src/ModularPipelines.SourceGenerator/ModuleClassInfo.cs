namespace ModularPipelines.SourceGenerator;

/// <summary>
/// Information about a Module class discovered by the generator.
/// </summary>
internal sealed record ModuleClassInfo(
    string Namespace,
    string ClassName
);
