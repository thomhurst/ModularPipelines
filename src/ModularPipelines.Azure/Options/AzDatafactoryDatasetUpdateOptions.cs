using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datafactory", "dataset", "update")]
public record AzDatafactoryDatasetUpdateOptions(
[property: CliOption("--linked-service-name")] string LinkedServiceName
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--annotations")]
    public string? Annotations { get; set; }

    [CliOption("--dataset-name")]
    public string? DatasetName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--factory-name")]
    public string? FactoryName { get; set; }

    [CliOption("--folder")]
    public string? Folder { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--schema")]
    public string? Schema { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--structure")]
    public string? Structure { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}