#!/usr/bin/env python3
"""
Batch fix remaining migration issues
"""
import re
from pathlib import Path

files_to_fix = [
    Path(r'C:\git\ModularPipelines\test\ModularPipelines.UnitTests\FailedPipelineTests.cs'),
    Path(r'C:\git\ModularPipelines\test\ModularPipelines.UnitTests\ModuleHistoryTests.cs'),
    Path(r'C:\git\ModularPipelines\test\ModularPipelines.UnitTests\ResultsRepositoryTests.cs'),
    Path(r'C:\git\ModularPipelines\test\ModularPipelines.UnitTests\SkipDependabotAttributeTests.cs'),
    Path(r'C:\git\ModularPipelines\test\ModularPipelines.UnitTests\IgnoredFailureTests.cs'),
    Path(r'C:\git\ModularPipelines\test\ModularPipelines.UnitTests\RetryTests.cs'),
    Path(r'C:\git\ModularPipelines\test\ModularPipelines.UnitTests\SkippedModuleTests.cs'),
    Path(r'C:\git\ModularPipelines\test\ModularPipelines.UnitTests\DependsOnAllInheritingFromTests.cs'),
]

for file_path in files_to_fix:
    if not file_path.exists():
        print(f"SKIP: {file_path}")
        continue

    with open(file_path, 'r', encoding='utf-8') as f:
        content = f.read()

    original = content

    # Fix protected override to public override
    content = re.sub(
        r'protected override (async )?Task<',
        r'public override \1Task<',
        content
    )

    # Fix protected internal override to public
    content = re.sub(
        r'protected internal override (async )?Task<',
        r'public override \1Task<',
        content
    )

    # Fix ShouldSkip method
    content = re.sub(
        r'protected override Task<SkipDecision> ShouldSkip\(IPipelineContext context\)',
        r'public Task<SkipDecision> ShouldSkipAsync(IPipelineContext context)',
        content
    )

    # Add IModuleSkipLogic interface if ShouldSkipAsync exists
    if 'ShouldSkipAsync' in content and 'IModuleSkipLogic' not in content:
        content = re.sub(
            r'(class \w+ : ModuleNew(?:<[^>]+>)?)\s*\{',
            r'\1, IModuleSkipLogic\n    {',
            content
        )

    # Fix ShouldIgnoreFailures method
    content = re.sub(
        r'protected override Task<bool> ShouldIgnoreFailures\(IPipelineContext context, Exception exception\)',
        r'public Task<bool> ShouldIgnoreFailuresAsync(IPipelineContext context, Exception exception)',
        content
    )

    # Add IModuleErrorHandling interface if ShouldIgnoreFailuresAsync exists
    if 'ShouldIgnoreFailuresAsync' in content and 'IModuleErrorHandling' not in content:
        content = re.sub(
            r'(class \w+ : ModuleNew(?:<[^>]+>)?)\s*\{',
            r'\1, IModuleErrorHandling\n    {',
            content
        )

    # Fix UseResultFromHistoryIfSkipped
    content = re.sub(
        r'protected override Task<bool> UseResultFromHistoryIfSkipped\(IPipelineContext context\)',
        r'public Task<bool> UseResultFromHistoryIfSkippedAsync(IPipelineContext context)',
        content
    )

    # Fix RetryPolicy property to GetRetryPolicy method
    content = re.sub(
        r'protected override (AsyncRetryPolicy<[^>]+>) RetryPolicy \{ get; \} =\s*([^;]+);',
        r'public IAsyncPolicy GetRetryPolicy() => \2;',
        content
    )

    # Add IModuleRetryPolicy interface if GetRetryPolicy exists
    if 'GetRetryPolicy' in content and 'IModuleRetryPolicy' not in content:
        content = re.sub(
            r'(class \w+ : ModuleNew(?:<[^>]+>)?)\s*\{',
            r'\1, IModuleRetryPolicy\n    {',
            content
        )

    # Add using statement for behaviors if needed
    if any(x in content for x in ['IModuleSkipLogic', 'IModuleErrorHandling', 'IModuleTimeout', 'IModuleRetryPolicy']):
        if 'using ModularPipelines.Modules.Behaviors;' not in content:
            # Find last using statement
            last_using = list(re.finditer(r'^using [^;]+;', content, re.MULTILINE))
            if last_using:
                pos = last_using[-1].end()
                content = content[:pos] + '\nusing ModularPipelines.Modules.Behaviors;' + content[pos:]

    if content != original:
        with open(file_path, 'w', encoding='utf-8') as f:
            f.write(content)
        print(f"FIXED: {file_path.name}")
    else:
        print(f"OK: {file_path.name}")

print("\nDone!")
