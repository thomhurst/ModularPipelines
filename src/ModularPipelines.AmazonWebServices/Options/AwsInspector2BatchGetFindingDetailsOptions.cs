using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("inspector2", "batch-get-finding-details")]
public record AwsInspector2BatchGetFindingDetailsOptions(
[property: CommandSwitch("--finding-arns")] string[] FindingArns
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}