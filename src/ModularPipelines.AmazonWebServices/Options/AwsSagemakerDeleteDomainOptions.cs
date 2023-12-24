using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "delete-domain")]
public record AwsSagemakerDeleteDomainOptions(
[property: CommandSwitch("--domain-id")] string DomainId
) : AwsOptions
{
    [CommandSwitch("--retention-policy")]
    public string? RetentionPolicy { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}