using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.TypeDetection;

public class OptionTypeEnhancerTests
{
    [Test]
    public async Task EnhanceAsync_Preserves_Repeatability_For_Detected_Enums()
    {
        var detector = new HeuristicTypeDetector(NullLogger<HeuristicTypeDetector>.Instance);
        var pipeline = new OptionTypeDetectorPipeline(
            [detector],
            NullLogger<OptionTypeDetectorPipeline>.Instance);
        var enhancer = new OptionTypeEnhancer(pipeline, NullLogger<OptionTypeEnhancer>.Instance);
        var option = new CliOptionDefinition
        {
            SwitchName = "--sort",
            PropertyName = "Sort",
            CSharpType = "IEnumerable<string>?",
            Description = "May be repeated or comma-separated. Possible values: ascending, descending.",
            AcceptsMultipleValues = true,
        };
        var command = new CliCommandDefinition
        {
            FullCommand = "pulumi stack ls",
            CommandParts = ["stack", "ls"],
            ClassName = "PulumiStackLsOptions",
            ParentClassName = "PulumiOptions",
            ToolNamespacePrefix = "Pulumi",
            Options = [option],
        };
        var tool = new CliToolDefinition
        {
            ToolName = "pulumi",
            NamespacePrefix = "Pulumi",
            TargetNamespace = "ModularPipelines.Pulumi",
            OutputDirectory = "src/ModularPipelines.Pulumi",
            Commands = [command],
        };

        var enhanced = await enhancer.EnhanceAsync(tool);
        var enhancedOption = enhanced.Commands.Single().Options.Single();

        await Assert.That(enhancedOption.AcceptsMultipleValues).IsTrue();
        await Assert.That(enhancedOption.EnumDefinition).IsNotNull();
        await Assert.That(enhancedOption.CSharpType)
            .IsEqualTo($"IEnumerable<{enhancedOption.EnumDefinition!.EnumName}>?");
    }

