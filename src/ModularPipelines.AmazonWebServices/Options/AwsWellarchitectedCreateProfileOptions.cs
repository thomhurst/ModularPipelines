using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "create-profile")]
public record AwsWellarchitectedCreateProfileOptions(
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--profile-description")] string ProfileDescription,
[property: CliOption("--profile-questions")] string[] ProfileQuestions
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}