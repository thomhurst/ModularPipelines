using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tnb", "get-sol-function-instance")]
public record AwsTnbGetSolFunctionInstanceOptions(
[property: CliOption("--vnf-instance-id")] string VnfInstanceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}