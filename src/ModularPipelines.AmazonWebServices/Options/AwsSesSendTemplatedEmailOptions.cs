using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ses", "send-templated-email")]
public record AwsSesSendTemplatedEmailOptions(
[property: CommandSwitch("--source")] string Source,
[property: CommandSwitch("--destination")] string Destination,
[property: CommandSwitch("--template")] string Template,
[property: CommandSwitch("--template-data")] string TemplateData
) : AwsOptions
{
    [CommandSwitch("--reply-to-addresses")]
    public string[]? ReplyToAddresses { get; set; }

    [CommandSwitch("--return-path")]
    public string? ReturnPath { get; set; }

    [CommandSwitch("--source-arn")]
    public string? SourceArn { get; set; }

    [CommandSwitch("--return-path-arn")]
    public string? ReturnPathArn { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--configuration-set-name")]
    public string? ConfigurationSetName { get; set; }

    [CommandSwitch("--template-arn")]
    public string? TemplateArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}