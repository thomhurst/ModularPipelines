using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-sms-voice-v2", "describe-registration-field-values")]
public record AwsPinpointSmsVoiceV2DescribeRegistrationFieldValuesOptions(
[property: CliOption("--registration-id")] string RegistrationId
) : AwsOptions
{
    [CliOption("--version-number")]
    public long? VersionNumber { get; set; }

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