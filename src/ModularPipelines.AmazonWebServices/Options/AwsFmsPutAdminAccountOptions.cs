using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fms", "put-admin-account")]
public record AwsFmsPutAdminAccountOptions(
[property: CliOption("--admin-account")] string AdminAccount
) : AwsOptions
{
    [CliOption("--admin-scope")]
    public string? AdminScope { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}