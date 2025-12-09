using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ses", "send-bulk-templated-email")]
public record AwsSesSendBulkTemplatedEmailOptions(
[property: CliOption("--source")] string Source,
[property: CliOption("--template")] string Template,
[property: CliOption("--destinations")] string[] Destinations
) : AwsOptions
{
    [CliOption("--source-arn")]
    public string? SourceArn { get; set; }

    [CliOption("--reply-to-addresses")]
    public string[]? ReplyToAddresses { get; set; }

    [CliOption("--return-path")]
    public string? ReturnPath { get; set; }

    [CliOption("--return-path-arn")]
    public string? ReturnPathArn { get; set; }

    [CliOption("--configuration-set-name")]
    public string? ConfigurationSetName { get; set; }

    [CliOption("--default-tags")]
    public string[]? DefaultTags { get; set; }

    [CliOption("--template-arn")]
    public string? TemplateArn { get; set; }

    [CliOption("--default-template-data")]
    public string? DefaultTemplateData { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}