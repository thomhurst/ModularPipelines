using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ses", "send-templated-email")]
public record AwsSesSendTemplatedEmailOptions(
[property: CliOption("--source")] string Source,
[property: CliOption("--destination")] string Destination,
[property: CliOption("--template")] string Template,
[property: CliOption("--template-data")] string TemplateData
) : AwsOptions
{
    [CliOption("--reply-to-addresses")]
    public string[]? ReplyToAddresses { get; set; }

    [CliOption("--return-path")]
    public string? ReturnPath { get; set; }

    [CliOption("--source-arn")]
    public string? SourceArn { get; set; }

    [CliOption("--return-path-arn")]
    public string? ReturnPathArn { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--configuration-set-name")]
    public string? ConfigurationSetName { get; set; }

    [CliOption("--template-arn")]
    public string? TemplateArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}