using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codestar-notifications", "unsubscribe")]
public record AwsCodestarNotificationsUnsubscribeOptions(
[property: CliOption("--arn")] string Arn,
[property: CliOption("--target-address")] string TargetAddress
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}