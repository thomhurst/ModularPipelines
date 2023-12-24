using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sns", "get-platform-application-attributes")]
public record AwsSnsGetPlatformApplicationAttributesOptions(
[property: CommandSwitch("--platform-application-arn")] string PlatformApplicationArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}