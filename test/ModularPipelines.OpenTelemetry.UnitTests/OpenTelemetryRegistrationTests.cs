using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.TestHelpers;
using ModularPipelines.Tracing;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ModularPipelines.OpenTelemetry.UnitTests;

public class OpenTelemetryRegistrationTests
{
    private sealed class TestModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [Test]
    public async Task AddOpenTelemetry_Initializes_Trace_And_Meter_Providers()
    {
        var metricReader = new RecordingMetricReader();
        var builder = TestPipelineBuilder.Create();

        builder.AddOpenTelemetry();
        builder.Services.AddOpenTelemetry().WithMetrics(metrics => metrics.AddReader(metricReader));
        builder.AddModule<TestModule>();

        await using var pipeline = await builder.BuildAsync();

        using var activitySource = new ActivitySource(PipelineTelemetry.PipelineSourceName);
        using var activity = activitySource.StartActivity("test");
        using var meter = new Meter(PipelineTelemetry.MeterName);
        var counter = meter.CreateCounter<long>("test");

        await Assert.That(activity).IsNotNull();
        await Assert.That(counter.Enabled).IsTrue();
        await Assert.That(pipeline.Services.GetService<TracerProvider>()).IsNotNull();
        await Assert.That(pipeline.Services.GetService<MeterProvider>()).IsNotNull();
    }

    [Test]
    public async Task AddOpenTelemetry_ForceFlushes_Providers_Before_Disposal()
    {
        var activityProcessor = new RecordingActivityProcessor();
        var metricReader = new RecordingMetricReader();
        var builder = TestPipelineBuilder.Create();

        builder.AddOpenTelemetry();
        builder.Services.AddOpenTelemetry().WithTracing(tracing =>
            tracing.AddProcessor(activityProcessor));
        builder.Services.AddOpenTelemetry().WithMetrics(metrics => metrics.AddReader(metricReader));
        builder.AddModule<TestModule>();

        var pipeline = await builder.BuildAsync();
        await pipeline.DisposeAsync();

        await Assert.That(activityProcessor.ForceFlushCalled).IsTrue();
        await Assert.That(activityProcessor.FlushTimeoutMilliseconds).IsGreaterThan(0);
        await Assert.That(metricReader.CollectCalled).IsTrue();
        await Assert.That(metricReader.CollectTimeoutMilliseconds).IsGreaterThan(0);
    }

