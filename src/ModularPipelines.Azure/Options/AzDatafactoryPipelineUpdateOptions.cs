using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datafactory", "pipeline", "update")]
public record AzDatafactoryPipelineUpdateOptions : AzOptions
{
    [CliOption("--activities")]
    public string? Activities { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--annotations")]
    public string? Annotations { get; set; }

    [CliOption("--concurrency")]
    public string? Concurrency { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--duration")]
    public string? Duration { get; set; }

    [CliOption("--factory-name")]
    public string? FactoryName { get; set; }

    [CliOption("--folder-name")]
    public string? FolderName { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--run-dimensions")]
    public string? RunDimensions { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--variables")]
    public string? Variables { get; set; }
}