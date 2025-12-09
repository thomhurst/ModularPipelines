using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "remove-permission")]
public record AwsLambdaRemovePermissionOptions(
[property: CliOption("--function-name")] string FunctionName,
[property: CliOption("--statement-id")] string StatementId
) : AwsOptions
{
    [CliOption("--qualifier")]
    public string? Qualifier { get; set; }

    [CliOption("--revision-id")]
    public string? RevisionId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}