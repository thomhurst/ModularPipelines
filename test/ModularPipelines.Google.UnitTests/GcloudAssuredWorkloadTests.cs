using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Google.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Google.UnitTests;

public class GcloudAssuredWorkloadTests
{
    [Test]
    [Arguments(0, true)]
    [Arguments(1, true)]
    [Arguments(2, true)]
    [Arguments(3, false)]
    [Arguments(4, true)]
    [Arguments(5, false)]
    [Arguments(6, false)]
    [Arguments(7, false)]
    public async Task Workload_Identity_Allows_Omission_Or_One_Spelling(int selection, bool valid)
    {
        var options = CreateOptions();
        var identities = IdentityProperties();
        for (var index = 0; index < identities.Length; index++)
        {
            if ((selection & (1 << index)) != 0)
            {
                identities[index].SetValue(options, "workload");
            }
        }

        var errors = new List<ValidationResult>();
        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(valid);
    }

    [Test]
    [Arguments(-1, null)]
    [Arguments(0, "workload")]
    [Arguments(1, "--workload-id=workload")]
    [Arguments(2, "--external-identifier=workload")]
    public async Task Workload_Identity_Renders_Using_Its_Selected_Spelling(int identity, string? expectedIdentity)
    {
        var options = CreateOptions();
        if (identity >= 0)
        {
            IdentityProperties()[identity].SetValue(options, "workload");
        }

        var expected = new List<string>
        {
            "--framework=fedramp-moderate",
            "--location=us-central1",
            "--organization=123",
            "--target-project=projects/456",
        };
        if (expectedIdentity is not null)
        {
            expected.Add(expectedIdentity);
        }

        var arguments = BuildArguments(options);
        await Assert.That(arguments).IsEquivalentTo(expected);
        if (identity == 0)
        {
            await Assert.That(arguments[0]).IsEqualTo("workload");
        }
    }

    private static GcloudAssuredV2WorkloadsCreateOptions CreateOptions() =>
        new("fedramp-moderate", "us-central1", "123") { TargetProject = "projects/456" };

    private static PropertyInfo[] IdentityProperties()
    {
        var properties = typeof(GcloudAssuredV2WorkloadsCreateOptions).GetProperties();
        return
        [
            properties.Single(property => property.GetCustomAttribute<CliArgumentAttribute>() is not null),
            properties.Single(property => property.GetCustomAttribute<CliOptionAttribute>()?.Name == "--workload-id"),
            properties.Single(property => property.GetCustomAttribute<CliOptionAttribute>()?.Name == "--external-identifier"),
        ];
    }
}
