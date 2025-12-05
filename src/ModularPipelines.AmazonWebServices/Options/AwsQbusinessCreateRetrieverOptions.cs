using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "create-retriever")]
public record AwsQbusinessCreateRetrieverOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--configuration")] string Configuration,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}