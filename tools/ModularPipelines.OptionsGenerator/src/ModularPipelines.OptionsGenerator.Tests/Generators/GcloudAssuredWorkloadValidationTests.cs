using System.ComponentModel.DataAnnotations;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    public async Task Gcloud_Assured_Workload_Identity_Alternatives_Compile_And_Validate()
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "Gcloud", "586.0.0",
            "gcloud-assured-v2-workloads-create.txt"));
        var command = (await new GcloudResourceArgumentTests.TestScraper().Parse(
            ["gcloud", "assured", "v2", "workloads", "create"], help))!;
        var tool = InheritedPropertyCollisionResolver.Resolve(new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "src/ModularPipelines.Google",
            Commands = [command],
        });
        var resolved = tool.Commands.Single();
        var operandName = resolved.PositionalArguments.Single().PropertyName;
        var flagName = resolved.Options.Single(option => option.SwitchName == "--workload-id").PropertyName;
        await Assert.That(operandName).IsNotEqualTo(flagName);
        var generated = await Generate([.. resolved.Options], resolved.PositionalArguments, resolved.RequiredAlternativeGroups);
        var optionsType = Compile(generated).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        string[] identities = [operandName, flagName, "ExternalIdentifier"];

        // Each spelling works alone, omission requests a server-generated ID, and combinations conflict.
        for (var selection = 0; selection < 8; selection++)
        {
            var instance = Activator.CreateInstance(optionsType, ["framework", "location", "organization"])!;
            optionsType.GetProperty("TargetProject")!.SetValue(instance, "projects/project");
            var selectedCount = 0;
            for (var index = 0; index < identities.Length; index++)
            {
                if ((selection & (1 << index)) != 0)
                {
                    optionsType.GetProperty(identities[index])!.SetValue(instance, "workload");
                    selectedCount++;
                }
            }

            var errors = new List<ValidationResult>();
            await Assert.That(Validator.TryValidateObject(instance, new(instance), errors, true))
                .IsEqualTo(selectedCount <= 1).Because($"Identity selection {selection}: {string.Join("; ", errors)}");
        }
    }
}
