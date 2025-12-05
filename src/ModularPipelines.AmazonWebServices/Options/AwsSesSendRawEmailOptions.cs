using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ses", "send-raw-email")]
public record AwsSesSendRawEmailOptions(
[property: CliOption("--raw-message")] string RawMessage
) : AwsOptions
{
    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--destinations")]
    public string[]? Destinations { get; set; }

    [CliOption("--from-arn")]
    public string? FromArn { get; set; }

    [CliOption("--source-arn")]
    public string? SourceArn { get; set; }

    [CliOption("--return-path-arn")]
    public string? ReturnPathArn { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--configuration-set-name")]
    public string? ConfigurationSetName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}