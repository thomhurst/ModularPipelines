using System.ComponentModel.DataAnnotations;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    [Arguments("550.0.0")]
    [Arguments("586.0.0")]
    public async Task Gcloud_Training_Groups_Have_Independent_Membership(string version)
    {
        var command = await ScrapeTrainingCommand(version);
        string[][] expectedGroups =
        [
            ["KmsKey", "KmsKeyring", "KmsLocation", "KmsProject"],
            ["ParameterServerCount", "ParameterServerMachineType"],
            ["WorkerCount", "WorkerMachineType"],
        ];
        foreach (var names in expectedGroups)
        {
            var group = command.RequiredAlternativeGroups.Single(group => group.PropertyNames.Contains(names[0]));
            await Assert.That(group.PropertyNames).IsEquivalentTo(names);
            await Assert.That(group.IsRequired).IsFalse();
        }

        await Assert.That(command.Options.Single(option => option.PropertyName == "WorkerCount").Description)
            .DoesNotContain("KMS");
    }

    [Test]
    [Arguments("550.0.0")]
    [Arguments("586.0.0")]
    public async Task Gcloud_Training_Groups_Compile_And_Validate_All_Selections_Independently(string version)
    {
        var command = await ScrapeTrainingCommand(version);
        string[] names = ["KmsKey", "KmsKeyring", "KmsLocation", "KmsProject",
            "ParameterServerCount", "ParameterServerMachineType", "WorkerCount", "WorkerMachineType"];
        var groups = command.RequiredAlternativeGroups.Where(group => group.PropertyNames.Any(names.Contains)).ToList();
        var generated = await Generate([.. command.Options.Where(option => names.Contains(option.PropertyName))],
            alternativeGroups: groups);
        var optionsType = Compile(generated).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;

        // Exhaust all combinations: selectors require their key; each machine pair is all-or-none.
        for (var selection = 0; selection < 1 << names.Length; selection++)
        {
            var instance = Activator.CreateInstance(optionsType)!;
            for (var index = 0; index < names.Length; index++)
            {
                if ((selection & (1 << index)) == 0)
                {
                    continue;
                }

                var property = optionsType.GetProperty(names[index])!;
                var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                property.SetValue(instance, Convert.ChangeType("1", type));
            }

            var keyValid = (selection & 14) == 0 || (selection & 1) != 0;
            var serversValid = (selection & 48) is 0 or 48;
            var workersValid = (selection & 192) is 0 or 192;
            var errors = new List<ValidationResult>();
            await Assert.That(Validator.TryValidateObject(instance, new(instance), errors, true))
                .IsEqualTo(keyValid && serversValid && workersValid)
                .Because($"Training selection {selection}: {string.Join("; ", errors)}");
        }
    }

    private static async Task<CliCommandDefinition> ScrapeTrainingCommand(string version)
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "Gcloud", version,
            "gcloud-ai-platform-jobs-submit-training.txt"));
        return (await GcloudResourceArgumentTests.ScrapeFixture("ai-platform jobs submit training", help)).Single();
    }
}
