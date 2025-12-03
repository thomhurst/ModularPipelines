using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rum", "create-app-monitor")]
public record AwsRumCreateAppMonitorOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--app-monitor-configuration")]
    public string? AppMonitorConfiguration { get; set; }

    [CliOption("--custom-events")]
    public string? CustomEvents { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}