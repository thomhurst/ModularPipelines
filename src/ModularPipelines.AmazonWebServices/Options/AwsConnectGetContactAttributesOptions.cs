using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "get-contact-attributes")]
public record AwsConnectGetContactAttributesOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--initial-contact-id")] string InitialContactId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}