using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-managed-prefix-list")]
public record AwsEc2ModifyManagedPrefixListOptions(
[property: CommandSwitch("--prefix-list-id")] string PrefixListId
) : AwsOptions
{
    [CommandSwitch("--current-version")]
    public long? CurrentVersion { get; set; }

    [CommandSwitch("--prefix-list-name")]
    public string? PrefixListName { get; set; }

    [CommandSwitch("--add-entries")]
    public string[]? AddEntries { get; set; }

    [CommandSwitch("--remove-entries")]
    public string[]? RemoveEntries { get; set; }

    [CommandSwitch("--max-entries")]
    public int? MaxEntries { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}