using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "execute-action")]
public record AwsIotsitewiseExecuteActionOptions(
[property: CliOption("--target-resource")] string TargetResource,
[property: CliOption("--action-definition-id")] string ActionDefinitionId,
[property: CliOption("--action-payload")] string ActionPayload
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}