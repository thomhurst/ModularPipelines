using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appconfig", "delete-extension")]
public record AwsAppconfigDeleteExtensionOptions(
[property: CommandSwitch("--extension-identifier")] string ExtensionIdentifier
) : AwsOptions
{
    [CommandSwitch("--version-number")]
    public int? VersionNumber { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}