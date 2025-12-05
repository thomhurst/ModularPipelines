using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "dismiss-user-contact")]
public record AwsConnectDismissUserContactOptions(
[property: CliOption("--user-id")] string UserId,
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--contact-id")] string ContactId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}