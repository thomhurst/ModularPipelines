#!/usr/bin/env python3
"""
Script to migrate test modules from Module<T> to ModuleNew<T>
"""
import re
import os
from pathlib import Path

def migrate_file(file_path):
    """Migrate a single file from Module to ModuleNew"""
    with open(file_path, 'r', encoding='utf-8') as f:
        content = f.read()

    original_content = content

    # Skip files that use SubModule (they need manual migration)
    if 'SubModule' in content:
        print(f"  SKIP (uses SubModule): {file_path}")
        return 0

    # Count modules before migration
    module_count = len(re.findall(r': Module(?:<|(?:\s|$))', content))

    # Pattern 1: Change `: Module` to `: ModuleNew` (non-generic)
    content = re.sub(r'(\s+class\s+\w+\s*):(\s*)Module(\s|$)', r'\1:\2ModuleNew\3', content)

    # Pattern 2: Change `: Module<T>` to `: ModuleNew<T>` (generic)
    content = re.sub(r'(\s+class\s+\w+\s*):(\s*)Module<', r'\1:\2ModuleNew<', content)

    # Pattern 3: Change `protected override` to `public override` for ExecuteAsync
    content = re.sub(
        r'protected override async Task<(.+?)>\s+ExecuteAsync\(',
        r'public override async Task<\1> ExecuteAsync(',
        content
    )

    # Pattern 4: Replace `await GetModule<T>()` with `await context.GetModuleAsync<T>()`
    content = re.sub(r'\bawait GetModule<', r'await context.GetModuleAsync<', content)

    # Pattern 5: Replace `GetModuleIfRegistered<T>()` with `await context.GetModuleIfRegisteredAsync<T>()`
    content = re.sub(r'\bGetModuleIfRegistered<', r'await context.GetModuleIfRegisteredAsync<', content)

    # Pattern 6: Replace `return await NothingAsync();` with `await Task.CompletedTask; return null;`
    content = re.sub(
        r'return await NothingAsync\(\);',
        'await Task.CompletedTask;\n            return null;',
        content
    )

    # Pattern 7: Remove ModuleRunType property and add [AlwaysRun] attribute
    # First, find classes with ModuleRunType property
    class_pattern = r'((?:\[[\w\.<>]+(?:\([^\]]*\))?\]\s*)*)(public|private)\s+class\s+(\w+)\s*:\s*ModuleNew(?:<[^>]+>)?\s*\{([^}]+)public override ModuleRunType ModuleRunType => ModuleRunType\.AlwaysRun;'

    def add_always_run_attribute(match):
        attributes = match.group(1)
        visibility = match.group(2)
        class_name = match.group(3)
        class_body = match.group(4)

        # Add [AlwaysRun] attribute if not already present
        if '[AlwaysRun]' not in attributes:
            attributes = attributes.rstrip() + '\n    [AlwaysRun]\n    '

        return f'{attributes}{visibility} class {class_name} : ModuleNew\n    {{{class_body}'

    content = re.sub(class_pattern, add_always_run_attribute, content, flags=re.DOTALL)

    # Pattern 8: Update test assertions from `result.Result.Value` to `result.Value`
    content = re.sub(r'\.Result\.Value', r'.Value', content)

    if content != original_content:
        with open(file_path, 'w', encoding='utf-8') as f:
            f.write(content)
        return module_count

    return 0

def main():
    """Main migration function"""
    test_dirs = [
        Path(r'C:\git\ModularPipelines\test\ModularPipelines.UnitTests'),
        Path(r'C:\git\ModularPipelines\test\ModularPipelines.Azure.UnitTests'),
        Path(r'C:\git\ModularPipelines\test\ModularPipelines.TestHelpers'),
        Path(r'C:\git\ModularPipelines\src\ModularPipelines.Analyzers\ModularPipelines.Analyzers.Test'),
    ]

    total_files = 0
    total_modules = 0
    skipped_files = []

    for test_dir in test_dirs:
        if not test_dir.exists():
            continue

        print(f"\nProcessing directory: {test_dir}")

        for file_path in test_dir.rglob('*.cs'):
            # Skip obj directories
            if 'obj' in file_path.parts or 'bin' in file_path.parts:
                continue

            # Check if file contains Module classes
            with open(file_path, 'r', encoding='utf-8') as f:
                content = f.read()

            if re.search(r': Module(?:<|(?:\s|$))', content):
                print(f"\nMigrating: {file_path.relative_to(test_dir.parent)}")
                migrated_count = migrate_file(file_path)
                if migrated_count > 0:
                    total_files += 1
                    total_modules += migrated_count
                    print(f"  ✓ Migrated {migrated_count} module(s)")
                elif 'SubModule' in content:
                    skipped_files.append(str(file_path))

    print(f"\n{'='*60}")
    print(f"Migration complete!")
    print(f"Total files migrated: {total_files}")
    print(f"Total modules migrated: {total_modules}")

    if skipped_files:
        print(f"\nSkipped files (using SubModule):")
        for f in skipped_files:
            print(f"  - {f}")

if __name__ == '__main__':
    main()
