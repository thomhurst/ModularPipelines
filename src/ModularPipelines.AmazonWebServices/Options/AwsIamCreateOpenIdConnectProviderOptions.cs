using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "create-open-id-connect-provider")]
public record AwsIamCreateOpenIdConnectProviderOptions(
[property: CliOption("--url")] string Url,
[property: CliOption("--thumbprint-list")] string[] ThumbprintList
) : AwsOptions
{
    [CliOption("--client-id-list")]
    public string[]? ClientIdList { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}