    [Test]
    public async Task EnhanceAsync_Applies_Key_Filtered_Secret_Metadata()
    {
        var result = new OptionTypeDetectionResult
        {
            Type = CliOptionType.KeyValue,
            Confidence = 100,
            Source = "ManualOverride",
            SecretValueKeys = ["token", "password"],
        };
        var pipeline = new OptionTypeDetectorPipeline(
            [new FixedDetector(result)],
            NullLogger<OptionTypeDetectorPipeline>.Instance);
        var enhancer = new OptionTypeEnhancer(pipeline, NullLogger<OptionTypeEnhancer>.Instance);
        var tool = CreateTool(new CliOptionDefinition
        {
            SwitchName = "--build-arg",
            PropertyName = "BuildArg",
            CSharpType = "string[]?",
            AcceptsMultipleValues = true,
        });

        var enhanced = await enhancer.EnhanceAsync(tool);
        var option = enhanced.Commands.Single().Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(option.CSharpType).IsEqualTo("IReadOnlyList<KeyValue>?");
            await Assert.That(option.IsKeyValue).IsTrue();
            await Assert.That(option.IsSecret).IsTrue();
            await Assert.That(option.SecretValueKeys).IsEquivalentTo(["token", "password"]);
        }
    }

    [Test]
    public async Task EnhanceAsync_Applies_Metadata_Only_Secret_Override()
    {
        var result = new OptionTypeDetectionResult
        {
            Type = CliOptionType.Unknown,
            Confidence = 100,
            Source = "ManualOverride",
            IsSecret = false,
        };
        var pipeline = new OptionTypeDetectorPipeline(
            [new FixedDetector(result)],
            NullLogger<OptionTypeDetectorPipeline>.Instance);
        var enhancer = new OptionTypeEnhancer(pipeline, NullLogger<OptionTypeEnhancer>.Instance);
        var tool = CreateTool(new CliOptionDefinition
        {
            SwitchName = "--password",
            PropertyName = "Password",
            CSharpType = "string?",
            IsSecret = true,
        });

        var enhanced = await enhancer.EnhanceAsync(tool);

        await Assert.That(enhanced.Commands.Single().Options.Single().IsSecret).IsFalse();
    }

    [Test]
    public async Task Docker_Override_Seeds_Build_Argument_Secret_Keys()
    {
        var detector = new ManualOverrideDetector(
            NullLogger<ManualOverrideDetector>.Instance,
            Path.Combine(AppContext.BaseDirectory, "TypeOverrides"));
        var pipeline = new OptionTypeDetectorPipeline(
            [detector],
            NullLogger<OptionTypeDetectorPipeline>.Instance);
        var enhancer = new OptionTypeEnhancer(pipeline, NullLogger<OptionTypeEnhancer>.Instance);
        var tool = CreateTool(new CliOptionDefinition
        {
            SwitchName = "--build-arg",
            PropertyName = "BuildArg",
            CSharpType = "string[]?",
            AcceptsMultipleValues = true,
        });

        var enhanced = await enhancer.EnhanceManualOverridesAsync(tool);
        var option = enhanced.Commands.Single().Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(option.IsKeyValue).IsTrue();
            await Assert.That(option.IsSecret).IsTrue();
            await Assert.That(option.SecretValueKeys).Contains("token");
            await Assert.That(option.SecretValueKeys).Contains("private_key");
        }
    }

    [Test]
    public async Task EnhanceAsync_Removes_Inferred_Secret_From_Path_Option()
    {
        var pipeline = new OptionTypeDetectorPipeline(
            [],
            NullLogger<OptionTypeDetectorPipeline>.Instance);
        var enhancer = new OptionTypeEnhancer(pipeline, NullLogger<OptionTypeEnhancer>.Instance);
        var tool = CreateTool(new CliOptionDefinition
        {
            SwitchName = "--private-key-location",
            PropertyName = "PrivateKeyLocation",
            CSharpType = "string?",
            Description = "Path to the private key file.",
            IsSecret = true,
        });

        var enhanced = await enhancer.EnhanceAsync(tool);

        await Assert.That(enhanced.Commands.Single().Options.Single().IsSecret).IsFalse();
    }

    [Test]
    public async Task EnhanceAsync_Warns_When_Secret_Looking_Option_Is_Boolean()
    {
        var pipeline = new OptionTypeDetectorPipeline(
            [],
            NullLogger<OptionTypeDetectorPipeline>.Instance);
        var logger = new RecordingLogger<OptionTypeEnhancer>();
        var enhancer = new OptionTypeEnhancer(pipeline, logger);
        var tool = CreateTool(new CliOptionDefinition
        {
            SwitchName = "--show-password",
            PropertyName = "ShowPassword",
            CSharpType = "bool?",
            IsFlag = true,
        });

        var enhanced = await enhancer.EnhanceAsync(tool);

        using (Assert.Multiple())
        {
            await Assert.That(enhanced.Commands.Single().Options.Single().IsSecret).IsFalse();
            await Assert.That(logger.Messages).Contains(message =>
                message.Contains("was detected as boolean", StringComparison.Ordinal));
        }
    }

    [Test]
    public async Task EnhanceAsync_Removes_Inferred_Secret_From_Boolean_Value_Option()
    {
        var pipeline = new OptionTypeDetectorPipeline(
            [],
            NullLogger<OptionTypeDetectorPipeline>.Instance);
        var enhancer = new OptionTypeEnhancer(pipeline, NullLogger<OptionTypeEnhancer>.Instance);
        var tool = CreateTool(new CliOptionDefinition
        {
            SwitchName = "--xml-raw-token",
            PropertyName = "XmlRawToken",
            CSharpType = "bool?",
            Description = "Enables using RawToken instead of Token.",
            IsSecret = true,
        });

        var enhanced = await enhancer.EnhanceAsync(tool);
        var option = enhanced.Commands.Single().Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(option.IsFlag).IsFalse();
            await Assert.That(option.IsSecret).IsFalse();
            await Assert.That(option.SecretValueKeys).IsEmpty();
        }
    }

    private static CliToolDefinition CreateTool(CliOptionDefinition option)
    {
        return new CliToolDefinition
        {
            ToolName = "docker",
            NamespacePrefix = "Docker",
            TargetNamespace = "ModularPipelines.Docker",
            OutputDirectory = "src/ModularPipelines.Docker",
            Commands =
            [
                new CliCommandDefinition
                {
                    FullCommand = "docker build",
                    CommandParts = ["build"],
                    ClassName = "DockerBuildOptions",
                    ParentClassName = "DockerOptions",
                    ToolNamespacePrefix = "Docker",
                    Options = [option],
                }
            ],
        };
    }

    private sealed class FixedDetector(OptionTypeDetectionResult result) : IOptionTypeDetector
    {
        public int Priority => 0;

        public string Name => result.Source;

        public bool CanHandle(string toolName) => true;

        public Task<OptionTypeDetectionResult> DetectTypeAsync(
            OptionDetectionContext context,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(result);
    }

    private sealed class RecordingLogger<T> : ILogger<T>
    {
        public List<string> Messages { get; } = [];

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (logLevel == LogLevel.Warning)
            {
                Messages.Add(formatter(state, exception));
            }
        }
    }
}
