using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "create-computer")]
public record AwsDsCreateComputerOptions(
[property: CliOption("--directory-id")] string DirectoryId,
[property: CliOption("--computer-name")] string ComputerName,
[property: CliOption("--password")] string Password
) : AwsOptions
{
    [CliOption("--organizational-unit-distinguished-name")]
    public string? OrganizationalUnitDistinguishedName { get; set; }

    [CliOption("--computer-attributes")]
    public string[]? ComputerAttributes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}