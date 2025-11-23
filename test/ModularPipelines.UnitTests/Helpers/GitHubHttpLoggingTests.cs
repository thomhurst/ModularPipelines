using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Context;
using ModularPipelines.GitHub.Options;
using ModularPipelines.Http;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Helpers;

public class GitHubHttpLoggingTests : TestBase
{
    public class GitHubOptionsModule : Module<GitHubOptions>
    {
        protected override async Task<GitHubOptions?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            var options = context.ServiceProvider.GetRequiredService<IOptions<GitHubOptions>>();
            return options.Value;
        }
    }

    public class PipelineOptionsModule : Module<PipelineOptions>
    {
        protected override async Task<PipelineOptions?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            var options = context.ServiceProvider.GetRequiredService<IOptions<PipelineOptions>>();
            return options.Value;
        }
    }

    [Test]
    public async Task GitHub_HttpLogging_Defaults_To_PipelineOptions()
    {
        var module = await RunModule<PipelineOptionsModule>();

        var pipelineOptions = module.Result.Value!;

        using (Assert.Multiple())
        {
            await Assert.That(pipelineOptions).IsNotNull();
            await Assert.That(pipelineOptions.DefaultHttpLogging).IsEqualTo(HttpLoggingType.StatusCode | HttpLoggingType.Duration);
        }
    }

    [Test]
    public async Task GitHub_HttpLogging_Can_Be_Overridden()
    {
        var (service, _) = await GetService<IOptions<GitHubOptions>>((_, collection) =>
        {
            collection.Configure<GitHubOptions>(opt =>
            {
                opt.HttpLogging = HttpLoggingType.Request | HttpLoggingType.Response;
            });
        });

        var options = service.Value;

        using (Assert.Multiple())
        {
            await Assert.That(options).IsNotNull();
            await Assert.That(options.HttpLogging).IsEqualTo(HttpLoggingType.Request | HttpLoggingType.Response);
        }
    }

    [Test]
    public async Task GitHub_HttpLogging_Can_Be_Set_To_None()
    {
        var (service, _) = await GetService<IOptions<GitHubOptions>>((_, collection) =>
        {
            collection.Configure<GitHubOptions>(opt =>
            {
                opt.HttpLogging = HttpLoggingType.None;
            });
        });

        var options = service.Value;

        using (Assert.Multiple())
        {
            await Assert.That(options).IsNotNull();
            await Assert.That(options.HttpLogging).IsEqualTo(HttpLoggingType.None);
        }
    }

    [Test]
    public async Task PipelineOptions_DefaultHttpLogging_Can_Be_Configured()
    {
        var (service, _) = await GetService<IOptions<PipelineOptions>>((_, collection) =>
        {
            collection.Configure<PipelineOptions>(opt =>
            {
                opt.DefaultHttpLogging = HttpLoggingType.Request | HttpLoggingType.StatusCode;
            });
        });

        var options = service.Value;

        using (Assert.Multiple())
        {
            await Assert.That(options).IsNotNull();
            await Assert.That(options.DefaultHttpLogging).IsEqualTo(HttpLoggingType.Request | HttpLoggingType.StatusCode);
        }
    }
}
