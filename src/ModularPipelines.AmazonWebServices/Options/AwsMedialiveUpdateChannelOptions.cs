using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medialive", "update-channel")]
public record AwsMedialiveUpdateChannelOptions(
[property: CliOption("--channel-id")] string ChannelId
) : AwsOptions
{
    [CliOption("--cdi-input-specification")]
    public string? CdiInputSpecification { get; set; }

    [CliOption("--destinations")]
    public string[]? Destinations { get; set; }

    [CliOption("--encoder-settings")]
    public string? EncoderSettings { get; set; }

    [CliOption("--input-attachments")]
    public string[]? InputAttachments { get; set; }

    [CliOption("--input-specification")]
    public string? InputSpecification { get; set; }

    [CliOption("--log-level")]
    public string? LogLevel { get; set; }

    [CliOption("--maintenance")]
    public string? Maintenance { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}