using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "create-document")]
public record AwsSsmCreateDocumentOptions(
[property: CommandSwitch("--content")] string Content,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--requires")]
    public string[]? Requires { get; set; }

    [CommandSwitch("--attachments")]
    public string[]? Attachments { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--version-name")]
    public string? VersionName { get; set; }

    [CommandSwitch("--document-type")]
    public string? DocumentType { get; set; }

    [CommandSwitch("--document-format")]
    public string? DocumentFormat { get; set; }

    [CommandSwitch("--target-type")]
    public string? TargetType { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}