    [Test]
    public async Task AddOpenTelemetry_Returns_The_Pipeline_Builder()
    {
        var builder = TestPipelineBuilder.Create();

        var result = builder.AddOpenTelemetry();

        await Assert.That(result).IsSameReferenceAs(builder);
        await Assert.That(typeof(OpenTelemetryPipelineBuilderExtensions).Namespace)
            .IsEqualTo("ModularPipelines.OpenTelemetry");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task AddOtlpExporter_Configures_Trace_Metric_And_Log_Export(bool replaceOptions)
    {
        using var handler = new RecordingHttpMessageHandler();
        using var client = new HttpClient(handler);
        var builder = TestPipelineBuilder.Create();
        ModularPipelinesOpenTelemetryBuilder? telemetryBuilder = null;
        ModularPipelinesOpenTelemetryBuilder? result = null;

        builder.AddOpenTelemetry(telemetry =>
        {
            telemetryBuilder = telemetry;
            result = telemetry.AddOtlpExporter(options =>
            {
                var configured = replaceOptions ? new OtlpExporterOptions() : options;
                configured.Endpoint = new Uri("http://collector.example/otlp/");
                configured.Headers = "x-test-token=test-value";
                configured.Protocol = OtlpExportProtocol.HttpProtobuf;
                configured.TimeoutMilliseconds = 1234;
                configured.HttpClientFactory = () => client;
                return configured;
            });
        });

        using var services = builder.Services.BuildServiceProvider();
        var traces = services.GetRequiredService<TracerProvider>();
        var metrics = services.GetRequiredService<MeterProvider>();
        using var source = new ActivitySource(PipelineTelemetry.PipelineSourceName);
        using (source.StartActivity("export-test"))
        {
        }

        using var meter = new Meter(PipelineTelemetry.MeterName);
        meter.CreateCounter<long>("export-test").Add(1);
        var loggerProvider = services.GetRequiredService<LoggerProvider>();
        services.GetRequiredService<ILoggerFactory>().CreateLogger("external-logger").LogInformation("export-test");
        var logsFlushed = loggerProvider.ForceFlush();
        var tracesFlushed = traces.ForceFlush();
        var metricsFlushed = metrics.ForceFlush();
        var options = services.GetRequiredService<IOptionsMonitor<OtlpExporterOptions>>().CurrentValue;

        await Assert.That(result).IsSameReferenceAs(telemetryBuilder);
        await Assert.That(options.TimeoutMilliseconds).IsEqualTo(1234);
        await Assert.That(logsFlushed).IsTrue();
        await Assert.That(tracesFlushed).IsTrue();
        await Assert.That(metricsFlushed).IsTrue();
        await Assert.That(handler.Requests.Any(request =>
            request.Uri.AbsolutePath == "/otlp/v1/traces" && request.Token == "test-value")).IsTrue();
        await Assert.That(handler.Requests.Any(request =>
            request.Uri.AbsolutePath == "/otlp/v1/metrics" && request.Token == "test-value")).IsTrue();
        await Assert.That(handler.Requests.Any(request =>
            request.Uri.AbsolutePath == "/otlp/v1/logs" && request.Token == "test-value")).IsTrue();
    }

    [Test]
    [Arguments(false, false, false)]
    [Arguments(false, true, false)]
    [Arguments(true, false, false)]
    [Arguments(true, true, false)]
    [Arguments(false, false, true)]
    [Arguments(false, true, true)]
    [Arguments(true, false, true)]
    [Arguments(true, true, true)]
    public async Task AddOtlpExporter_Repeated_Calls_Register_Once_And_Compose_Options(
        bool configureFirst, bool configureSecond, bool separateBuilders)
    {
        using var handler = new RecordingHttpMessageHandler();
        using var client = new HttpClient(handler);
        var builder = TestPipelineBuilder.Create();
        builder.Services.Configure<OtlpExporterOptions>(options =>
        {
            options.Endpoint = new Uri("http://collector.example/otlp/");
            options.Protocol = OtlpExportProtocol.HttpProtobuf;
            options.Headers = "x-test-token=test-value";
            options.TimeoutMilliseconds = 1200;
            options.HttpClientFactory = () => client;
        });
        var configurationOrder = new List<int>();
        builder.AddOpenTelemetry(telemetry =>
        {
            RegisterExporter(telemetry, configureFirst, 1);
            if (!separateBuilders)
            {
                RegisterExporter(telemetry, configureSecond, 2);
            }
        });
        if (separateBuilders)
        {
            builder.AddOpenTelemetry(telemetry => RegisterExporter(telemetry, configureSecond, 2));
        }

        using var services = builder.Services.BuildServiceProvider();
        var traces = services.GetRequiredService<TracerProvider>();
        var metrics = services.GetRequiredService<MeterProvider>();
        using var source = new ActivitySource(PipelineTelemetry.PipelineSourceName);
        using (source.StartActivity("repeat-export-test"))
        {
        }

        using var meter = new Meter(PipelineTelemetry.MeterName);
        meter.CreateCounter<long>("repeat-export-test").Add(1);
        await Assert.That(traces.ForceFlush()).IsTrue();
        await Assert.That(metrics.ForceFlush()).IsTrue();
        var options = services.GetRequiredService<IOptionsMonitor<OtlpExporterOptions>>().CurrentValue;

        await Assert.That(handler.Requests.Count(request => request.Uri.AbsolutePath == "/otlp/v1/traces")).IsEqualTo(1);
        await Assert.That(handler.Requests.Count(request => request.Uri.AbsolutePath == "/otlp/v1/metrics")).IsEqualTo(1);
        await Assert.That(options.TimeoutMilliseconds).IsEqualTo(1200 + (configureFirst ? 1 : 0) + (configureSecond ? 2 : 0));
        await Assert.That(configurationOrder.SequenceEqual(
            new[] { configureFirst ? 1 : 0, configureSecond ? 2 : 0 }.Where(value => value != 0))).IsTrue();

        void RegisterExporter(ModularPipelinesOpenTelemetryBuilder telemetry, bool configure, int order)
        {
            if (configure)
            {
                telemetry.AddOtlpExporter(options =>
                {
                    configurationOrder.Add(order);
                    options.TimeoutMilliseconds += order;
                    return options;
                });
            }
            else
            {
                telemetry.AddOtlpExporter();
            }
        }
    }

    [Test]
    public async Task Configure_Providers_Adds_Custom_Instrumentation_And_Processors()
    {
        var processor = new RecordingActivityProcessor();
        var reader = new RecordingMetricReader();
        var sourceName = $"custom-source-{Guid.NewGuid()}";
        var meterName = $"custom-meter-{Guid.NewGuid()}";
        var builder = TestPipelineBuilder.Create();
        ModularPipelinesOpenTelemetryBuilder? telemetryBuilder = null;
        ModularPipelinesOpenTelemetryBuilder? result = null;

        builder.AddOpenTelemetry(telemetry =>
        {
            telemetryBuilder = telemetry;
            result = telemetry
                .ConfigureTracing(tracing => tracing.AddSource(sourceName).AddProcessor(processor))
                .ConfigureMetrics(metrics => metrics.AddMeter(meterName).AddReader(reader));
            telemetry.Builder.ConfigureResource(resource => resource.AddAttributes(
                [new KeyValuePair<string, object>("test.attribute", "configured")]));
        });

        using var services = builder.Services.BuildServiceProvider();
        var traces = services.GetRequiredService<TracerProvider>();
        var metrics = services.GetRequiredService<MeterProvider>();
        using var source = new ActivitySource(sourceName);
        using var activity = source.StartActivity("custom");
        using var defaultSource = new ActivitySource(PipelineTelemetry.PipelineSourceName);
        using var defaultActivity = defaultSource.StartActivity("default");
        using var meter = new Meter(meterName);
        using var defaultMeter = new Meter(PipelineTelemetry.MeterName);
        var counter = meter.CreateCounter<long>("custom");
        var defaultCounter = defaultMeter.CreateCounter<long>("default");
        traces.ForceFlush();
        metrics.ForceFlush();

        await Assert.That(result).IsSameReferenceAs(telemetryBuilder);
        await Assert.That(activity).IsNotNull();
        await Assert.That(defaultActivity).IsNotNull();
        await Assert.That(counter.Enabled).IsTrue();
        await Assert.That(defaultCounter.Enabled).IsTrue();
        await Assert.That(processor.ForceFlushCalled).IsTrue();
        await Assert.That(reader.CollectCalled).IsTrue();
        await Assert.That(traces.GetResource().Attributes.Any(attribute =>
            attribute.Key == "test.attribute" && Equals(attribute.Value, "configured"))).IsTrue();
    }

    [Test]
    public void Configuration_Hooks_Reject_Null_Callbacks()
    {
        var builder = TestPipelineBuilder.Create();
        builder.AddOpenTelemetry(telemetry =>
        {
            Assert.Throws<ArgumentNullException>(() => telemetry.AddOtlpExporter(null!));
            Assert.Throws<ArgumentNullException>(() => telemetry.ConfigureTracing(null!));
            Assert.Throws<ArgumentNullException>(() => telemetry.ConfigureMetrics(null!));
        });
    }

    [Test]
    public void AddOtlpExporter_Rejects_A_Null_Options_Result()
    {
        var builder = TestPipelineBuilder.Create();
        builder.AddOpenTelemetry(telemetry => telemetry.AddOtlpExporter(_ => null!));
        using var services = builder.Services.BuildServiceProvider();

        Assert.Throws<InvalidOperationException>(() =>
            _ = services.GetRequiredService<IOptionsMonitor<OtlpExporterOptions>>().CurrentValue);
    }

    [Test]
    public async Task AddOtlpExporter_Preserves_Options_Configuration_Order()
    {
        var builder = TestPipelineBuilder.Create();
        builder.Services.Configure<OtlpExporterOptions>(options => options.Headers = "before");
        builder.AddOpenTelemetry(telemetry => telemetry.AddOtlpExporter(options => new OtlpExporterOptions
        {
            Headers = options.Headers + ",transform",
        }));
        builder.Services.Configure<OtlpExporterOptions>(options => options.Headers += ",after");
        builder.Services.PostConfigure<OtlpExporterOptions>(options => options.Headers += ",post");
        using var services = builder.Services.BuildServiceProvider();

        var options = services.GetRequiredService<IOptionsMonitor<OtlpExporterOptions>>().CurrentValue;

        await Assert.That(options.Headers).IsEqualTo("before,transform,after,post");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task AddOtlpExporter_Default_HttpClient_Factory_Uses_PostConfigured_Options(bool replaceOptions)
    {
        var builder = TestPipelineBuilder.Create();
        Func<HttpClient>? liveFactory = null;
        builder.AddOpenTelemetry(telemetry => telemetry.AddOtlpExporter(options =>
        {
            liveFactory = options.HttpClientFactory;
            var configured = replaceOptions ? new OtlpExporterOptions() : options;
            configured.TimeoutMilliseconds = 1234;
            configured.MaxResponseSizeBytes = 1024;
            return configured;
        }));
        builder.Services.PostConfigure<OtlpExporterOptions>(options =>
        {
            options.TimeoutMilliseconds = 5678;
            options.MaxResponseSizeBytes = 8192;
        });
        using var services = builder.Services.BuildServiceProvider();
        var options = services.GetRequiredService<IOptionsMonitor<OtlpExporterOptions>>().CurrentValue;

        using var client = options.HttpClientFactory();

        await Assert.That((object) options.HttpClientFactory).IsSameReferenceAs(liveFactory);
        await Assert.That(client.Timeout).IsEqualTo(TimeSpan.FromMilliseconds(5678));
        await Assert.That(client.MaxResponseContentBufferSize).IsEqualTo(8192L);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task AddOtlpExporter_Preserves_Custom_HttpClient_Factory(bool configureFactoryBefore)
    {
        using var client = new HttpClient();
        Func<HttpClient> factory = () => client;
        var builder = TestPipelineBuilder.Create();
        if (configureFactoryBefore)
        {
            builder.Services.Configure<OtlpExporterOptions>(options => options.HttpClientFactory = factory);
        }

        builder.AddOpenTelemetry(telemetry => telemetry.AddOtlpExporter(_ =>
        {
            var configured = new OtlpExporterOptions();
            if (!configureFactoryBefore)
            {
                configured.HttpClientFactory = factory;
            }

            return configured;
        }));
        builder.Services.PostConfigure<OtlpExporterOptions>(options => options.TimeoutMilliseconds = 5678);
        using var services = builder.Services.BuildServiceProvider();
        var options = services.GetRequiredService<IOptionsMonitor<OtlpExporterOptions>>().CurrentValue;

        await Assert.That((object) options.HttpClientFactory).IsSameReferenceAs(factory);
        await Assert.That(options.HttpClientFactory()).IsSameReferenceAs(client);
    }

    private sealed class RecordingHttpMessageHandler : HttpMessageHandler
    {
        public ConcurrentBag<(Uri Uri, string? Token)> Requests { get; } = [];

        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests.Add((request.RequestUri!, request.Headers.GetValues("x-test-token").Single()));
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new ByteArrayContent([]),
            };
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(Send(request, cancellationToken));
    }

    private sealed class RecordingActivityProcessor : BaseProcessor<Activity>
    {
        public bool ForceFlushCalled { get; private set; }

        public int FlushTimeoutMilliseconds { get; private set; }

        protected override bool OnForceFlush(int timeoutMilliseconds)
        {
            ForceFlushCalled = true;
            FlushTimeoutMilliseconds = timeoutMilliseconds;
            return true;
        }
    }

    private sealed class RecordingMetricReader : MetricReader
    {
        public bool CollectCalled { get; private set; }

        public int CollectTimeoutMilliseconds { get; private set; }

        protected override bool OnCollect(int timeoutMilliseconds)
        {
            CollectCalled = true;
            CollectTimeoutMilliseconds = timeoutMilliseconds;
            return true;
        }
    }
}
