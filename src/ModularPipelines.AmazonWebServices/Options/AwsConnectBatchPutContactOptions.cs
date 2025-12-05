using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "batch-put-contact")]
public record AwsConnectBatchPutContactOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--contact-data-request-list")] string[] ContactDataRequestList
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}