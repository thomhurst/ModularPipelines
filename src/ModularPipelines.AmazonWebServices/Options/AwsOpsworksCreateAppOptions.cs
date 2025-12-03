using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "create-app")]
public record AwsOpsworksCreateAppOptions(
[property: CliOption("--stack-id")] string StackId,
[property: CliOption("--name")] string Name,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--shortname")]
    public string? Shortname { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--data-sources")]
    public string[]? DataSources { get; set; }

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