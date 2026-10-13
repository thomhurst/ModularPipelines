using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Distributed;

namespace ModularPipelines.UnitTests.Api;

public class ImmutableDistributedOptionsTests
{
    [Test]
    [MatrixDataSource]
    public async Task Later_Incompatible_Factory_Is_Rejected_By_Options_Accessors(
        [Matrix("type", "instance", "delegate")] string registration,
        [Matrix("add", "replace")] string operation,
        [Matrix("configure", "postconfigure")] string transform,
        [Matrix("options", "snapshot", "monitor")] string accessor)
    {
        IServiceCollection services = new ServiceCollection();
        var transformed = false;
        DistributedOptions Transform(DistributedOptions options)
        {
            transformed = true;
            return options with { RunId = "transformed" };
        }

        if (transform == "configure")
        {
            TransformOptionsFactory<DistributedOptions>.Configure(services, Transform);
        }
        else
        {
            TransformOptionsFactory<DistributedOptions>.PostConfigure(services, _ => Transform);
        }

        var factory = registration switch
        {
            "type" => ServiceDescriptor.Transient<IOptionsFactory<DistributedOptions>, OptionsFactory<DistributedOptions>>(),
            "instance" => ServiceDescriptor.Singleton<IOptionsFactory<DistributedOptions>>(new OptionsFactory<DistributedOptions>([], [])),
            "delegate" => ServiceDescriptor.Transient<IOptionsFactory<DistributedOptions>>(_ => new OptionsFactory<DistributedOptions>([], [])),
            _ => throw new ArgumentOutOfRangeException(nameof(registration)),
        };
        if (operation == "add")
        {
            services.Add(factory);
        }
        else
        {
            services.Replace(factory);
        }

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        Func<DistributedOptions> resolve = accessor switch
        {
            "options" => () => scope.ServiceProvider.GetRequiredService<IOptions<DistributedOptions>>().Value,
            "snapshot" => () => scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<DistributedOptions>>().Get("worker"),
            "monitor" => () => scope.ServiceProvider.GetRequiredService<IOptionsMonitor<DistributedOptions>>().Get("worker"),
            _ => throw new ArgumentOutOfRangeException(nameof(accessor)),
        };

        await Assert.That(resolve).Throws<InvalidOperationException>().WithMessageContaining("IOptionsFactory<DistributedOptions>");
        await Assert.That(transformed).IsFalse();
    }

    [Test]
    [MatrixDataSource]
    public async Task Later_Standard_Factory_Is_Rejected_When_Called_Directly(
        [Matrix("configure", "postconfigure")] string transform,
        [Matrix("", "worker")] string name)
    {
        var services = new ServiceCollection();
        if (transform == "configure")
        {
            TransformOptionsFactory<DistributedOptions>.Configure(services, options => options with { RunId = "transformed" });
        }
        else
        {
            TransformOptionsFactory<DistributedOptions>.PostConfigure(services, _ => options => options with { RunId = "transformed" });
        }

        services.AddTransient<IOptionsFactory<DistributedOptions>, OptionsFactory<DistributedOptions>>();
        using var provider = services.BuildServiceProvider();

        await Assert.That(() => provider.GetRequiredService<IOptionsFactory<DistributedOptions>>().Create(name))
            .Throws<InvalidOperationException>().WithMessageContaining("IOptionsFactory<DistributedOptions>");
    }

