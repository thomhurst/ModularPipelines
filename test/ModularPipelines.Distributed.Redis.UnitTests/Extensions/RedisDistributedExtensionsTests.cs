using System.IO.Compression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Caching;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Redis;
using ModularPipelines.Distributed.Redis.Artifacts;
using ModularPipelines.Distributed.Redis.Caching;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using Moq;
using Respire;

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
    public async Task DistributedOptionsUseSharedOptionsPipeline()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddDistributedMode(options => options with { RunId = "test-run" });
        builder.AddRedisDistributed(options => options with { ConnectionString = "unused" });
        builder.ConfigureDistributedOptions(options => options with { ArtifactCompressionLevel = CompressionLevel.NoCompression });
        await using var pipeline = await builder.BuildAsync();

        var configuredOptions = pipeline.Services.GetRequiredService<IOptions<DistributedOptions>>().Value;
        var directOptions = pipeline.Services.GetRequiredService<DistributedOptions>();

        using (Assert.Multiple())
        {
            await Assert.That(directOptions).IsSameReferenceAs(configuredOptions);
            await Assert.That(configuredOptions.ArtifactCompressionLevel)
                .IsEqualTo(CompressionLevel.NoCompression);
        }
    }

    [Test]
    public async Task ArtifactStoreRegistersStandaloneRedisDependencies()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.ConfigureDistributedOptions(options => options with { RunId = "artifact-run" });

        builder.AddRedisDistributedArtifactStore(options => options with { ConnectionString = "artifact-only" });

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
                descriptor.ServiceType == typeof(IRespireClient))).IsFalse();
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
            builder.AddRedisDistributedArtifactStore(options => options with { ConnectionString = "artifact-only" });

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
        builder.AddDistributedMode(options => options with { RunId = "current-run" });
        builder.AddRedisDistributedCoordinator(options => options with
        {
            ConnectionString = "unused",
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
        builder.AddRedisDistributedCoordinator(options => options with { ConnectionString = "unused" });
        builder.AddDistributedMode(options => options with { RunId = "configured-after-redis" });

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
            builder.AddDistributedMode(_ => _);

            builder.AddRedisDistributedCoordinator(options => options with { ConnectionString = "unused" });

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
            builder.AddDistributedMode(options => options with { TotalInstances = 2 });
            builder.AddRedisDistributedCoordinator(options => options with { ConnectionString = "unused" });

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
    public async Task Redis_Features_Do_Not_Adopt_An_Application_Client()
    {
        var applicationConnection = new Mock<IRespireClient>().Object;
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddDistributedMode(options => options with { RunId = "own-connection" });
        builder.Services.AddSingleton(applicationConnection);
        builder.AddRedisDistributed(options => options with { ConnectionString = "unused" });

        await using var pipeline = await builder.BuildAsync();

        using (Assert.Multiple())
        {
            await Assert.That(pipeline.Services.GetRequiredService<IRespireClient>())
                .IsSameReferenceAs(applicationConnection);
            await Assert.That(pipeline.Services.GetRequiredService<RedisConnectionProvider>()).IsNotNull();
            await Assert.That(builder.Services.Count(descriptor =>
                descriptor.ServiceType == typeof(IRespireClient))).IsEqualTo(1);
        }
    }

    [Test]
    public async Task Artifact_Store_Uses_Its_Own_Connection()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddDistributedMode(options => options with { RunId = "separate-connections" });
        builder.AddRedisDistributed(options => options with { ConnectionString = "unused" });

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
            ConfigureConnection = connection => connection with
            {
                Password = "key,with=special;characters",
                UseTls = true,
            },
        };

        var configuration = RedisConnectionProvider.CreateConfiguration(options);

        using (Assert.Multiple())
        {
            await Assert.That(configuration.Endpoints).IsEquivalentTo([new RespireEndpoint("redis.example", 6380)]);
            await Assert.That(configuration.Password).IsEqualTo("key,with=special;characters");
            await Assert.That(configuration.UseTls).IsTrue();
        }
    }

    [Test]
    [Arguments("localhost:6379,abortConnect=false")]
    [Arguments("localhost:6379,connectTimeout=soon")]
    public async Task Invalid_Connection_String_Fails_Validation_At_Startup(string connectionString)
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddDistributedMode(options => options with { RunId = "invalid-connection-string" });
        builder.AddRedisDistributed(options => options with { ConnectionString = connectionString });

        await Assert.That(async () => await builder.BuildAsync())
            .Throws<OptionsValidationException>()
            .WithMessageContaining($"{nameof(RedisOptions)}.{nameof(RedisOptions.ConnectionString)} is invalid");
    }

    [Test]
    public async Task Missing_Connection_Fails_Validation_At_Startup()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddDistributedMode(options => options with { RunId = "validation-run" });
        builder.AddRedisDistributedArtifactStore(_ => _);

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
        RedisOptions Configure(RedisOptions options) => options with
        {
            ConnectionString = mode == "remove" ? "localhost:6379" : string.Empty,
            ConfigureConnection = mode switch
            {
                "noop" => connection => connection,
                "tls" => connection => connection with { UseTls = true },
                "remove" => connection => connection with { Endpoints = [] },
                _ => null,
            },
        };

        if (moduleCache)
        {
            builder.AddRedisModuleCache(Configure);
        }
        else
        {
            builder.AddDistributedMode(options => options with { RunId = "endpoint-validation" });
            builder.AddRedisDistributed(Configure);
        }

        var exception = await Assert.ThrowsAsync<OptionsValidationException>(() => builder.BuildAsync());
        await Assert.That(exception!.Message).Contains(nameof(RedisOptions.ConnectionString));
        await Assert.That(exception.OptionsName)
            .IsEqualTo(moduleCache ? "ModularPipelines.RedisModuleCache" : string.Empty);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Valid_Cache_Only_Configuration_Builds_Without_Connecting(bool callbackOnly)
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddRedisModuleCache(options => options with
        {
            TimeToLive = TimeSpan.FromMinutes(1),
            ConfigureConnection = callbackOnly
                ? connection => connection with { Endpoints = [new RespireEndpoint("cache.invalid", 6381)] }
                : null,
            ConnectionString = callbackOnly ? string.Empty : "cache.invalid:6381",
        });

        await using var pipeline = await builder.BuildAsync();
        var cache = pipeline.Services.GetRequiredService<IOptionsMonitor<RedisOptions>>()
            .Get("ModularPipelines.RedisModuleCache");
        await Assert.That(RedisConnectionProvider.CreateConfiguration(cache).Endpoints.Count).IsEqualTo(1);
        await Assert.That(cache.TimeToLive).IsEqualTo(TimeSpan.FromMinutes(1));
    }

    [Test]
    [Arguments(false, "default")]
    [Arguments(false, "null")]
    [Arguments(false, "transform")]
    [Arguments(true, "default")]
    [Arguments(true, "null")]
    [Arguments(true, "transform")]
    public async Task ModuleCache_Overloads_Preserve_Registration_And_Cache_Options(
        bool useConfigurationSection,
        string cacheConfiguration)
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        var section = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Redis:ConnectionString"] = "cache.invalid:6381",
                ["Redis:KeyPrefix"] = "cache-only",
            })
            .Build()
            .GetSection("Redis");
        RedisOptions ConfigureRedis(RedisOptions options) => options with
        {
            ConnectionString = "cache.invalid:6381",
            KeyPrefix = "cache-only",
        };
        ModuleCacheOptions ConfigureCache(ModuleCacheOptions options) => options with { MaxInputFiles = 42 };

        var returnedBuilder = (useConfigurationSection, cacheConfiguration) switch
        {
            (false, "default") => builder.AddRedisModuleCache(ConfigureRedis),
            (false, "null") => builder.AddRedisModuleCache(ConfigureRedis, null),
            (false, "transform") => builder.AddRedisModuleCache(ConfigureRedis, ConfigureCache),
            (true, "default") => builder.AddRedisModuleCache(section),
            (true, "null") => builder.AddRedisModuleCache(section, null),
            (true, "transform") => builder.AddRedisModuleCache(section, ConfigureCache),
            _ => throw new ArgumentOutOfRangeException(nameof(cacheConfiguration)),
        };

        await using var pipeline = await builder.BuildAsync();
        var redisOptions = pipeline.Services.GetRequiredService<IOptionsMonitor<RedisOptions>>()
            .Get("ModularPipelines.RedisModuleCache");
        var cacheOptions = pipeline.Services.GetRequiredService<IOptions<ModuleCacheOptions>>().Value;

        using (Assert.Multiple())
        {
            await Assert.That(returnedBuilder).IsSameReferenceAs(builder);
            await Assert.That(redisOptions.ConnectionString).IsEqualTo("cache.invalid:6381");
            await Assert.That(redisOptions.KeyPrefix).IsEqualTo("cache-only");
            await Assert.That(cacheOptions.MaxInputFiles)
                .IsEqualTo(cacheConfiguration == "transform" ? 42 : new ModuleCacheOptions().MaxInputFiles);
            await Assert.That(pipeline.Services.GetRequiredService<IModuleCacheStore>()).IsTypeOf<RedisModuleCache>();
            await Assert.That(builder.Services.Any(descriptor =>
                descriptor.ServiceType == typeof(IDistributedCoordinatorFactory))).IsFalse();
        }
    }

    [Test]
    public async Task Validated_Configuration_Is_Reused_Without_Reinvoking_Callback()
    {
        var calls = 0;
        RespireOptions? callbackConfiguration = null;
        var options = new RedisOptions
        {
            ConnectionString = "localhost:6379",
            ConfigureConnection = connection =>
            {
                Interlocked.Increment(ref calls);
                callbackConfiguration = connection with
                {
                    Password = "key,with=special;characters",
                    UseTls = true,
                };
                return callbackConfiguration;
            },
        };
        var validator = new RedisOptionsValidator(
            Microsoft.Extensions.Options.Options.DefaultName,
            Microsoft.Extensions.Options.Options.Create(new DistributedOptions()));

        var result = validator.Validate(Microsoft.Extensions.Options.Options.DefaultName, options);
        await Assert.That(result.Succeeded).IsTrue();
        await Assert.That(calls).IsEqualTo(1);

        // Neither the options the callback retained nor a returned copy can change the validated snapshot.
        callbackConfiguration!.Endpoints.Clear();
        var first = RedisConnectionProvider.CreateConfiguration(options);
        first.Endpoints.Clear();
        first.ReplicaEndpoints.Add(new RespireEndpoint("replica.invalid", 6379));
        await using var provider = new RedisConnectionProvider(options);
        var second = RedisConnectionProvider.CreateConfiguration(options);

        using (Assert.Multiple())
        {
            await Assert.That(calls).IsEqualTo(1);
            await Assert.That(second.Endpoints).IsEquivalentTo([new RespireEndpoint("localhost", 6379)]);
            await Assert.That(second.ReplicaEndpoints).IsEmpty();
            await Assert.That(second.Password).IsEqualTo("key,with=special;characters");
            await Assert.That(second.UseTls).IsTrue();
        }
    }

    [Test]
    public async Task Callback_Returning_Null_Fails_Validation()
    {
        var options = new RedisOptions
        {
            ConnectionString = "localhost:6379",
            ConfigureConnection = _ => null!,
        };

        await Assert.That(() => RedisConnectionProvider.CreateConfiguration(options))
            .Throws<InvalidOperationException>()
            .WithMessageContaining(nameof(RedisOptions.ConfigureConnection));
    }

    [Test]
    public async Task Callback_Endpoints_Respect_PostConfiguration_And_Named_Cache_Isolation()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddDistributedMode(options => options with { RunId = "callback-endpoints" });
        builder.AddRedisDistributed(options => options with { ConfigureConnection = connection => connection });
        builder.Services.PostConfigure<RedisOptions>(options =>
            typeof(RedisOptions).GetProperty(nameof(RedisOptions.ConfigureConnection))!.SetValue(
                options,
                (Func<RespireOptions, RespireOptions>) (connection => connection with
                {
                    Endpoints = [new RespireEndpoint("coordinator.invalid", 6380)],
                })));
        builder.AddRedisModuleCache(options => options with
        {
            TimeToLive = TimeSpan.FromMinutes(1),
            ConfigureConnection = connection => connection with { Endpoints = [new RespireEndpoint("cache.invalid", 6381)] },
        });

        await using var pipeline = await builder.BuildAsync();
        var distributed = pipeline.Services.GetRequiredService<IOptions<RedisOptions>>().Value;
        var cache = pipeline.Services.GetRequiredService<IOptionsMonitor<RedisOptions>>()
            .Get("ModularPipelines.RedisModuleCache");

        using (Assert.Multiple())
        {
            await Assert.That(RedisConnectionProvider.CreateConfiguration(distributed).Endpoints.Single())
                .IsEqualTo(new RespireEndpoint("coordinator.invalid", 6380));
            await Assert.That(RedisConnectionProvider.CreateConfiguration(cache).Endpoints.Single())
                .IsEqualTo(new RespireEndpoint("cache.invalid", 6381));
            await Assert.That(cache.TimeToLive).IsEqualTo(TimeSpan.FromMinutes(1));
        }
    }

    [Test]
    public async Task TimeToLive_Must_Exceed_ModuleResultTimeout()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<NoOpModule>();
        builder.AddDistributedMode(options => options with
        {
            RunId = "ttl-run",
            ModuleResultTimeout = TimeSpan.FromHours(2),
        });
        builder.AddRedisDistributedCoordinator(options => options with
        {
            ConnectionString = "unused",
            TimeToLive = TimeSpan.FromHours(1),
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
        builder.AddDistributedMode(options => options with
        {
            RunId = "ttl-master-run",
            ModuleResultTimeout = TimeSpan.FromMinutes(1),
            MasterTimeout = TimeSpan.FromHours(2),
        });
        builder.AddRedisDistributedCoordinator(options => options with
        {
            ConnectionString = "unused",
            TimeToLive = TimeSpan.FromHours(1),
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
        builder.AddDistributedMode(options => options with { RunId = "twice" });
        builder.AddRedisDistributed(options => options with { ConnectionString = "unused" });
        builder.AddRedisDistributedArtifactStore(options => options with { KeyPrefix = "later" });

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

        await Assert.That(() => builder.AddRedisDistributedArtifactStore(options => options with { ConnectionString = "unused" }))
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
