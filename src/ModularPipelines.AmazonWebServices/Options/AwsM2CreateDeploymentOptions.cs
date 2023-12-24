using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("m2", "create-deployment")]
public record AwsM2CreateDeploymentOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--application-version")] int ApplicationVersion,
[property: CommandSwitch("--environment-id")] string EnvironmentId
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}