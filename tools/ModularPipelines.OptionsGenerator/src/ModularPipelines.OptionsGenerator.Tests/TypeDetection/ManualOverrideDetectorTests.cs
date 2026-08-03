using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.TypeDetection;

public class ManualOverrideDetectorTests
{
    [Test]
    [Arguments("managed-cassandra", "cluster", "invoke-command")]
    [Arguments("synapse", "spark", "job", "submit")]
    public async Task Azure_Arguments_Overrides_Are_String_Lists(params string[] commandParts)
    {
        var detector = new ManualOverrideDetector(NullLogger<ManualOverrideDetector>.Instance);
        var result = await detector.DetectTypeAsync(new OptionDetectionContext
        {
            ToolName = "az",
            CommandPath = ["az", .. commandParts],
            OptionName = "--arguments",
            AllNames = ["--arguments"],
        });

        await Assert.That(result.Type).IsEqualTo(CliOptionType.StringList);
        await Assert.That(result.GroupValues).IsTrue();
    }

    [Test]
    public async Task Azure_Arguments_Override_Preserves_Space_Separator()
    {
        var detector = new ManualOverrideDetector(NullLogger<ManualOverrideDetector>.Instance);
        var enhancer = new OptionTypeEnhancer(
            new OptionTypeDetectorPipeline(
                [detector],
                NullLogger<OptionTypeDetectorPipeline>.Instance),
            NullLogger<OptionTypeEnhancer>.Instance);
        var command = new CliCommandDefinition
        {
            FullCommand = "az synapse spark job submit",
            CommandParts = ["synapse", "spark", "job", "submit"],
            ClassName = "AzSynapseSparkJobSubmitOptions",
            ParentClassName = "AzOptions",
            ToolNamespacePrefix = "Az",
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--arguments",
                    PropertyName = "Arguments",
                    CSharpType = "bool?",
                    IsFlag = true,
                    ValueSeparator = " ",
                },
            ],
        };
        var tool = new CliToolDefinition
        {
            ToolName = "az",
            NamespacePrefix = "Az",
            TargetNamespace = "ModularPipelines.Azure",
            OutputDirectory = "src/ModularPipelines.Azure",
            Commands = [command],
        };

        var enhanced = await enhancer.EnhanceAsync(tool);
        var option = enhanced.Commands.Single().Options.Single();

        await Assert.That(option.CSharpType).IsEqualTo("string[]?");
        await Assert.That(option.IsFlag).IsFalse();
        await Assert.That(option.AcceptsMultipleValues).IsTrue();
        await Assert.That(option.GroupValues).IsTrue();
        await Assert.That(option.ValueSeparator).IsEqualTo(" ");
    }
}
