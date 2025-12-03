using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmailmessageflow", "get-raw-message-content")]
public record AwsWorkmailmessageflowGetRawMessageContentOptions(
[property: CliOption("--message-id")] string MessageId
) : AwsOptions;