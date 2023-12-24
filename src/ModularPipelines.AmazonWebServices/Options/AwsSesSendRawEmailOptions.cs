using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ses", "send-raw-email")]
public record AwsSesSendRawEmailOptions(
[property: CommandSwitch("--raw-message")] string RawMessage
) : AwsOptions
{
    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--destinations")]
    public string[]? Destinations { get; set; }

    [CommandSwitch("--from-arn")]
    public string? FromArn { get; set; }

    [CommandSwitch("--source-arn")]
    public string? SourceArn { get; set; }

    [CommandSwitch("--return-path-arn")]
    public string? ReturnPathArn { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--configuration-set-name")]
    public string? ConfigurationSetName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}