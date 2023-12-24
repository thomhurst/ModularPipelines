using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "create-template")]
public record AwsQuicksightCreateTemplateOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--template-id")] string TemplateId
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--permissions")]
    public string[]? Permissions { get; set; }

    [CommandSwitch("--source-entity")]
    public string? SourceEntity { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--version-description")]
    public string? VersionDescription { get; set; }

    [CommandSwitch("--definition")]
    public string? Definition { get; set; }

    [CommandSwitch("--validation-strategy")]
    public string? ValidationStrategy { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}