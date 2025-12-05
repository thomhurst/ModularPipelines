using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "import-phone-number")]
public record AwsConnectImportPhoneNumberOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--source-phone-number-arn")] string SourcePhoneNumberArn
) : AwsOptions
{
    [CliOption("--phone-number-description")]
    public string? PhoneNumberDescription { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}