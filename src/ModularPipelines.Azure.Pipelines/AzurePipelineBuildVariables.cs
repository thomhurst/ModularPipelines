using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Pipelines;

[ExcludeFromCodeCoverage]
public record AzurePipelineBuildVariables : AzurePipelineVariableBase
{
    public string? SourceBranch => Get("SourceBranch");

    public string? SourceVersion => Get("SourceVersion");

    public string? BuildId => Get("BuildId");

    /// <inheritdoc/>
    protected override string Prefix => "Build";
}
