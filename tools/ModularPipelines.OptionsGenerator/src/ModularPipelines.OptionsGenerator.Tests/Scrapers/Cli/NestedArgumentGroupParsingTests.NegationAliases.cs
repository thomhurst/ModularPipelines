using System.ComponentModel.DataAnnotations;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public partial class NestedArgumentGroupParsingTests
{
    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task Gcloud_Explicit_Negation_Aliases_Share_One_Choice(bool negativeFirst, bool required)
    {
        const string positive = "        --log-denies\n            Log denies. Use --no-log-denies to disable.";
        const string negative = "        --no-log-denies\n            Disable logging.";
        var help = $"FLAGS\n    {(required ? "Exactly" : "At most")} one of these must be specified:\n"
            + (negativeFirst ? negative + "\n" + positive : positive + "\n" + negative);
        var command = (await CreateGcloudScraper().Parse(["gcloud", "example", "create"], help))!;
        var option = command.Options.Single();
        await Assert.That(option.NegatedSwitchName).IsEqualTo("--no-log-denies");
        await Assert.That(command.RequiredAlternativeGroups.Single().PropertyNames).IsEquivalentTo(["LogDenies"]);
        var tool = new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "output",
            Commands = [command],
        };
        var generated = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
        await VerifyGeneratedValidation(generated, command.ClassName, async type =>
        {
            foreach (var value in new bool?[] { null, false, true })
            {
                var instance = Activator.CreateInstance(type)!;
                type.GetProperty("LogDenies")!.SetValue(instance, value);
                var errors = ((IValidatableObject) instance).Validate(new ValidationContext(instance));
                await Assert.That(!errors.Any()).IsEqualTo(!required || value.HasValue);
            }
        });
    }

    [Test]
    [Arguments("hub")]
    [Arguments("fleet")]
    public async Task Gcloud_Captured_Policycontroller_Has_Unique_Negation_Switches(string group)
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "Gcloud", "585.0.0",
            $"gcloud-container-{group}-policycontroller-enable.txt"));
        var command = (await CreateGcloudScraper().Parse(["gcloud", "container", group, "policycontroller", "enable"], help))!;
        var switches = command.Options.SelectMany(option => new[] { option.SwitchName, option.NegatedSwitchName })
            .OfType<string>().ToArray();
        await Assert.That(switches.Distinct(StringComparer.Ordinal).Count()).IsEqualTo(switches.Length);
        await Assert.That(command.Options.Single(option => option.PropertyName == "LogDenies").NegatedSwitchName)
            .IsEqualTo("--no-log-denies");
        await Assert.That(command.Options.Any(option => option.PropertyName == "NoLogDenies")).IsFalse();
    }
}
