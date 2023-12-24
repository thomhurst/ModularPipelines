using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "update-application-settings")]
public record AwsPinpointUpdateApplicationSettingsOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--write-application-settings-request")] string WriteApplicationSettingsRequest
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}