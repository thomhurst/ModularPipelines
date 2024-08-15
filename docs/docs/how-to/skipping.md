---
title: Skipping Modules
sidebar_position: 7
---

# Skipping Modules

## Overriding ShouldSkip

Within a module, we can override the ShouldSkip module with custom logic.

We return `return SkipDecision.Skip(reason)` to skip and `SkipDecision.DoNotSkip` to not skip. The reason is used in the reporting so we can easily see why a module did or didn't run.

### Example

```csharp
public class MyModule : Module<CommandResult>
{
    protected override Task<SkipDecision> ShouldSkip(IPipelineContext context)
    {
        if (context.Git().Information.BranchName == "main")
        {
            return SkipDecision.DoNotSkip.AsTask();
        }
        
        return SkipDecision.Skip("This should only run on the main branch").AsTask();
    }

```

## History
If a module was skipped, we can attempt to find its history from a previous run. See [History](storing-and-retrieving-results)

## Run Conditions

See [Run Conditions](run-conditions)

## Categories

See [Categories](categories)