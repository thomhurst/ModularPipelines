using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sms", "update-app")]
public record AwsSmsUpdateAppOptions : AwsOptions
{
    [CommandSwitch("--app-id")]
    public string? AppId { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--role-name")]
    public string? RoleName { get; set; }

    [CommandSwitch("--server-groups")]
    public string[]? ServerGroups { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}