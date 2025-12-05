using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "claim-phone-number")]
public record AwsConnectClaimPhoneNumberOptions(
[property: CliOption("--phone-number")] string PhoneNumber
) : AwsOptions
{
    [CliOption("--target-arn")]
    public string? TargetArn { get; set; }

    [CliOption("--instance-id")]
    public string? InstanceId { get; set; }

    [CliOption("--phone-number-description")]
    public string? PhoneNumberDescription { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}