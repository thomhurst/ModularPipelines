using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medialive", "create-channel")]
public record AwsMedialiveCreateChannelOptions : AwsOptions
{
    [CliOption("--cdi-input-specification")]
    public string? CdiInputSpecification { get; set; }

    [CliOption("--channel-class")]
    public string? ChannelClass { get; set; }

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

    [CliOption("--request-id")]
    public string? RequestId { get; set; }

    [CliOption("--reserved")]
    public string? Reserved { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--vpc")]
    public string? Vpc { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}