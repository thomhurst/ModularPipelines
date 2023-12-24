using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "describe-template")]
public record AwsQuicksightDescribeTemplateOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--template-id")] string TemplateId
) : AwsOptions
{
    [CommandSwitch("--version-number")]
    public long? VersionNumber { get; set; }

    [CommandSwitch("--alias-name")]
    public string? AliasName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}