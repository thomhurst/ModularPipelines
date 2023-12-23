using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm-sap", "get-component")]
public record AwsSsmSapGetComponentOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--component-id")] string ComponentId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}