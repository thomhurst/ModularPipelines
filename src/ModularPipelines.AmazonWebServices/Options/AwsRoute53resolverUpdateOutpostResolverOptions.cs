using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "update-outpost-resolver")]
public record AwsRoute53resolverUpdateOutpostResolverOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--instance-count")]
    public int? InstanceCount { get; set; }

    [CliOption("--preferred-instance-type")]
    public string? PreferredInstanceType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}