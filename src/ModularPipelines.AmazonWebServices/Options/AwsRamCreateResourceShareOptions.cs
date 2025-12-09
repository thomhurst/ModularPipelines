using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ram", "create-resource-share")]
public record AwsRamCreateResourceShareOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--resource-arns")]
    public string[]? ResourceArns { get; set; }

    [CliOption("--principals")]
    public string[]? Principals { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--permission-arns")]
    public string[]? PermissionArns { get; set; }

    [CliOption("--sources")]
    public string[]? Sources { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}