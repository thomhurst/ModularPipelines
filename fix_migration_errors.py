#!/usr/bin/env python3
"""
Script to fix migration errors in test modules
"""
import re
import os
from pathlib import Path

def fix_file(file_path):
    """Fix migration errors in a single file"""
    with open(file_path, 'r', encoding='utf-8') as f:
        content = f.read()

    original_content = content

    # Fix 1: Change remaining `protected` to `public` for ExecuteAsync
    content = re.sub(
        r'protected async Task<(.+?)>\s+ExecuteAsync\(',
        r'public override async Task<\1> ExecuteAsync(',
        content
    )

    # Fix 2: Remove Timeout property and add IModuleTimeout interface
    # Pattern: protected internal override TimeSpan Timeout { get; } = TimeSpan.FromSeconds(X);
    timeout_pattern = r'(\s+)(protected internal override|protected override|public override)\s+TimeSpan\s+Timeout\s+\{\s+get;\s+\}\s*=\s*TimeSpan\.FromSeconds\((\d+(?:\.\d+)?)\);'

    def add_timeout_interface(match):
        indent = match.group(1)
        seconds = match.group(3)
        # Return a GetTimeout method implementation
        return f'{indent}public TimeSpan GetTimeout() => TimeSpan.FromSeconds({seconds});'

    # Check if file has Timeout property
    if re.search(timeout_pattern, content):
        content = re.sub(timeout_pattern, add_timeout_interface, content)

        # Add IModuleTimeout to the class declaration if Timeout was found
        content = re.sub(
            r'(class\s+\w+\s*:\s*ModuleNew(?:<[^>]+>)?)\s*\{',
            r'\1, IModuleTimeout\n    {',
            content
        )

        # Add using statement for behaviors
        if 'using ModularPipelines.Modules.Behaviors;' not in content:
            # Find the last using statement and add after it
            last_using = list(re.finditer(r'^using [^;]+;', content, re.MULTILINE))
            if last_using:
                pos = last_using[-1].end()
                content = content[:pos] + '\nusing ModularPipelines.Modules.Behaviors;' + content[pos:]

    # Fix 3: Handle TimeSpan.Zero timeout
    timeout_zero_pattern = r'(\s+)(protected internal override|protected override|public override)\s+TimeSpan\s+Timeout\s+=>\s+TimeSpan\.Zero;'

    def add_timeout_zero_interface(match):
        indent = match.group(1)
        return f'{indent}public TimeSpan GetTimeout() => TimeSpan.Zero;'

    if re.search(timeout_zero_pattern, content):
        content = re.sub(timeout_zero_pattern, add_timeout_zero_interface, content)

        # Add IModuleTimeout to the class declaration
        content = re.sub(
            r'(class\s+\w+\s*:\s*ModuleNew(?:<[^>]+>)?)\s*\{',
            r'\1, IModuleTimeout\n    {',
            content
        )

        # Add using statement for behaviors
        if 'using ModularPipelines.Modules.Behaviors;' not in content:
            last_using = list(re.finditer(r'^using [^;]+;', content, re.MULTILINE))
            if last_using:
                pos = last_using[-1].end()
                content = content[:pos] + '\nusing ModularPipelines.Modules.Behaviors;' + content[pos:]

    # Fix 4: Convert ShouldSkip method to ShouldSkipAsync with IModuleSkipLogic interface
    shouldskip_pattern = r'(\s+)(protected override|public override)\s+Task<SkipDecision>\s+ShouldSkip\(IPipelineContext\s+context\)'

    if re.search(shouldskip_pattern, content):
        content = re.sub(
            shouldskip_pattern,
            r'\1public Task<SkipDecision> ShouldSkipAsync(IPipelineContext context)',
            content
        )

        # Add IModuleSkipLogic to class declaration
        content = re.sub(
            r'(class\s+\w+\s*:\s*ModuleNew(?:<[^>]+>)?)\s*\{',
            r'\1, IModuleSkipLogic\n    {',
            content
        )

        # Add using statement for behaviors
        if 'using ModularPipelines.Modules.Behaviors;' not in content:
            last_using = list(re.finditer(r'^using [^;]+;', content, re.MULTILINE))
            if last_using:
                pos = last_using[-1].end()
                content = content[:pos] + '\nusing ModularPipelines.Modules.Behaviors;' + content[pos:]

    # Fix 5: Convert ShouldIgnoreFailures to ShouldIgnoreFailuresAsync with IModuleErrorHandling
    shouldignore_pattern = r'(\s+)(protected override|public override)\s+Task<bool>\s+ShouldIgnoreFailures\(IPipelineContext\s+context,\s+Exception\s+exception\)'

    if re.search(shouldignore_pattern, content):
        content = re.sub(
            shouldignore_pattern,
            r'\1public Task<bool> ShouldIgnoreFailuresAsync(IPipelineContext context, Exception exception)',
            content
        )

        # Add IModuleErrorHandling to class declaration
        content = re.sub(
            r'(class\s+\w+\s*:\s*ModuleNew(?:<[^>]+>)?)\s*\{',
            r'\1, IModuleErrorHandling\n    {',
            content
        )

        # Add using statement for behaviors
        if 'using ModularPipelines.Modules.Behaviors;' not in content:
            last_using = list(re.finditer(r'^using [^;]+;', content, re.MULTILINE))
            if last_using:
                pos = last_using[-1].end()
                content = content[:pos] + '\nusing ModularPipelines.Modules.Behaviors;' + content[pos:]

    # Fix 6: Convert RetryPolicy to GetRetryPolicy with IModuleRetryPolicy
    retrypolicy_pattern = r'(\s+)(protected override|public override)\s+AsyncRetryPolicy<[^>]+>\s+RetryPolicy\s+\{[^\}]+\}'

    if re.search(retrypolicy_pattern, content):
        # This is complex - need to convert property to method
        def convert_retry_policy(match):
            indent = match.group(1)
            policy_content = match.group(0)

            # Extract the policy expression
            policy_match = re.search(r'=\s*(.+?);', policy_content, re.DOTALL)
            if policy_match:
                policy_expr = policy_match.group(1).strip()
                return f'{indent}public IAsyncPolicy GetRetryPolicy() => {policy_expr};'
            return policy_content

        content = re.sub(retrypolicy_pattern, convert_retry_policy, content, flags=re.DOTALL)

        # Add IModuleRetryPolicy to class declaration
        content = re.sub(
            r'(class\s+\w+\s*:\s*ModuleNew(?:<[^>]+>)?)\s*\{',
            r'\1, IModuleRetryPolicy\n    {',
            content
        )

        # Add using statement for behaviors
        if 'using ModularPipelines.Modules.Behaviors;' not in content:
            last_using = list(re.finditer(r'^using [^;]+;', content, re.MULTILINE))
            if last_using:
                pos = last_using[-1].end()
                content = content[:pos] + '\nusing ModularPipelines.Modules.Behaviors;' + content[pos:]

    # Fix 7: Handle duplicate interface additions (clean up)
    content = re.sub(r'(, IModule\w+)+', lambda m: ', '.join(sorted(set(m.group(0).split(', ')))), content)

    if content != original_content:
        with open(file_path, 'w', encoding='utf-8') as f:
            f.write(content)
        return True

    return False

