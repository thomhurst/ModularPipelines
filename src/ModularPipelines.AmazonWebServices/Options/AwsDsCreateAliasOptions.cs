using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "create-alias")]
public record AwsDsCreateAliasOptions(
[property: CommandSwitch("--directory-id")] string DirectoryId,
[property: CommandSwitch("--alias")] string Alias
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}