using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "restore-managed-prefix-list-version")]
public record AwsEc2RestoreManagedPrefixListVersionOptions(
[property: CommandSwitch("--prefix-list-id")] string PrefixListId,
[property: CommandSwitch("--previous-version")] long PreviousVersion,
[property: CommandSwitch("--current-version")] long CurrentVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}