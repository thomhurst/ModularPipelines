using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("servicebus", "topic", "subscription", "rule", "update")]
public record AzServicebusTopicSubscriptionRuleUpdateOptions : AzOptions
{
    [CliOption("--action-compatibility-level")]
    public string? ActionCompatibilityLevel { get; set; }

    [CliFlag("--action-preprocessing")]
    public bool? ActionPreprocessing { get; set; }

    [CliOption("--action-sql-expression")]
    public string? ActionSqlExpression { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--compatibility-level")]
    public string? CompatibilityLevel { get; set; }

    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--correlation-id")]
    public string? CorrelationId { get; set; }

    [CliFlag("--enable-correlation-preprocessing")]
    public bool? EnableCorrelationPreprocessing { get; set; }

    [CliFlag("--enable-sql-preprocessing")]
    public bool? EnableSqlPreprocessing { get; set; }

    [CliOption("--filter-sql-expression")]
    public string? FilterSqlExpression { get; set; }

    [CliOption("--filter-type")]
    public string? FilterType { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--label")]
    public string? Label { get; set; }

    [CliOption("--message-id")]
    public string? MessageId { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--namespace-name")]
    public string? NamespaceName { get; set; }

    [CliOption("--properties")]
    public string? Properties { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--reply-to")]
    public string? ReplyTo { get; set; }

    [CliOption("--reply-to-session-id")]
    public string? ReplyToSessionId { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--session-id")]
    public string? SessionId { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--subscription-name")]
    public string? SubscriptionName { get; set; }

    [CliOption("--to")]
    public string? To { get; set; }

    [CliOption("--topic-name")]
    public string? TopicName { get; set; }
}