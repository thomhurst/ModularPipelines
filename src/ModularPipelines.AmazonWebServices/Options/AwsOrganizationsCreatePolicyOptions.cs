using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("organizations", "create-policy")]
public record AwsOrganizationsCreatePolicyOptions(
[property: CliOption("--content")] string Content,
[property: CliOption("--description")] string Description,
[property: CliOption("--name")] string Name,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}