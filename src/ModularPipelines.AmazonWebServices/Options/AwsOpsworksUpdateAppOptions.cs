using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "update-app")]
public record AwsOpsworksUpdateAppOptions(
[property: CliOption("--app-id")] string AppId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--data-sources")]
    public string[]? DataSources { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--app-source")]
    public string? AppSource { get; set; }

    [CliOption("--domains")]
    public string[]? Domains { get; set; }

    [CliOption("--ssl-configuration")]
    public string? SslConfiguration { get; set; }

    [CliOption("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CliOption("--environment")]
    public string[]? Environment { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}