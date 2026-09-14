namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public partial class NestedArgumentGroupParsingTests
{
    [Test]
    [Arguments("FLAGS", false)]
    [Arguments("REQUIRED FLAGS", true)]
    [Arguments("OPTIONAL FLAGS", false)]
    public async Task Gcloud_Flag_Sections_Preserve_Spaced_Default_Annotations(string section, bool required)
    {
        var help = $$"""
            NAME
                gcloud example create - create an example
            {{section}}
                 --retry-policy=POLICY; default=<PolicyValueValuesEnum.on-demand: 1>
                    The retry policy.
                 --------
            """;
        var command = (await CreateGcloudScraper().Parse(["gcloud", "example", "create"], help))!;
        var option = command.Options.Single();
        await Assert.That(option.SwitchName).IsEqualTo("--retry-policy");
        await Assert.That(option.IsRequired).IsEqualTo(required);
    }

    [Test]
    public async Task Gcloud_Required_Schedule_Group_Preserves_Optional_Defaults_And_Resources()
    {
        // Required-flag excerpt rendered from the public Workbench command reference.
        // https://docs.cloud.google.com/sdk/gcloud/reference/workbench/schedules/create
        var help = await File.ReadAllTextAsync(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "Gcloud", "workbench-schedules-create.txt"));
        var command = (await CreateGcloudScraper().Parse(["gcloud", "workbench", "schedules", "create"], help))!;

        await Assert.That(command.RequiredOptions.Select(option => option.SwitchName)).IsEquivalentTo([
            "--region", "--cron-schedule", "--display-name", "--execution-display-name",
            "--gcs-output-uri", "--service-account", "--gcs-notebook-uri",
        ]);
        await Assert.That(command.RequiredAlternativeGroups).IsEmpty();
        await Assert.That(command.Options.Single(option => option.SwitchName == "--execution-timeout").CSharpType)
            .IsEqualTo("string?");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--region").Description!)
            .DoesNotContain("Configuration of the schedule. This must be specified.");
    }

    [Test]
    public async Task Gcloud_Privateca_Required_Switches_Accept_Documented_Negation()
    {
        var help = await File.ReadAllTextAsync(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "Gcloud", "privateca-templates-create.txt"));
        var command = (await CreateGcloudScraper().Parse(["gcloud", "privateca", "templates", "create"], help))!;

        await Assert.That(command.RequiredAlternativeGroups.SelectMany(group => group.PropertyNames))
            .IsEquivalentTo(["CopySans", "NoCopySans", "CopySubject", "NoCopySubject"]);
        await Assert.That(command.RequiredAlternativeGroups.All(group => group.IsMutuallyExclusive)).IsTrue();
    }

    [Test]
    [Arguments("The max running time, as a duration. See gcloud topic datetimes for formatting.", "string?")]
    [Arguments("Timeout specified as a duration, for example 1h or 30m.", "string?")]
    [Arguments("The timeout in seconds.", "int?")]
    public async Task Gcloud_Duration_Syntax_Takes_Precedence_Over_Numeric_Hints(string description, string expectedType)
    {
        var help = $"""
            NAME
                gcloud example create - create an example
            FLAGS
                 --execution-timeout=EXECUTION_TIMEOUT
                    {description}
            """;
        var command = (await CreateGcloudScraper().Parse(["gcloud", "example", "create"], help))!;
        await Assert.That(command.Options.Single().CSharpType).IsEqualTo(expectedType);
    }

    [Test]
    public async Task Gcloud_Duration_Group_Documentation_Does_Not_Change_Numeric_Members()
    {
        const string help = """
            NAME
                gcloud example create - create an example
            FLAGS
                 Configure the duration of the operation.
                   --retry-count=COUNT
                      Number of retries.
            """;
        var command = (await CreateGcloudScraper().Parse(["gcloud", "example", "create"], help))!;
        await Assert.That(command.Options.Single().CSharpType).IsEqualTo("int?");
    }
}
