using System.IO.Compression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Redis;
using ModularPipelines.Distributed.Redis.Artifacts;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using Moq;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Redis.UnitTests.Extensions;

[TUnit.Core.NotInParallel("ProcessEnvironment")]
public class RedisDistributedExtensionsTests
{
    private static readonly string[] ExecutionEnvironmentVariables =
    [
        "GITHUB_RUN_ID",
        "GITHUB_RUN_ATTEMPT",
        "MODULARPIPELINES_RUN_ID",
        "BUILD_BUILDID",
        "CI_PIPELINE_ID",
    ];

    [Test]
    public async Task ArtifactOptionsUseSharedOptionsPipeline()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddDistributedMode(options => options.RunId = "test-run");
        builder.AddRedisDistributed(options => options.ConnectionString = "unused");
        builder.Services.Configure<ArtifactOptions>(options => options.CompressionLevel = CompressionLevel.NoCompression);
        await using var pipeline = await builder.BuildAsync();

        var configuredOptions = pipeline.Services.GetRequiredService<IOptions<ArtifactOptions>>().Value;
        var directOptions = pipeline.Services.GetRequiredService<ArtifactOptions>();

        using (Assert.Multiple())
        {
            await Assert.That(directOptions).IsSameReferenceAs(configuredOptions);
            await Assert.That(configuredOptions.CompressionLevel)
                .IsEqualTo(CompressionLevel.NoCompression);
        }
    }

    [Test]
    public async Task ArtifactStoreRegistersStandaloneRedisDependencies()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.Services.Configure<DistributedOptions>(options => options.RunId = "artifact-run");

        builder.AddRedisDistributedArtifactStore(options =>
            options.ConnectionString = "artifact-only");

        await using var pipeline = await builder.BuildAsync();
        var redisOptions = pipeline.Services.GetRequiredService<IOptions<RedisOptions>>().Value;
        var distributedOptions = pipeline.Services.GetRequiredService<IOptions<DistributedOptions>>().Value;

        using (Assert.Multiple())
        {
            await Assert.That(redisOptions.ConnectionString).IsEqualTo("artifact-only");
            await Assert.That(distributedOptions.RunId).IsEqualTo("artifact-run");
            await Assert.That(builder.Services.Any(descriptor =>
                descriptor.ServiceType == typeof(RedisConnectionProvider))).IsTrue();
            await Assert.That(builder.Services.Any(descriptor =>
                descriptor.ServiceType == typeof(IConnectionMultiplexer))).IsFalse();
            await Assert.That(builder.Services.Any(descriptor =>
                descriptor.ServiceType == typeof(IDistributedArtifactStoreFactory))).IsTrue();
        }
    }

    [Test]
    public async Task ArtifactStore_Rejects_Unconfigured_RunId()
    {
        var originals = ExecutionEnvironmentVariables.ToDictionary(
            name => name,
            Environment.GetEnvironmentVariable);

        try
        {
            foreach (var name in ExecutionEnvironmentVariables)
            {
                Environment.SetEnvironmentVariable(name, null);
            }

            var builder = Pipeline.CreateBuilder();
            builder.AddModule<NoOpModule>();
            builder.AddRedisDistributedArtifactStore(options =>
                options.ConnectionString = "artifact-only");

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => builder.BuildAsync());

            await Assert.That(exception!.Message).Contains(nameof(DistributedOptions.RunId));
        }
        finally
        {
            foreach (var (name, value) in originals)
            {
                Environment.SetEnvironmentVariable(name, value);
            }
        }
    }

    [Test]
    public async Task Coordinator_Uses_Core_RunId()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddDistributedMode(options => options.RunId = "current-run");
        builder.AddRedisDistributedCoordinator(options =>
        {
            options.ConnectionString = "unused";
        });
        await using var pipeline = await builder.BuildAsync();

        var distributedOptions = pipeline.Services
            .GetRequiredService<IOptions<DistributedOptions>>()
            .Value;
        await Assert.That(distributedOptions.RunId).IsEqualTo("current-run");
        await Assert.That(typeof(RedisOptions).GetProperty("RunIdentifier")).IsNull();
    }

    [Test]
    public async Task Coordinator_Allows_RunId_Configured_After_Redis_Registration()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddRedisDistributedCoordinator(options =>
            options.ConnectionString = "unused");
        builder.AddDistributedMode(options => options.RunId = "configured-after-redis");

        await using var pipeline = await builder.BuildAsync();
        var options = pipeline.Services.GetRequiredService<IOptions<DistributedOptions>>().Value;

        await Assert.That(options.RunId).IsEqualTo("configured-after-redis");
    }

    [Test]
    public async Task Coordinator_Rejects_Unconfigured_Single_Instance_Run()
    {
        var originals = ExecutionEnvironmentVariables.ToDictionary(
            name => name,
            Environment.GetEnvironmentVariable);

        try
        {
            foreach (var name in ExecutionEnvironmentVariables)
            {
                Environment.SetEnvironmentVariable(name, null);
            }

            var builder = Pipeline.CreateBuilder();
            builder.AddModule<NoOpModule>();
            builder.AddDistributedMode(_ => { });

            builder.AddRedisDistributedCoordinator(options =>
                options.ConnectionString = "unused");

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => builder.BuildAsync());

            using (Assert.Multiple())
            {
                await Assert.That(exception!.Message).Contains(nameof(DistributedOptions.RunId));
                await Assert.That(exception.Message).Contains("MODULARPIPELINES_RUN_ID");
            }
        }
        finally
        {
            foreach (var (name, value) in originals)
            {
                Environment.SetEnvironmentVariable(name, value);
            }
        }
    }

    [Test]
    public async Task Distributed_Mode_Rejects_Unconfigured_Multi_Instance_Run()
    {
        var originals = ExecutionEnvironmentVariables.ToDictionary(
            name => name,
            Environment.GetEnvironmentVariable);

        try
        {
            foreach (var name in ExecutionEnvironmentVariables)
            {
                Environment.SetEnvironmentVariable(name, null);
            }

            var builder = Pipeline.CreateBuilder();
            builder.AddDistributedMode(options => options.TotalInstances = 2);
            builder.AddRedisDistributedCoordinator(options =>
                options.ConnectionString = "unused");

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => builder.BuildAsync());

            using (Assert.Multiple())
            {
                await Assert.That(exception!.Message).Contains(nameof(DistributedOptions.RunId));
                await Assert.That(exception.Message).Contains("MODULARPIPELINES_RUN_ID");
            }
        }
        finally
        {
            foreach (var (name, value) in originals)
            {
                Environment.SetEnvironmentVariable(name, value);
            }
        }
    }

    [Test]
    public async Task ConfigurationSectionBindsRedisOptions()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Redis:ConnectionString"] = "redis.example:6380",
                ["Distributed:RunId"] = "configured-run",
            })
            .Build();
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddDistributedMode(configuration.GetSection("Distributed"));

        builder.AddRedisDistributedCoordinator(configuration.GetSection("Redis"));
        await using var pipeline = await builder.BuildAsync();
        var redisOptions = pipeline.Services.GetRequiredService<IOptions<RedisOptions>>().Value;
        var distributedOptions = pipeline.Services.GetRequiredService<IOptions<DistributedOptions>>().Value;

        using (Assert.Multiple())
        {
            await Assert.That(redisOptions.ConnectionString).IsEqualTo("redis.example:6380");
            await Assert.That(distributedOptions.RunId).IsEqualTo("configured-run");
        }
    }

    [Test]
    public async Task Redis_Features_Do_Not_Adopt_An_Application_Multiplexer()
    {
        var applicationConnection = new Mock<IConnectionMultiplexer>().Object;
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddDistributedMode(options => options.RunId = "own-connection");
        builder.Services.AddSingleton(applicationConnection);
        builder.AddRedisDistributed(options => options.ConnectionString = "unused");

        await using var pipeline = await builder.BuildAsync();

        using (Assert.Multiple())
        {
            await Assert.That(pipeline.Services.GetRequiredService<IConnectionMultiplexer>())
                .IsSameReferenceAs(applicationConnection);
            await Assert.That(pipeline.Services.GetRequiredService<RedisConnectionProvider>()).IsNotNull();
            await Assert.That(builder.Services.Count(descriptor =>
                descriptor.ServiceType == typeof(IConnectionMultiplexer))).IsEqualTo(1);
        }
    }

    [Test]
    public async Task Artifact_Store_Uses_Its_Own_Connection()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddDistributedMode(options => options.RunId = "separate-connections");
        builder.AddRedisDistributed(options => options.ConnectionString = "unused");

        await using var pipeline = await builder.BuildAsync();

        var coordinatorConnection = pipeline.Services.GetRequiredService<RedisConnectionProvider>();
        var artifactConnection = pipeline.Services.GetRequiredKeyedService<RedisConnectionProvider>(
            RedisDistributedExtensions.ArtifactConnectionKey);
        await Assert.That(artifactConnection).IsNotSameReferenceAs(coordinatorConnection);
        await Assert.That(pipeline.Services.GetRequiredService<IDistributedArtifactStoreFactory>())
            .IsTypeOf<RedisDistributedArtifactStoreFactory>();
    }

    [Test]
    public async Task ConfigureConnection_Is_Applied_To_Parsed_Connection_String()
    {
        var options = new RedisOptions
        {
            ConnectionString = "redis.example:6380",
            ConfigureConnection = connection =>
            {
                connection.Password = "key,with=special;characters";
                connection.Ssl = true;
            },
        };

        var configuration = RedisConnectionProvider.CreateConfiguration(options);

        using (Assert.Multiple())
        {
            await Assert.That(configuration.EndPoints.Count).IsEqualTo(1);
            await Assert.That(configuration.Password).IsEqualTo("key,with=special;characters");
            await Assert.That(configuration.Ssl).IsTrue();
        }
    }

    [Test]
    public async Task Missing_Connection_Fails_Validation_At_Startup()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddDistributedMode(options => options.RunId = "validation-run");
        builder.AddRedisDistributedArtifactStore(_ => { });

        await Assert.That(async () => await builder.BuildAsync())
            .Throws<OptionsValidationException>()
            .WithMessageContaining(nameof(RedisOptions.ConnectionString));
    }

    [Test]
    [Arguments("empty", false)]
    [Arguments("noop", false)]
    [Arguments("tls", false)]
    [Arguments("remove", false)]
    [Arguments("empty", true)]
    [Arguments("noop", true)]
    [Arguments("tls", true)]
    [Arguments("remove", true)]
    public async Task Effective_Connection_Requires_Endpoints_At_Startup(string mode, bool moduleCache)
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddDistributedMode(options => options.RunId = "endpoint-validation");
        void Configure(RedisOptions options)
        {
            options.ConnectionString = mode == "remove" ? "localhost:6379" : string.Empty;
            options.ConfigureConnection = mode switch
            {
                "noop" => _ =>
                {
                }
                ,
                "tls" => connection => connection.Ssl = true,
                "remove" => connection => connection.EndPoints.Clear(),
                _ => null,
            };
        }

        if (moduleCache)
        {
            builder.AddRedisDistributed(options => options.ConnectionString = "unused");
            builder.AddRedisModuleCache(Configure);
        }
        else
        {
            builder.AddRedisDistributed(Configure);
        }

        var exception = await Assert.ThrowsAsync<OptionsValidationException>(() => builder.BuildAsync());
        await Assert.That(exception!.Message).Contains(nameof(RedisOptions.ConnectionString));
        await Assert.That(exception.OptionsName)
            .IsEqualTo(moduleCache ? "ModularPipelines.RedisModuleCache" : string.Empty);
    }

    [Test]
    public async Task Validated_Configuration_Is_Reused_Without_Reinvoking_Callback()
    {
        var calls = 0;
        ConfigurationOptions? callbackConfiguration = null;
        var options = new RedisOptions
        {
            ConnectionString = "localhost:6379",
            ConfigureConnection = connection =>
            {
                Interlocked.Increment(ref calls);
                callbackConfiguration = connection;
                connection.Password = "key,with=special;characters";
                connection.Ssl = true;
            },
        };
        var validator = new RedisOptionsValidator(Microsoft.Extensions.Options.Options.Create(new DistributedOptions()));

        var result = validator.Validate(Microsoft.Extensions.Options.Options.DefaultName, options);
        await Assert.That(result.Succeeded).IsTrue();
        await Assert.That(calls).IsEqualTo(1);

        callbackConfiguration!.EndPoints.Clear();
        var first = RedisConnectionProvider.CreateConfiguration(options);
        first.EndPoints.Clear();
        first.Password = "changed";
        await using var provider = new RedisConnectionProvider(options);
        var second = RedisConnectionProvider.CreateConfiguration(options);

        using (Assert.Multiple())
        {
            await Assert.That(calls).IsEqualTo(1);
            await Assert.That(second.EndPoints.Count).IsEqualTo(1);
            await Assert.That(second.Password).IsEqualTo("key,with=special;characters");
            await Assert.That(second.Ssl).IsTrue();
        }
    }

    [Test]
    public async Task Callback_Endpoints_Respect_PostConfiguration_And_Named_Cache_Isolation()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddDistributedMode(options => options.RunId = "callback-endpoints");
        builder.AddRedisDistributed(options => options.ConfigureConnection = _ => { });
        builder.Services.PostConfigure<RedisOptions>(options =>
            options.ConfigureConnection = connection => connection.EndPoints.Add("coordinator.invalid", 6380));
        builder.AddRedisModuleCache(options =>
        {
            options.TimeToLive = TimeSpan.FromMinutes(1);
            options.ConfigureConnection = connection => connection.EndPoints.Add("cache.invalid", 6381);
        });

        await using var pipeline = await builder.BuildAsync();
        var distributed = pipeline.Services.GetRequiredService<IOptions<RedisOptions>>().Value;
        var cache = pipeline.Services.GetRequiredService<IOptionsMonitor<RedisOptions>>()
            .Get("ModularPipelines.RedisModuleCache");

        using (Assert.Multiple())
        {
            await Assert.That(RedisConnectionProvider.CreateConfiguration(distributed).EndPoints.Single().ToString())
                .IsEqualTo("Unspecified/coordinator.invalid:6380");
            await Assert.That(RedisConnectionProvider.CreateConfiguration(cache).EndPoints.Single().ToString())
                .IsEqualTo("Unspecified/cache.invalid:6381");
            await Assert.That(cache.TimeToLive).IsEqualTo(TimeSpan.FromMinutes(1));
        }
    }

    [Test]
    public async Task TimeToLive_Must_Exceed_ModuleResultTimeout()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddDistributedMode(options =>
        {
            options.RunId = "ttl-run";
            options.ModuleResultTimeout = TimeSpan.FromHours(2);
        });
        builder.AddRedisDistributedCoordinator(options =>
        {
            options.ConnectionString = "unused";
            options.TimeToLive = TimeSpan.FromHours(1);
        });

        await Assert.That(async () => await builder.BuildAsync())
            .Throws<OptionsValidationException>()
            .WithMessageContaining(nameof(DistributedOptions.ModuleResultTimeout));
    }

    [Test]
    public async Task TimeToLive_Must_Exceed_MasterTimeout()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddDistributedMode(options =>
        {
            options.RunId = "ttl-master-run";
            options.ModuleResultTimeout = TimeSpan.FromMinutes(1);
            options.MasterTimeout = TimeSpan.FromHours(2);
        });
        builder.AddRedisDistributedCoordinator(options =>
        {
            options.ConnectionString = "unused";
            options.TimeToLive = TimeSpan.FromHours(1);
        });

        await Assert.That(async () => await builder.BuildAsync())
            .Throws<OptionsValidationException>()
            .WithMessageContaining(nameof(DistributedOptions.MasterTimeout));
    }

    [Test]
    public async Task Registering_Redis_Twice_Is_Idempotent()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddDistributedMode(options => options.RunId = "twice");
        builder.AddRedisDistributed(options => options.ConnectionString = "unused");
        builder.AddRedisDistributedArtifactStore(options => options.KeyPrefix = "later");

        await using var pipeline = await builder.BuildAsync();

        using (Assert.Multiple())
        {
            await Assert.That(builder.Services.Count(descriptor =>
                descriptor.ServiceType == typeof(IDistributedArtifactStoreFactory))).IsEqualTo(1);
            await Assert.That(builder.Services.Count(descriptor =>
                descriptor.ServiceType == typeof(IDistributedCoordinatorFactory))).IsEqualTo(1);
            await Assert.That(pipeline.Services.GetRequiredService<IOptions<RedisOptions>>().Value.KeyPrefix)
                .IsEqualTo("later");
        }
    }

    [Test]
    public async Task A_Second_Artifact_Store_Backend_Is_Rejected()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddDistributedArtifactStoreFactory<OtherArtifactStoreFactory>();

        await Assert.That(() => builder.AddRedisDistributedArtifactStore(options => options.ConnectionString = "unused"))
            .Throws<InvalidOperationException>()
            .WithMessageContaining(nameof(OtherArtifactStoreFactory));
    }

    private sealed class OtherArtifactStoreFactory : IDistributedArtifactStoreFactory
    {
        public Task<IDistributedArtifactStore> CreateAsync(CancellationToken cancellationToken) =>
            throw new NotSupportedException();
    }

    private sealed class NoOpModule : Module<int>
    {
        protected override Task<int> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult(0);
    }
}
