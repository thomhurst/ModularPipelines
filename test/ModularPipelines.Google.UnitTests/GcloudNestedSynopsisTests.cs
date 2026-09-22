using System.ComponentModel.DataAnnotations;
using ModularPipelines.Google.Options;

namespace ModularPipelines.Google.UnitTests;

public class GcloudNestedSynopsisTests
{
    [Test]
    [Arguments("none", false)]
    [Arguments("config", true)]
    [Arguments("branch", true)]
    [Arguments("tag", true)]
    [Arguments("build", true)]
    [Arguments("description", true)]
    [Arguments("combined", true)]
    [Arguments("mixed-config", false)]
    [Arguments("both-patterns", false)]
    [Arguments("both-builds", false)]
    public async Task Build_Trigger_Updates_Preserve_Documented_Nested_Choices(string selection, bool valid)
    {
        var options = new GcloudBuildsTriggersUpdateGithubOptions("trigger")
        {
            TriggerConfig = selection is "config" or "mixed-config" ? "trigger.yaml" : null,
            BranchPattern = selection is "branch" or "combined" or "mixed-config" or "both-patterns" ? ".*" : null,
            TagPattern = selection is "tag" or "both-patterns" ? "v.*" : null,
            BuildConfig = selection is "build" or "combined" or "both-builds" ? "cloudbuild.yaml" : null,
            InlineConfig = selection == "both-builds" ? "inline.yaml" : null,
            Description = selection is "description" or "combined" ? "Updated trigger" : null,
        };
        var errors = new List<ValidationResult>();
        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(valid);
    }

    [Test]
    [Arguments("bucket-manifest", true)]
    [Arguments("bucket-prefix", true)]
    [Arguments("list-prefix", true)]
    [Arguments("bucket-only", false)]
    [Arguments("filter-only", false)]
    [Arguments("both-buckets", false)]
    [Arguments("both-filters", false)]
    [Arguments("dry-run", true)]
    [Arguments("insights", true)]
    [Arguments("insights-only", false)]
    [Arguments("mixed", false)]
    public async Task Storage_Source_Branches_Keep_Bucket_Selectors_And_Filters_Together(string source, bool valid)
    {
        var options = new GcloudStorageBatchOperationsJobsCreateOptions("job")
        {
            DeleteObject = true,
            Bucket = source is "bucket-manifest" or "bucket-prefix" or "bucket-only" or "both-buckets" or "both-filters" or "mixed" ? "bucket" : null,
            BucketList = source is "list-prefix" or "both-buckets" ? ["bucket"] : null,
            ManifestLocation = source is "bucket-manifest" or "filter-only" or "both-buckets" or "both-filters" or "mixed" ? "gs://bucket/manifest.csv" : null,
            IncludedObjectPrefixes = source is "bucket-prefix" or "list-prefix" or "both-filters" ? "prefix" : null,
            DryRunJobId = source is "dry-run" or "mixed" ? "dry-run-job" : null,
            InsightsDataSetConfig = source is "insights" or "insights-only" ? "dataset" : null,
            TargetProject = source == "insights" ? "project" : null,
        };
        var errors = new List<ValidationResult>();
        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(valid);
    }

    [Test]
    [Arguments("none", false)]
    [Arguments("clear-all", true)]
    [Arguments("clear-and-update", true)]
    [Arguments("clear-only", true)]
    [Arguments("update-only", true)]
    [Arguments("clear-and-file", true)]
    [Arguments("update-and-file", false)]
    [Arguments("file", true)]
    [Arguments("mixed", false)]
    public async Task Storage_Custom_Context_Alternatives_Preserve_Documented_Choices(string selection, bool valid)
    {
        var options = new GcloudStorageBatchOperationsJobsCreateOptions("job")
        {
            Bucket = "bucket",
            ManifestLocation = "gs://bucket/manifest.csv",
            ClearAllObjectCustomContexts = selection is "clear-all" or "mixed" ? true : null,
            ClearObjectCustomContexts = selection is "clear-and-update" or "clear-only" or "clear-and-file" ? ["old-key"] : null,
            UpdateObjectCustomContexts = selection is "clear-and-update" or "update-only" or "update-and-file" ? ["key=value"] : null,
            UpdateObjectCustomContextsFile = selection is "file" or "mixed" or "clear-and-file" or "update-and-file" ? "contexts.json" : null,
        };
        var errors = new List<ValidationResult>();
        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(valid);
    }

    [Test]
    [Arguments("api-key", "", true)]
    [Arguments("two", "", true)]
    [Arguments("three", "", true)]
    [Arguments("two", "client-id", false)]
    [Arguments("two", "secret", false)]
    [Arguments("two", "token-url", false)]
    [Arguments("three", "client-id", false)]
    [Arguments("three", "secret", false)]
    [Arguments("three", "token-url", false)]
    [Arguments("three", "authorization-url", false)]
    [Arguments("three", "continue-uri", false)]
    [Arguments("three", "pkce", false)]
    public async Task Agent_Identity_Oauth_Requires_Every_Member_Of_The_Selected_Branch(string branch, string missing, bool valid)
    {
        var options = new GcloudAgentIdentityAuthProvidersCreateOptions("provider")
        {
            ApiKey = branch == "api-key" ? "api-key" : null,
            TwoLeggedOauthClientId = branch == "two" && missing != "client-id" ? "client" : null,
            TwoLeggedOauthClientSecret = branch == "two" && missing != "secret" ? "secret" : null,
            TwoLeggedOauthTokenUrl = branch == "two" && missing != "token-url" ? "https://example.com/token" : null,
            ThreeLeggedOauthClientId = branch == "three" && missing != "client-id" ? "client" : null,
            ThreeLeggedOauthClientSecret = branch == "three" && missing != "secret" ? "secret" : null,
            ThreeLeggedOauthTokenUrl = branch == "three" && missing != "token-url" ? "https://example.com/token" : null,
            ThreeLeggedOauthAuthorizationUrl = branch == "three" && missing != "authorization-url" ? "https://example.com/authorize" : null,
            ThreeLeggedOauthDefaultContinueUri = branch == "three" && missing != "continue-uri" ? "https://example.com/continue" : null,
            ThreeLeggedOauthEnablePkce = branch == "three" && missing != "pkce" ? true : null,
        };
        var errors = new List<ValidationResult>();
        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(valid);
    }
}
