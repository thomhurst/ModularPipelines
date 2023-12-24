using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appconfig", "list-hosted-configuration-versions")]
public record AwsAppconfigListHostedConfigurationVersionsOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--configuration-profile-id")] string ConfigurationProfileId
) : AwsOptions
{
    [CommandSwitch("--version-label")]
    public string? VersionLabel { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}