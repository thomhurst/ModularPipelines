using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amp", "create-rule-groups-namespace")]
public record AwsAmpCreateRuleGroupsNamespaceOptions(
[property: CliOption("--workspace-id")] string WorkspaceId,
[property: CliOption("--name")] string Name,
[property: CliOption("--data")] string Data
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}