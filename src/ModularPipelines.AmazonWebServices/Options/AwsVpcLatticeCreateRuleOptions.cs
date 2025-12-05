using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vpc-lattice", "create-rule")]
public record AwsVpcLatticeCreateRuleOptions(
[property: CliOption("--action")] string Action,
[property: CliOption("--listener-identifier")] string ListenerIdentifier,
[property: CliOption("--match")] string Match,
[property: CliOption("--name")] string Name,
[property: CliOption("--priority")] int Priority,
[property: CliOption("--service-identifier")] string ServiceIdentifier
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}