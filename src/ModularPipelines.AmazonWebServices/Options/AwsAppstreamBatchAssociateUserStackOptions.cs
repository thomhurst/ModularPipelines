using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "batch-associate-user-stack")]
public record AwsAppstreamBatchAssociateUserStackOptions(
[property: CliOption("--user-stack-associations")] string[] UserStackAssociations
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}