using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medialive", "create-channel")]
public record AwsMedialiveCreateChannelOptions : AwsOptions
{
    [CommandSwitch("--cdi-input-specification")]
    public string? CdiInputSpecification { get; set; }

    [CommandSwitch("--channel-class")]
    public string? ChannelClass { get; set; }

    [CommandSwitch("--destinations")]
    public string[]? Destinations { get; set; }

    [CommandSwitch("--encoder-settings")]
    public string? EncoderSettings { get; set; }

    [CommandSwitch("--input-attachments")]
    public string[]? InputAttachments { get; set; }

    [CommandSwitch("--input-specification")]
    public string? InputSpecification { get; set; }

    [CommandSwitch("--log-level")]
    public string? LogLevel { get; set; }

    [CommandSwitch("--maintenance")]
    public string? Maintenance { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--request-id")]
    public string? RequestId { get; set; }

    [CommandSwitch("--reserved")]
    public string? Reserved { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--vpc")]
    public string? Vpc { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}