    [Test]
    public async Task Options_Accessors_Preserve_Cache_Lifetimes()
    {
        var services = new ServiceCollection();
        var transformations = 0;
        TransformOptionsFactory<DistributedOptions>.Configure(services, options =>
        {
            transformations++;
            return options with { RunId = "transformed" };
        });
        using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });
        using var firstScope = provider.CreateScope();
        using var secondScope = provider.CreateScope();
        var options = firstScope.ServiceProvider.GetRequiredService<IOptions<DistributedOptions>>();
        var firstSnapshot = firstScope.ServiceProvider.GetRequiredService<IOptionsSnapshot<DistributedOptions>>();
        var secondSnapshot = secondScope.ServiceProvider.GetRequiredService<IOptionsSnapshot<DistributedOptions>>();
        var monitor = firstScope.ServiceProvider.GetRequiredService<IOptionsMonitor<DistributedOptions>>();

        await Assert.That(options).IsSameReferenceAs(secondScope.ServiceProvider.GetRequiredService<IOptions<DistributedOptions>>());
        await Assert.That(options.Value).IsSameReferenceAs(options.Value);
        await Assert.That(firstSnapshot.Value).IsSameReferenceAs(firstSnapshot.Value);
        await Assert.That(firstSnapshot.Value).IsNotSameReferenceAs(secondSnapshot.Value);
        await Assert.That(monitor).IsSameReferenceAs(secondScope.ServiceProvider.GetRequiredService<IOptionsMonitor<DistributedOptions>>());
        await Assert.That(monitor.CurrentValue).IsSameReferenceAs(monitor.CurrentValue);
        await Assert.That(transformations).IsEqualTo(4);
    }

    [Test]
    [Arguments("type", "configure")]
    [Arguments("instance", "configure")]
    [Arguments("delegate", "configure")]
    [Arguments("type", "postconfigure")]
    [Arguments("instance", "postconfigure")]
    [Arguments("delegate", "postconfigure")]
    [Arguments("type", "register")]
    [Arguments("instance", "register")]
    [Arguments("delegate", "register")]
    public async Task Incompatible_Closed_Factory_Is_Rejected_Before_Registering_Transforms(string registration, string operation)
    {
        IServiceCollection services = new ServiceCollection();
        services.AddOptions();
        services.Add(registration switch
        {
            "type" => ServiceDescriptor.Transient<IOptionsFactory<DistributedOptions>, OptionsFactory<DistributedOptions>>(),
            "instance" => ServiceDescriptor.Singleton<IOptionsFactory<DistributedOptions>>(new OptionsFactory<DistributedOptions>([], [])),
            "delegate" => ServiceDescriptor.Transient<IOptionsFactory<DistributedOptions>>(_ => new OptionsFactory<DistributedOptions>([], [])),
            _ => throw new ArgumentOutOfRangeException(nameof(registration)),
        });
        var originalCount = services.Count;
        Action registerTransform = operation switch
        {
            "configure" => () => TransformOptionsFactory<DistributedOptions>.Configure(services, options => options with { RunId = "transformed" }),
            "postconfigure" => () => TransformOptionsFactory<DistributedOptions>.PostConfigure(services, _ => options => options with { RunId = "transformed" }),
            "register" => () => TransformOptionsFactory<DistributedOptions>.Register(services),
            _ => throw new ArgumentOutOfRangeException(nameof(operation)),
        };

        await Assert.That(registerTransform).Throws<InvalidOperationException>().WithMessageContaining("IOptionsFactory<DistributedOptions>");
        await Assert.That(services.Count).IsEqualTo(originalCount);
        using var provider = services.BuildServiceProvider();
        await Assert.That(provider.GetRequiredService<IOptions<DistributedOptions>>().Value.RunId).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task Keyed_Factory_Does_Not_Conflict_With_Repeat_Transform_Registrations()
    {
        var services = new ServiceCollection();
        services.AddKeyedTransient<IOptionsFactory<DistributedOptions>, OptionsFactory<DistributedOptions>>("custom");
        TransformOptionsFactory<DistributedOptions>.Configure(services, options => options with { RunId = "first" });
        TransformOptionsFactory<DistributedOptions>.Configure(services, options => options with { RunId = options.RunId + "-last" });
        TransformOptionsFactory<DistributedOptions>.PostConfigure(services, _ => options => options with { TotalInstances = 3 });
        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<DistributedOptions>>().Value;

        await Assert.That(options.RunId).IsEqualTo("first-last");
        await Assert.That(options.TotalInstances).IsEqualTo(3);
        await Assert.That(services.Count(descriptor => descriptor.ServiceType == typeof(IOptionsFactory<DistributedOptions>) && !descriptor.IsKeyedService)).IsEqualTo(1);
        await Assert.That(provider.GetRequiredKeyedService<IOptionsFactory<DistributedOptions>>("custom")).IsTypeOf<OptionsFactory<DistributedOptions>>();
    }

    [Test]
    public async Task Binding_And_Transforms_Apply_In_Registration_Order()
    {
        var builder = Pipeline.CreateBuilder();
        builder.ConfigureDistributedOptions(options => options with { RunId = "first", MinimumWorkerCount = 1 });
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Distributed:RunId"] = "bound",
            ["Distributed:TotalInstances"] = "3",
        }).Build();
        builder.AddDistributedMode(configuration.GetSection("Distributed"));
        builder.ConfigureDistributedOptions(options => options with { RunId = options.RunId + "-last" });
        using var provider = builder.Services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<DistributedOptions>>().Value;

        await Assert.That(options.RunId).IsEqualTo("bound-last");
        await Assert.That(options.TotalInstances).IsEqualTo(3);
        await Assert.That(options.MinimumWorkerCount).IsEqualTo(1);
        await Assert.That(options.Enabled).IsTrue();
    }

    [Test]
    public async Task Named_Options_Keep_Binding_Transforms_And_Validation()
    {
        var services = new ServiceCollection();
        TransformOptionsFactory<DistributedOptions>.Configure(services, options => options with { RunId = "default" });
        TransformOptionsFactory<DistributedOptions>.Configure(services, options => options with { RunId = "named" }, "worker");
        services.AddOptions<DistributedOptions>("worker").Validate(options => options.RunId == "named", "wrong run");
        using var provider = services.BuildServiceProvider();
        await Assert.That(provider.GetRequiredService<IOptions<DistributedOptions>>().Value.RunId).IsEqualTo("default");
        await Assert.That(provider.GetRequiredService<IOptionsSnapshot<DistributedOptions>>().Get("worker").RunId).IsEqualTo("named");
        await Assert.That(provider.GetRequiredService<IOptionsMonitor<DistributedOptions>>().Get("worker").RunId).IsEqualTo("named");
        await Assert.That(provider.GetRequiredService<IOptionsFactory<DistributedOptions>>().Create("worker").RunId).IsEqualTo("named");
    }

    [Test]
    public async Task Configuration_Reload_Reapplies_Transforms_Without_Changing_Previous_Options()
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Distributed:RunId"] = "first",
        }).Build();
        var builder = Pipeline.CreateBuilder();
        builder.AddDistributedMode(configuration.GetSection("Distributed"));
        builder.ConfigureDistributedOptions(options => options with { RunId = options.RunId + "-transformed" });
        using var provider = builder.Services.BuildServiceProvider();
        var monitor = provider.GetRequiredService<IOptionsMonitor<DistributedOptions>>();
        var original = monitor.CurrentValue;
        configuration["Distributed:RunId"] = "second";
        configuration.Reload();

        await Assert.That(original.RunId).IsEqualTo("first-transformed");
        await Assert.That(monitor.CurrentValue.RunId).IsEqualTo("second-transformed");
        await Assert.That(monitor.CurrentValue.Enabled).IsTrue();
    }

    [Test]
    public async Task Null_Transform_Result_Is_Rejected()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddDistributedMode(_ => null!);
        using var provider = builder.Services.BuildServiceProvider();
        await Assert.That(() => provider.GetRequiredService<IOptions<DistributedOptions>>().Value)
            .Throws<InvalidOperationException>().WithMessageContaining("DistributedOptions configuration returned null");
    }

    [Test]
    public async Task Validation_Sees_Final_Transform_Result()
    {
        var builder = Pipeline.CreateBuilder();
        builder.ConfigureDistributedOptions(options => options with { TotalInstances = 3 });
        builder.ConfigureDistributedOptions(options => options with { TotalInstances = 0 });
        builder.Services.AddOptions<DistributedOptions>().Validate(options => options.TotalInstances > 0, "positive count required");
        using var provider = builder.Services.BuildServiceProvider();
        await Assert.That(() => provider.GetRequiredService<IOptions<DistributedOptions>>().Value)
            .Throws<OptionsValidationException>().WithMessageContaining("positive count required");
    }

    [Test]
    public async Task Transform_Returns_New_Record_And_Preserves_Defaults()
    {
        DistributedOptions? original = null;
        var builder = Pipeline.CreateBuilder();
        builder.ConfigureDistributedOptions(options =>
        {
            original = options;
            return options with { RunId = "transformed" };
        });
        using var provider = builder.Services.BuildServiceProvider();
        var configured = provider.GetRequiredService<IOptions<DistributedOptions>>().Value;
        await Assert.That(original!.RunId).IsEqualTo(string.Empty);
        await Assert.That(configured.RunId).IsEqualTo("transformed");
        await Assert.That(configured.TotalInstances).IsEqualTo(1);
        await Assert.That(configured.WorkerTimeout).IsEqualTo(TimeSpan.FromSeconds(30));
        await Assert.That(configured.Enabled).IsFalse();
    }
    [Test]
    public async Task Enabling_Distributed_Mode_Does_Not_Enable_Unrelated_Named_Options()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddDistributedMode(options => options with { RunId = "default" });
        using var provider = builder.Services.BuildServiceProvider();
        var monitor = provider.GetRequiredService<IOptionsMonitor<DistributedOptions>>();
        await Assert.That(monitor.CurrentValue.Enabled).IsTrue();
        await Assert.That(monitor.Get("other").Enabled).IsFalse();
        await Assert.That(monitor.Get("other").RunId).IsEqualTo(string.Empty);
    }
}
