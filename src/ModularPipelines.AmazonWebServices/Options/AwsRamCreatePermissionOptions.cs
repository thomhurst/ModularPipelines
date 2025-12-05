using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ram", "create-permission")]
public record AwsRamCreatePermissionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-type")] string ResourceType,
[property: CliOption("--policy-template")] string PolicyTemplate
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}