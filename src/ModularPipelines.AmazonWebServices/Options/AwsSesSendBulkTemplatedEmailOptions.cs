using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ses", "send-bulk-templated-email")]
public record AwsSesSendBulkTemplatedEmailOptions(
[property: CommandSwitch("--source")] string Source,
[property: CommandSwitch("--template")] string Template,
[property: CommandSwitch("--destinations")] string[] Destinations
) : AwsOptions
{
    [CommandSwitch("--source-arn")]
    public string? SourceArn { get; set; }

    [CommandSwitch("--reply-to-addresses")]
    public string[]? ReplyToAddresses { get; set; }

    [CommandSwitch("--return-path")]
    public string? ReturnPath { get; set; }

    [CommandSwitch("--return-path-arn")]
    public string? ReturnPathArn { get; set; }

    [CommandSwitch("--configuration-set-name")]
    public string? ConfigurationSetName { get; set; }

    [CommandSwitch("--default-tags")]
    public string[]? DefaultTags { get; set; }

    [CommandSwitch("--template-arn")]
    public string? TemplateArn { get; set; }

    [CommandSwitch("--default-template-data")]
    public string? DefaultTemplateData { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}