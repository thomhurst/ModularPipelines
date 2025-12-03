using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resiliencehub", "create-resiliency-policy")]
public record AwsResiliencehubCreateResiliencyPolicyOptions(
[property: CliOption("--policy")] IEnumerable<KeyValue> Policy,
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--tier")] string Tier
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--data-location-constraint")]
    public string? DataLocationConstraint { get; set; }

    [CliOption("--policy-description")]
    public string? PolicyDescription { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}