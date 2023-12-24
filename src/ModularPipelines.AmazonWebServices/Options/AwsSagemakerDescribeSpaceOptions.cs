using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "describe-space")]
public record AwsSagemakerDescribeSpaceOptions(
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--space-name")] string SpaceName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}