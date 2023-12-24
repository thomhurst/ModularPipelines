using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicediscovery", "register-instance")]
public record AwsServicediscoveryRegisterInstanceOptions(
[property: CommandSwitch("--service-id")] string ServiceId,
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--attributes")] IEnumerable<KeyValue> Attributes
) : AwsOptions
{
    [CommandSwitch("--creator-request-id")]
    public string? CreatorRequestId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}