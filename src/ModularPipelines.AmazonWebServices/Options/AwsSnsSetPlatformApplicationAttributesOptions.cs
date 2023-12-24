using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sns", "set-platform-application-attributes")]
public record AwsSnsSetPlatformApplicationAttributesOptions(
[property: CommandSwitch("--platform-application-arn")] string PlatformApplicationArn,
[property: CommandSwitch("--attributes")] IEnumerable<KeyValue> Attributes
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}