def main():
    """Main fix function"""
    test_files = [
        Path(r'C:\git\ModularPipelines\test\ModularPipelines.UnitTests\ModuleTimeoutTests.cs'),
        Path(r'C:\git\ModularPipelines\test\ModularPipelines.UnitTests\EngineCancellationTokenTests.cs'),
        Path(r'C:\git\ModularPipelines\test\ModularPipelines.UnitTests\FailedPipelineTests.cs'),
        Path(r'C:\git\ModularPipelines\test\ModularPipelines.UnitTests\IgnoredFailureTests.cs'),
        Path(r'C:\git\ModularPipelines\test\ModularPipelines.UnitTests\ModuleHistoryTests.cs'),
        Path(r'C:\git\ModularPipelines\test\ModularPipelines.UnitTests\RetryTests.cs'),
        Path(r'C:\git\ModularPipelines\test\ModularPipelines.UnitTests\SkippedModuleTests.cs'),
        Path(r'C:\git\ModularPipelines\test\ModularPipelines.UnitTests\SkipDependabotAttributeTests.cs'),
        Path(r'C:\git\ModularPipelines\test\ModularPipelines.UnitTests\ResultsRepositoryTests.cs'),
    ]

    total_fixed = 0

    for file_path in test_files:
        if not file_path.exists():
            print(f"SKIP (not found): {file_path}")
            continue

        print(f"Fixing: {file_path.name}")
        if fix_file(file_path):
            total_fixed += 1
            print(f"  OK Fixed")
        else:
            print(f"  No changes needed")

    print(f"\nTotal files fixed: {total_fixed}")

if __name__ == '__main__':
    main()
