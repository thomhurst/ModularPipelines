using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectcases", "put-case-event-configuration")]
public record AwsConnectcasesPutCaseEventConfigurationOptions(
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--event-bridge")] string EventBridge
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}