using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "create-outpost-resolver")]
public record AwsRoute53resolverCreateOutpostResolverOptions(
[property: CliOption("--creator-request-id")] string CreatorRequestId,
[property: CliOption("--name")] string Name,
[property: CliOption("--preferred-instance-type")] string PreferredInstanceType,
[property: CliOption("--outpost-arn")] string OutpostArn
) : AwsOptions
{
    [CliOption("--instance-count")]
    public int? InstanceCount { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}