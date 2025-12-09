using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "create-rule")]
public record AwsConnectCreateRuleOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--name")] string Name,
[property: CliOption("--trigger-event-source")] string TriggerEventSource,
[property: CliOption("--function")] string Function,
[property: CliOption("--actions")] string[] Actions,
[property: CliOption("--publish-status")] string PublishStatus
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}