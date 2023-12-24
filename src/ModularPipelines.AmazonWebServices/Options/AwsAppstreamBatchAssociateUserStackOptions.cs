using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "batch-associate-user-stack")]
public record AwsAppstreamBatchAssociateUserStackOptions(
[property: CommandSwitch("--user-stack-associations")] string[] UserStackAssociations
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}