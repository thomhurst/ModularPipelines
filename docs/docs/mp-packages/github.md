---
title: GitHub Package
---

# GitHub Package

The **GitHub package** offered by **Modular Pipelines**, in conjunction with the implemented **OctoKit client library**, facilitates a streamlined approach to interacting with remote GitHub repositories.

By leveraging this package, developers can more efficiently integrate their pipelines with GitHub, allowing for a smoother workflow and interaction with the platform's extensive API. The convenience afforded by this tool makes it an indispensable asset for anyone looking to automate tasks, synchronize data, or manage repositories programmatically on GitHub within their build pipelines.

## GitHub Client

Using **Octokit** in Modular Pipelines is pretty straightforward. Simply reference the `ModularPipelines.GitHub` package, and then, in your module, access the OctoKit client using `GitHub().Client` property as (an example):

```cs
public class GitHubModule : Module
{
    protected override async Task ExecuteAsync(
        IPipelineContext context, 
        CancellationToken cancellationToken)
    {
        var info = context.GitHub().RepositoryInfo;
        var user = await context.GitHub().Client.User.Get(info.Owner);
        
        context.Logger.LogInformation("User location: {Location}", user.Location);

        return Task.CompletedTask;
    }
}
```

## RepositoryInfo

Various GitHub APIs need certain repository data to be passed to make them work. For general developer ergonomics and easy access, as an additional convenience to work with GitHub APIs, Modular Pipelines' `GitHub()` provides the `RepositoryInfo` property that holds various basic data for easy interaction with OctoKit.

This feature is really important because many services in the GitHub API need details such as the **repository owner** and **the name of the repository.** For example:

```cs
// Let's delete GitHub release with Id 12
var info = context.GitHub().RepositoryInfo;
var owner = info.Owner;
var repo = info.RepositoryName;

await context.GitHub().Client.Repository.Release.Delete(owner, repo, 12);
```

## Client Auth

Modular Pipelines' `GitHub()` automatically authenticates OctoKit client when:

*   Environment variable named `GITHUB_TOKEN` is set (which is automatically provided by GitHub when running the pipeline in the GitHub workflow)
*   By using property `AccessToken` in`GitHubOptions` record provided by the Modular Pipelines

### Using GitHubOptions Record

Configuring OctoKit auth using `GitHubOptions` is straightforward as it follows standard .NET practices for working with `IConfiguration` and `IOptions<T>`. When configuring your pipeline, use the usual practices for working with configuration to configure OctoKit auth:

```cs
await PipelineHostBuilder.Create()
    .ConfigureAppConfiguration((_, builder) =>
    {
        builder.AddJsonFile("appsettings.json")
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables();
    })
    .ConfigureServices((context, collection) =>
    {
        // Registers a section from the configuration file with GitHubOptions
        collection.Configure<GitHubOptions>(context.Configuration.GetSection("ModularPipelines:Secrets:GitHub"));
    })    
    .ExecutePipelineAsync();
```

where `appsettings.json` is constructed as follows:

```json
{
  "ModularPipelines": {
    "Secrets": {
      "AccessToken": "your github access token"
    }
  }
}
```

Once configured, Modular Pipelines will handle authentication and authorization automatically by utilizing the provided access token and will deliver a GitHub client that is ready for immediate use.

**Important Note:** This is just an example; **do not store any confidential data in appsettings.json, .env files, and similar.** Use secret storage, key-vault services, etc., for storing sensitive data, and then use the described configuration practices as shown in the example above.