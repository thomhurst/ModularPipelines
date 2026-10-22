using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ModularPipelines.Distributed.Artifacts.S3.Artifacts;

/// <summary>
/// Factory that creates a <see cref="S3DistributedArtifactStore"/> by initializing the S3 client.
/// Optionally checks an externally provisioned lifecycle rule for automatic artifact expiration.
/// </summary>
internal sealed class S3DistributedArtifactStoreFactory : IDistributedArtifactStoreFactory
{
    private const string LifecycleRuleIdPrefix = "modpipe-artifact-expiration:";
    private const string NoLifecycleConfigurationErrorCode = "NoSuchLifecycleConfiguration";
    private const int AbortIncompleteMultipartUploadDays = 1;

    private readonly S3StorageOptions _options;
    private readonly DistributedOptions _distributedOptions;
    private readonly ILogger<S3DistributedArtifactStoreFactory> _logger;
    private readonly Func<S3StorageOptions, IAmazonS3> _createClient;

    public S3DistributedArtifactStoreFactory(
        IOptions<S3StorageOptions> options,
        IOptions<DistributedOptions> distributedOptions,
        ILogger<S3DistributedArtifactStoreFactory> logger)
        : this(options, distributedOptions, logger, S3ClientFactory.Create)
    {
    }

    internal S3DistributedArtifactStoreFactory(
        IOptions<S3StorageOptions> options,
        IOptions<DistributedOptions> distributedOptions,
        ILogger<S3DistributedArtifactStoreFactory> logger,
        Func<S3StorageOptions, IAmazonS3> createClient)
    {
        _options = options.Value;
        _distributedOptions = distributedOptions.Value;
        _logger = logger;
        _createClient = createClient;
    }

    public async Task<IDistributedArtifactStore> CreateAsync(CancellationToken cancellationToken)
    {
        var s3 = _createClient(_options);
        try
        {
            if (_options.SetLifecycleRule)
            {
                await CheckLifecycleRuleAsync(s3, cancellationToken).ConfigureAwait(false);
            }

            return new S3DistributedArtifactStore(s3, _options, _distributedOptions.RunId, _logger);
        }
        catch
        {
            s3.Dispose();
            throw;
        }
    }

    private async Task CheckLifecycleRuleAsync(IAmazonS3 s3, CancellationToken cancellationToken)
    {
        var rule = CreateRule();
        try
        {
            var existingRules = await GetExistingRulesAsync(s3, cancellationToken).ConfigureAwait(false);
            var current = existingRules.FirstOrDefault(existing => existing.Id == rule.Id);
            if (current is not null && IsEquivalent(current, rule))
            {
                return;
            }

            // S3 replaces the entire configuration on each write. Independent runs cannot
            // safely merge rules at startup, so infrastructure owns all lifecycle writes.
            _logger.LogWarning(
                "S3 lifecycle rule {RuleId} on bucket {BucketName} is missing or differs from the expected prefix, enabled status, {ExpirationDays}-day expiration, or one-day multipart-abort action. Provision the rule outside pipeline startup; bucket lifecycle configuration was not changed.",
                rule.Id,
                _options.BucketName,
                rule.Expiration.Days);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            // Lifecycle configuration may be unsupported or unreadable with these credentials.
            // Artifacts still work, but the external expiration policy could not be checked.
            _logger.LogWarning(
                exception,
                "Could not check S3 lifecycle rule {RuleId} on bucket {BucketName}. Provision and manage lifecycle rules outside pipeline startup; bucket lifecycle configuration was not changed.",
                rule.Id,
                _options.BucketName);
        }
    }

    private async Task<List<LifecycleRule>> GetExistingRulesAsync(IAmazonS3 s3, CancellationToken cancellationToken)
    {
        try
        {
            var response = await s3.GetLifecycleConfigurationAsync(_options.BucketName, cancellationToken)
                .ConfigureAwait(false);
            return response.Configuration?.Rules ?? [];
        }
        catch (AmazonS3Exception exception) when (exception.ErrorCode == NoLifecycleConfigurationErrorCode)
        {
            return [];
        }
    }

    private LifecycleRule CreateRule()
    {
        var prefix = S3DistributedArtifactStore.GetArtifactPrefix(_options);
        return new LifecycleRule
        {
            Id = LifecycleRuleIdPrefix + prefix,
            Status = LifecycleRuleStatus.Enabled,
            Filter = new LifecycleFilter
            {
                LifecycleFilterPredicate = new LifecyclePrefixPredicate
                {
                    Prefix = prefix,
                },
            },
            Expiration = new LifecycleRuleExpiration
            {
                Days = Math.Max(1, (int) Math.Ceiling(_options.TimeToLive.TotalDays)),
            },

            // Expiration does not remove parts of multipart uploads that were never completed or
            // aborted, such as after a crash or a failed abort.
            AbortIncompleteMultipartUpload = new LifecycleRuleAbortIncompleteMultipartUpload
            {
                DaysAfterInitiation = AbortIncompleteMultipartUploadDays,
            },
        };
    }

    private static bool IsEquivalent(LifecycleRule existing, LifecycleRule desired) =>
        existing.Status == desired.Status
        && existing.Expiration?.Days == desired.Expiration?.Days
        && existing.AbortIncompleteMultipartUpload?.DaysAfterInitiation
            == desired.AbortIncompleteMultipartUpload?.DaysAfterInitiation
        && (existing.Filter?.LifecycleFilterPredicate as LifecyclePrefixPredicate)?.Prefix
            == ((LifecyclePrefixPredicate) desired.Filter!.LifecycleFilterPredicate).Prefix;
}
