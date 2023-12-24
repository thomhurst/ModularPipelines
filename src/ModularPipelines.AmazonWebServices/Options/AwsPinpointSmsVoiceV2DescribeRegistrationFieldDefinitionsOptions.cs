using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-sms-voice-v2", "describe-registration-field-definitions")]
public record AwsPinpointSmsVoiceV2DescribeRegistrationFieldDefinitionsOptions(
[property: CommandSwitch("--registration-type")] string RegistrationType
) : AwsOptions
{
    [CommandSwitch("--section-path")]
    public string? SectionPath { get; set; }

    [CommandSwitch("--field-paths")]
    public string[]? FieldPaths { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}