using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "update-resolver-config")]
public record AwsRoute53resolverUpdateResolverConfigOptions(
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--autodefined-reverse-flag")] string AutodefinedReverseFlag
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}