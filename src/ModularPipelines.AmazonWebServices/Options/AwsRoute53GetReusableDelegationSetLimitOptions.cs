using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "get-reusable-delegation-set-limit")]
public record AwsRoute53GetReusableDelegationSetLimitOptions(
[property: CliOption("--type")] string Type,
[property: CliOption("--delegation-set-id")] string DelegationSetId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}