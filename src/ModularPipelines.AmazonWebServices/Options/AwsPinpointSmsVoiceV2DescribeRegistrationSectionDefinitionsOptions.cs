using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice-v2", "describe-registration-section-definitions")]
public record AwsPinpointSmsVoiceV2DescribeRegistrationSectionDefinitionsOptions(
[property: CommandSwitch("--registration-type")] string RegistrationType
) : AwsOptions
{
    [CommandSwitch("--section-paths")]
    public string[]? SectionPaths { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}