using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kendra", "update-access-control-configuration")]
public record AwsKendraUpdateAccessControlConfigurationOptions(
[property: CommandSwitch("--index-id")] string IndexId,
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--access-control-list")]
    public string[]? AccessControlList { get; set; }

    [CommandSwitch("--hierarchical-access-control-list")]
    public string[]? HierarchicalAccessControlList { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}