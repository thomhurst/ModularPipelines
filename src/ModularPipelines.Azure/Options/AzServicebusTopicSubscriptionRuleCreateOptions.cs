using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("servicebus", "topic", "subscription", "rule", "create")]
public record AzServicebusTopicSubscriptionRuleCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--subscription-name")] string SubscriptionName,
[property: CliOption("--topic-name")] string TopicName
) : AzOptions
{
    [CliOption("--action-compatibility-level")]
    public string? ActionCompatibilityLevel { get; set; }

    [CliOption("--action-sql-expression")]
    public string? ActionSqlExpression { get; set; }

    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--correlation-filter")]
    public string? CorrelationFilter { get; set; }

    [CliOption("--correlation-id")]
    public string? CorrelationId { get; set; }

    [CliFlag("--enable-action-preprocessing")]
    public bool? EnableActionPreprocessing { get; set; }

    [CliFlag("--enable-correlation-preprocessing")]
    public bool? EnableCorrelationPreprocessing { get; set; }

    [CliFlag("--enable-sql-preprocessing")]
    public bool? EnableSqlPreprocessing { get; set; }

    [CliOption("--filter-sql-expression")]
    public string? FilterSqlExpression { get; set; }

    [CliOption("--filter-type")]
    public string? FilterType { get; set; }

    [CliOption("--label")]
    public string? Label { get; set; }

    [CliOption("--message-id")]
    public string? MessageId { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--reply-to")]
    public string? ReplyTo { get; set; }

    [CliOption("--reply-to-session-id")]
    public string? ReplyToSessionId { get; set; }

    [CliOption("--session-id")]
    public string? SessionId { get; set; }

    [CliOption("--to")]
    public string? To { get; set; }
}