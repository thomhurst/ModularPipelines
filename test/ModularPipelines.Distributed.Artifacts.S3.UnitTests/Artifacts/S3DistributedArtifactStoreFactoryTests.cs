using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed.Artifacts.S3.Artifacts;
using Moq;

namespace ModularPipelines.Distributed.Artifacts.S3.UnitTests.Artifacts;

public class S3DistributedArtifactStoreFactoryTests
{
    private const string OurRuleId = "modpipe-artifact-expiration:modpipe/artifacts/";

    private readonly Mock<IAmazonS3> _s3 = new();
    private readonly Mock<ILogger<S3DistributedArtifactStoreFactory>> _logger = new();

    [Test]
    public async Task Lifecycle_Rule_Is_Not_Touched_By_Default()
    {
        await CreateStoreAsync(new S3StorageOptions { BucketName = "bucket" });

        _s3.Verify(s => s.GetLifecycleConfigurationAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _s3.Verify(s => s.PutLifecycleConfigurationAsync(It.IsAny<PutLifecycleConfigurationRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task Lifecycle_Rule_Is_Merged_With_Existing_Rules()
    {
        var unrelated = new LifecycleRule { Id = "keep-me", Status = LifecycleRuleStatus.Enabled };
        var staleOurs = new LifecycleRule { Id = OurRuleId, Status = LifecycleRuleStatus.Disabled };
        _s3.Setup(s => s.GetLifecycleConfigurationAsync("bucket", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetLifecycleConfigurationResponse
            {
                Configuration = new LifecycleConfiguration { Rules = [unrelated, staleOurs] },
            });
        PutLifecycleConfigurationRequest? put = null;
        _s3.Setup(s => s.PutLifecycleConfigurationAsync(It.IsAny<PutLifecycleConfigurationRequest>(), It.IsAny<CancellationToken>()))
            .Callback<PutLifecycleConfigurationRequest, CancellationToken>((request, _) => put = request)
            .ReturnsAsync(new PutLifecycleConfigurationResponse());

        await CreateStoreAsync(new S3StorageOptions
        {
            BucketName = "bucket",
            SetLifecycleRule = true,
            TimeToLive = TimeSpan.FromHours(36),
        });

        await Assert.That(put).IsNotNull();
        var rules = put!.Configuration.Rules;
        var ours = rules.Single(rule => rule.Id == OurRuleId);
        using (Assert.Multiple())
        {
            await Assert.That(rules.Count).IsEqualTo(2);
            await Assert.That(rules).Contains(unrelated);
            await Assert.That(ours.Status).IsEqualTo(LifecycleRuleStatus.Enabled);
            await Assert.That(ours.Expiration.Days).IsEqualTo(2);
            await Assert.That(ours.AbortIncompleteMultipartUpload.DaysAfterInitiation).IsEqualTo(1);
            await Assert.That(((LifecyclePrefixPredicate) ours.Filter.LifecycleFilterPredicate).Prefix)
                .IsEqualTo("modpipe/artifacts/");
        }
    }

    [Test]
    public async Task Lifecycle_Rule_Is_Created_When_Bucket_Has_No_Configuration()
    {
        _s3.Setup(s => s.GetLifecycleConfigurationAsync("bucket", It.IsAny<CancellationToken>()))
            .ThrowsAsync(new AmazonS3Exception("none")
            {
                StatusCode = HttpStatusCode.NotFound,
                ErrorCode = "NoSuchLifecycleConfiguration",
            });
        PutLifecycleConfigurationRequest? put = null;
        _s3.Setup(s => s.PutLifecycleConfigurationAsync(It.IsAny<PutLifecycleConfigurationRequest>(), It.IsAny<CancellationToken>()))
            .Callback<PutLifecycleConfigurationRequest, CancellationToken>((request, _) => put = request)
            .ReturnsAsync(new PutLifecycleConfigurationResponse());

        await CreateStoreAsync(new S3StorageOptions { BucketName = "bucket", SetLifecycleRule = true });

        await Assert.That(put!.Configuration.Rules.Select(rule => rule.Id)).IsEquivalentTo([OurRuleId]);
    }

    [Test]
    public async Task Unchanged_Lifecycle_Rule_Is_Not_Rewritten()
    {
        _s3.Setup(s => s.GetLifecycleConfigurationAsync("bucket", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetLifecycleConfigurationResponse
            {
                Configuration = new LifecycleConfiguration
                {
                    Rules =
                    [
                        new LifecycleRule
                        {
                            Id = OurRuleId,
                            Status = LifecycleRuleStatus.Enabled,
                            Filter = new LifecycleFilter
                            {
                                LifecycleFilterPredicate = new LifecyclePrefixPredicate { Prefix = "modpipe/artifacts/" },
                            },
                            Expiration = new LifecycleRuleExpiration { Days = 1 },
                            AbortIncompleteMultipartUpload = new LifecycleRuleAbortIncompleteMultipartUpload { DaysAfterInitiation = 1 },
                        },
                    ],
                },
            });

        await CreateStoreAsync(new S3StorageOptions { BucketName = "bucket", SetLifecycleRule = true });

        _s3.Verify(s => s.PutLifecycleConfigurationAsync(It.IsAny<PutLifecycleConfigurationRequest>(), It.IsAny<CancellationToken>()), Times.Never);
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

        var store = await CreateStoreAsync(new S3StorageOptions { BucketName = "bucket", SetLifecycleRule = true });

        await Assert.That(store).IsNotNull();
        _s3.Verify(s => s.PutLifecycleConfigurationAsync(It.IsAny<PutLifecycleConfigurationRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        _logger.Verify(logger => logger.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<AmazonS3Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    private async Task<IDistributedArtifactStore> CreateStoreAsync(S3StorageOptions options)
    {
        var factory = new S3DistributedArtifactStoreFactory(
            Microsoft.Extensions.Options.Options.Create(options),
            Microsoft.Extensions.Options.Options.Create(new DistributedOptions { RunId = "run" }),
            _logger.Object,
            _ => _s3.Object);
        return await factory.CreateAsync(CancellationToken.None);
    }
}
