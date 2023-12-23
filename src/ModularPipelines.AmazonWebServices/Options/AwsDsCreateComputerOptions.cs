using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "create-computer")]
public record AwsDsCreateComputerOptions(
[property: CommandSwitch("--directory-id")] string DirectoryId,
[property: CommandSwitch("--computer-name")] string ComputerName,
[property: CommandSwitch("--password")] string Password
) : AwsOptions
{
    [CommandSwitch("--organizational-unit-distinguished-name")]
    public string? OrganizationalUnitDistinguishedName { get; set; }

    [CommandSwitch("--computer-attributes")]
    public string[]? ComputerAttributes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}