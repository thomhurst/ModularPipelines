using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medialive", "update-channel")]
public record AwsMedialiveUpdateChannelOptions(
[property: CommandSwitch("--channel-id")] string ChannelId
) : AwsOptions
{
    [CommandSwitch("--cdi-input-specification")]
    public string? CdiInputSpecification { get; set; }

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

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}