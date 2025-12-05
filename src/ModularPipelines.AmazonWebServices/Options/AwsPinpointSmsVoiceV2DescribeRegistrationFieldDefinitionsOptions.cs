using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "describe-registration-field-definitions")]
public record AwsPinpointSmsVoiceV2DescribeRegistrationFieldDefinitionsOptions(
[property: CliOption("--registration-type")] string RegistrationType
) : AwsOptions
{
    [CliOption("--section-path")]
    public string? SectionPath { get; set; }

    [CliOption("--field-paths")]
    public string[]? FieldPaths { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}