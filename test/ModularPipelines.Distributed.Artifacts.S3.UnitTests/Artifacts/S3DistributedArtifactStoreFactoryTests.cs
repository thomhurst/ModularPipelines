using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed.Artifacts.S3.Artifacts;
using Moq;

namespace ModularPipelines.Distributed.Artifacts.S3.UnitTests.Artifacts;

public class S3DistributedArtifactStoreFactoryTests
{
    private const string ArtifactPrefix = "modpipe/artifacts/";
    private const string OurRuleId = "modpipe-artifact-expiration:" + ArtifactPrefix;

    private readonly Mock<IAmazonS3> _s3 = new();
    private readonly Mock<ILogger<S3DistributedArtifactStoreFactory>> _logger = new();

    [Test]
    public async Task Run_Cleanup_Fallback_Uses_The_Factory_Logger()
    {
        _logger.Setup(logger => logger.IsEnabled(LogLevel.Information)).Returns(true);
        _s3.Setup(s => s.ListVersionsAsync(It.IsAny<ListVersionsRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new AmazonS3Exception("Access Denied") { StatusCode = HttpStatusCode.Forbidden });
        _s3.Setup(s => s.ListObjectsV2Async(It.IsAny<ListObjectsV2Request>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListObjectsV2Response { S3Objects = [], IsTruncated = false });
        using var store = (S3DistributedArtifactStore) await CreateStoreAsync(new S3StorageOptions { BucketName = "bucket" });

        await store.CleanupRunAsync(
            new DistributedRunCleanupContext { RunId = "run", PipelineSucceeded = true },
            CancellationToken.None);

        await Assert.That(_logger.Invocations.Count(call =>
            call.Method.Name == nameof(ILogger.Log) && call.Arguments[0] is LogLevel.Information)).IsEqualTo(1);
    }

    [Test]
    public async Task Lifecycle_Rule_Is_Not_Touched_By_Default()
    {
        using var store = (S3DistributedArtifactStore) await CreateStoreAsync(new S3StorageOptions { BucketName = "bucket" });

        _s3.Verify(s => s.GetLifecycleConfigurationAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        VerifyNoLifecycleWrites();
        VerifyWarnings(Times.Never());
    }

    [Test]
    [Arguments("missing")]
    [Arguments("disabled")]
    [Arguments("expiration")]
    [Arguments("multipart")]
    [Arguments("prefix")]
    public async Task Missing_Or_Different_Lifecycle_Rule_Is_Logged_And_Not_Changed(string difference)
    {
        var unrelated = new LifecycleRule { Id = "keep-me", Status = LifecycleRuleStatus.Enabled };
        var ours = CreateRule();
        switch (difference)
        {
            case "disabled":
                ours.Status = LifecycleRuleStatus.Disabled;
                break;
            case "expiration":
                ours.Expiration.Days = 2;
                break;
            case "multipart":
                ours.AbortIncompleteMultipartUpload = null;
                break;
            case "prefix":
                ((LifecyclePrefixPredicate) ours.Filter.LifecycleFilterPredicate).Prefix = "other/artifacts/";
                break;
        }

        var rules = difference == "missing" ? new List<LifecycleRule> { unrelated } : [unrelated, ours];
        _s3.Setup(s => s.GetLifecycleConfigurationAsync("bucket", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetLifecycleConfigurationResponse
            {
                Configuration = new LifecycleConfiguration { Rules = rules },
            });

        using var store = (S3DistributedArtifactStore) await CreateStoreAsync(new S3StorageOptions { BucketName = "bucket", SetLifecycleRule = true });

        VerifyNoLifecycleWrites();
        VerifyWarnings(Times.Once());
        await Assert.That(rules.Count).IsEqualTo(difference == "missing" ? 1 : 2);
        await Assert.That(rules[0]).IsSameReferenceAs(unrelated);
        await Assert.That(_logger.Invocations.Single(call => call.Method.Name == nameof(ILogger.Log)).Arguments[2]?.ToString())
            .Contains("Provision the rule outside pipeline startup");
    }

    [Test]
    public async Task Missing_Lifecycle_Configuration_Is_Logged_And_Not_Created()
    {
        _s3.Setup(s => s.GetLifecycleConfigurationAsync("bucket", It.IsAny<CancellationToken>()))
            .ThrowsAsync(new AmazonS3Exception("none")
            {
                StatusCode = HttpStatusCode.NotFound,
                ErrorCode = "NoSuchLifecycleConfiguration",
            });

        using var store = (S3DistributedArtifactStore) await CreateStoreAsync(new S3StorageOptions { BucketName = "bucket", SetLifecycleRule = true });

        VerifyNoLifecycleWrites();
        VerifyWarnings(Times.Once());
    }

    [Test]
    [Arguments(24, 1, "modpipe", "modpipe/artifacts/")]
    [Arguments(36, 2, "/custom/", "custom/artifacts/")]
    [Arguments(1, 1, "modpipe", "modpipe/artifacts/")]
    public async Task Matching_Provisioned_Lifecycle_Rule_Does_Not_Warn(int hours, int days, string keyPrefix, string artifactPrefix)
    {
        var rule = CreateRule();
        rule.Id = "modpipe-artifact-expiration:" + artifactPrefix;
        ((LifecyclePrefixPredicate) rule.Filter.LifecycleFilterPredicate).Prefix = artifactPrefix;
        rule.Expiration.Days = days;
        _s3.Setup(s => s.GetLifecycleConfigurationAsync("bucket", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetLifecycleConfigurationResponse
            {
                Configuration = new LifecycleConfiguration { Rules = [rule] },
            });

        using var store = (S3DistributedArtifactStore) await CreateStoreAsync(new S3StorageOptions
        {
            BucketName = "bucket",
            SetLifecycleRule = true,
            TimeToLive = TimeSpan.FromHours(hours),
            KeyPrefix = keyPrefix,
        });

        _s3.Verify(s => s.GetLifecycleConfigurationAsync("bucket", It.IsAny<CancellationToken>()), Times.Once);
        VerifyNoLifecycleWrites();
        VerifyWarnings(Times.Never());
    }

    [Test]
    public async Task Concurrent_Distinct_Prefixes_Never_Write_Lifecycle_Configuration()
    {
        var bothReading = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var readers = 0;
        var unrelated = new LifecycleRule { Id = "admin-rule", Status = LifecycleRuleStatus.Enabled };
        var rules = new List<LifecycleRule> { unrelated };
        _s3.Setup(s => s.GetLifecycleConfigurationAsync("bucket", It.IsAny<CancellationToken>()))
            .Returns(async () =>
            {
                if (Interlocked.Increment(ref readers) == 2)
                {
                    bothReading.SetResult();
                }

                await bothReading.Task;
                return new GetLifecycleConfigurationResponse
                {
                    Configuration = new LifecycleConfiguration { Rules = rules },
                };
            });

        var stores = await Task.WhenAll(
            CreateStoreAsync(new S3StorageOptions { BucketName = "bucket", KeyPrefix = "first", SetLifecycleRule = true }),
            CreateStoreAsync(new S3StorageOptions { BucketName = "bucket", KeyPrefix = "second", SetLifecycleRule = true }));
        foreach (var store in stores.Cast<S3DistributedArtifactStore>())
        {
            store.Dispose();
        }

        VerifyNoLifecycleWrites();
        VerifyWarnings(Times.Exactly(2));
        await Assert.That(rules.Count).IsEqualTo(1);
        await Assert.That(rules[0]).IsSameReferenceAs(unrelated);
    }

    [Test]
    public async Task Unreadable_Lifecycle_Configuration_Is_Logged_And_Not_Overwritten()
    {
        _s3.Setup(s => s.GetLifecycleConfigurationAsync("bucket", It.IsAny<CancellationToken>()))
            .ThrowsAsync(new AmazonS3Exception("Access Denied")
            {
                StatusCode = HttpStatusCode.Forbidden,
                ErrorCode = "AccessDenied",
            });

        using var store = (S3DistributedArtifactStore) await CreateStoreAsync(new S3StorageOptions { BucketName = "bucket", SetLifecycleRule = true });

        VerifyNoLifecycleWrites();
        VerifyWarnings(Times.Once());
    }

    [Test]
    public async Task Canceled_Lifecycle_Check_Propagates_And_Disposes_Client()
    {
        using var cancellation = new CancellationTokenSource();
        await cancellation.CancelAsync();
        _s3.Setup(s => s.GetLifecycleConfigurationAsync("bucket", cancellation.Token))
            .ThrowsAsync(new OperationCanceledException(cancellation.Token));

        await Assert.That(async () =>
            {
                await CreateStoreAsync(new S3StorageOptions { BucketName = "bucket", SetLifecycleRule = true }, cancellation.Token);
            })
            .Throws<OperationCanceledException>();

        _s3.Verify(s => s.Dispose(), Times.Once);
        VerifyNoLifecycleWrites();
        VerifyWarnings(Times.Never());
    }

    private static LifecycleRule CreateRule() => new()
    {
        Id = OurRuleId,
        Status = LifecycleRuleStatus.Enabled,
        Filter = new LifecycleFilter
        {
            LifecycleFilterPredicate = new LifecyclePrefixPredicate { Prefix = ArtifactPrefix },
        },
        Expiration = new LifecycleRuleExpiration { Days = 1 },
        AbortIncompleteMultipartUpload = new LifecycleRuleAbortIncompleteMultipartUpload { DaysAfterInitiation = 1 },
    };

    private void VerifyNoLifecycleWrites() =>
        _s3.Verify(s => s.PutLifecycleConfigurationAsync(It.IsAny<PutLifecycleConfigurationRequest>(), It.IsAny<CancellationToken>()), Times.Never);

    private void VerifyWarnings(Times times) =>
        _logger.Verify(logger => logger.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            times);

    private async Task<IDistributedArtifactStore> CreateStoreAsync(S3StorageOptions options, CancellationToken cancellationToken = default)
    {
        var factory = new S3DistributedArtifactStoreFactory(
            Microsoft.Extensions.Options.Options.Create(options),
            Microsoft.Extensions.Options.Options.Create(new DistributedOptions { RunId = "run" }),
            _logger.Object,
            _ => _s3.Object);
        return await factory.CreateAsync(cancellationToken);
    }
}
