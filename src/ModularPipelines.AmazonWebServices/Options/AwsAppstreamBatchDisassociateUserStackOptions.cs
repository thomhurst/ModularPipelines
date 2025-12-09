using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "batch-disassociate-user-stack")]
public record AwsAppstreamBatchDisassociateUserStackOptions(
[property: CliOption("--user-stack-associations")] string[] UserStackAssociations
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}