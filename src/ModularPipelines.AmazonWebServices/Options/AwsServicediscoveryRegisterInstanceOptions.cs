using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicediscovery", "register-instance")]
public record AwsServicediscoveryRegisterInstanceOptions(
[property: CliOption("--service-id")] string ServiceId,
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--attributes")] IEnumerable<KeyValue> Attributes
) : AwsOptions
{
    [CliOption("--creator-request-id")]
    public string? CreatorRequestId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}