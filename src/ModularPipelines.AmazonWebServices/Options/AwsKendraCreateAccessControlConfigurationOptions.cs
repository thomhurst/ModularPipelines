using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kendra", "create-access-control-configuration")]
public record AwsKendraCreateAccessControlConfigurationOptions(
[property: CommandSwitch("--index-id")] string IndexId,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--access-control-list")]
    public string[]? AccessControlList { get; set; }

    [CommandSwitch("--hierarchical-access-control-list")]
    public string[]? HierarchicalAccessControlList { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}