using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-managed-prefix-list")]
public record AwsEc2CreateManagedPrefixListOptions(
[property: CommandSwitch("--prefix-list-name")] string PrefixListName,
[property: CommandSwitch("--max-entries")] int MaxEntries,
[property: CommandSwitch("--address-family")] string AddressFamily
) : AwsOptions
{
    [CommandSwitch("--entries")]
    public string[]? Entries { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}