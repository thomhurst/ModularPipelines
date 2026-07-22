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
}
