using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "update-contact-attributes")]
public record AwsConnectUpdateContactAttributesOptions(
[property: CommandSwitch("--initial-contact-id")] string InitialContactId,
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--attributes")] IEnumerable<KeyValue> Attributes
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}