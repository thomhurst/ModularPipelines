using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("detective", "batch-get-membership-datasources")]
public record AwsDetectiveBatchGetMembershipDatasourcesOptions(
[property: CommandSwitch("--graph-arns")] string[] GraphArns
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}