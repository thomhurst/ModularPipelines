using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectcases", "put-case-event-configuration")]
public record AwsConnectcasesPutCaseEventConfigurationOptions(
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--event-bridge")] string EventBridge
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}