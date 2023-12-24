using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("proton", "list-environment-outputs")]
public record AwsProtonListEnvironmentOutputsOptions(
[property: CommandSwitch("--environment-name")] string EnvironmentName
) : AwsOptions
{
    [CommandSwitch("--deployment-id")]
    public string? DeploymentId { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}