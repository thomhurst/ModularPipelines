import assert from 'node:assert/strict';
import { spawnSync } from 'node:child_process';
import { mkdtempSync, readFileSync, rmSync, writeFileSync } from 'node:fs';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import test from 'node:test';

const draftWorkflow = readFileSync(new URL('../workflows/release-draft.yml', import.meta.url), 'utf8');
const dotnetWorkflow = readFileSync(new URL('../workflows/dotnet.yml', import.meta.url), 'utf8');
const marker = draftWorkflow.match(/DRAFT_MARKER: '([^']+)'/)?.[1];
const sha = 'a'.repeat(40);

function stepScript(workflow, name) {
  const step = workflow.match(new RegExp(`- name: ${name}\\r?\\n[\\s\\S]*?\\n( +)run: \\|\\r?\\n([\\s\\S]*?)(?=\\r?\\n {6}- |\\r?\\n\\r?\\n {2}[a-z]|$(?![\\s\\S]))`));
  assert.ok(step, `The workflow must define the '${name}' step`);
  const indent = step[1].length + 2;
  return step[2].split(/\r?\n/).map(line => line.slice(indent)).join('\n');
}

// Emulates `gh api`: --jq runs on each fixture page, as --paginate does, and calls are recorded.
const ghStub = String.raw`
gh() {
  printf '%s\037' "$@" >> "$GH_LOG"; printf '\036' >> "$GH_LOG"
  if [ -n "$FAIL_MATCH" ] && [[ "$*" == *"$FAIL_MATCH"* ]]; then echo '{"message":"Server Error"}'; return 1; fi
  shift
  local endpoint="" filter="." method=GET
  while [ $# -gt 0 ]; do
    case "$1" in
      --paginate) ;;
      --jq) filter="$2"; shift ;;
      -X) method="$2"; shift ;;
      -f|-F) shift ;;
      *) endpoint="$1" ;;
    esac
    shift
  done
  case "$method $endpoint" in
    "GET repos/o/r/releases?per_page=100") jq -r "$filter" "$FIXTURES/releases.json" ;;
    "GET repos/o/r/git/ref/heads/main") jq -n -r --arg sha "$MAIN_TIP" '{object: {sha: $sha}}' | jq -r "$filter" ;;
    "GET repos/o/r/git/matching-refs/tags/"*)
      jq --arg endpoint "$endpoint" '($endpoint | sub("^repos/o/r/git/matching-refs/"; "refs/")) as $prefix
        | [.[] | select(.ref | startswith($prefix))]' "$FIXTURES/tags.json" | jq -r "$filter" ;;
    "POST repos/o/r/releases/generate-notes") jq -r "$filter" "$FIXTURES/notes.json" ;;
    *) jq -r "$filter" "$FIXTURES/release.json" ;;
  esac
}
`;

function run(script, { releases = [], tags = [], release = {}, onMain = true, mainTip = sha, env = {} } = {}) {
  const directory = mkdtempSync(join(tmpdir(), 'release-draft-'));
  try {
    writeFileSync(join(directory, 'releases.json'), JSON.stringify(releases));
    writeFileSync(join(directory, 'tags.json'), JSON.stringify(tags.map(tag => ({ ref: `refs/tags/${tag}` }))));
    writeFileSync(join(directory, 'notes.json'), JSON.stringify({ body: '## Changes\n* Something (#1)' }));
    writeFileSync(join(directory, 'release.json'), JSON.stringify({ html_url: 'https://example.test/release', ...release }));
    for (const file of ['gh.log', 'output', 'summary']) writeFileSync(join(directory, file), '');
    const gitStub = `git() { [ "$1 $2" = "merge-base --is-ancestor" ] && [ "$ON_MAIN" = true ]; }\n`;
    const result = spawnSync('bash', ['--noprofile', '--norc', '-c', ghStub + gitStub + script], {
      env: {
        MAIN_TIP: mainTip, FAIL_MATCH: '', ...process.env, ...env, FIXTURES: directory, GH_LOG: join(directory, 'gh.log'), ON_MAIN: String(onMain),
        GITHUB_REPOSITORY: 'o/r', GITHUB_SHA: sha, GITHUB_OUTPUT: join(directory, 'output'),
        GITHUB_STEP_SUMMARY: join(directory, 'summary'), DRAFT_MARKER: marker,
      },
      encoding: 'utf8',
    });
    assert.ifError(result.error);
    const calls = readFileSync(join(directory, 'gh.log'), 'utf8').split('\x1e').filter(Boolean)
      .map(call => call.split('\x1f').slice(0, -1));
    return { ...result, calls, output: readFileSync(join(directory, 'output'), 'utf8') };
  }
  finally {
    rmSync(directory, { recursive: true, force: true });
  }
}

const field = (call, name) => call.find(argument => argument.startsWith(`${name}=`))?.slice(name.length + 1);
const release = (id, tag, { draft = false, prerelease = false, body = '' } = {}) =>
  ({ id, tag_name: tag, draft, prerelease, body });
// The repository's releases when the draft flow was introduced.
const published = [
  release(5, 'v4.0.0'), release(4, 'v4.0.0-rc.1', { prerelease: true }),
  release(3, 'diag-runtime-131944', { prerelease: true }), release(2, 'v3.2.8'), release(1, 'v3.1.90'),
];
const retiredTags = Array.from({ length: 28 }, (_, index) => `v4.0.${index + 1}`);

test('the draft workflow owns a marked draft for main', () => {
  assert.ok(marker?.startsWith('<!--') && marker.endsWith('-->'), 'The marker must be a hidden comment');
  assert.match(draftWorkflow, /push:\r?\n {4}branches: \["main"\]/);
  assert.match(draftWorkflow, /permissions: \{\}/);
  assert.match(draftWorkflow, /contents: write/);
  assert.match(draftWorkflow, /^ {4}if: github.ref == 'refs\/heads\/main'$/m, 'A dispatch from another branch must not move the draft');
  assert.match(draftWorkflow, /Keep the marker/, 'Document that the marker must stay');
  assert.doesNotMatch(stepScript(draftWorkflow, 'Update draft release'), /\$\{\{/, 'Pass expressions through env');
});

for (const { name, releases, tags, version, previous } of [
  { name: 'skips tags left by the retired tag-per-commit flow', releases: published, tags: retiredTags, version: '4.0.29', previous: 'v4.0.0' },
  { name: 'suggests one patch after the latest release', releases: published, tags: ['v4.0.0', 'v3.9.1'], version: '4.0.1', previous: 'v4.0.0' },
  { name: 'ignores prereleases and orders versions numerically', version: '4.10.3', previous: 'v4.10.2',
    releases: [release(9, 'v5.0.0-rc.1', { prerelease: true }), release(8, 'v4.9.9'), release(7, 'v4.10.2'), release(6, 'v4.2.10'),
      release(5, 'v6.0.0-beta.1'), release(4, 'v7.0.0', { draft: true })] },
  { name: 'accepts a release tag without the v prefix', releases: [release(1, '4.3.0')], tags: [], version: '4.3.1', previous: '4.3.0' },
  { name: 'skips existing tags in either form', releases: published, tags: ['v4.0.1', '4.0.2', 'v4.0.3'], version: '4.0.4', previous: 'v4.0.0' },
  { name: 'ignores releases that are not strict versions', version: '4.0.1', previous: 'v4.0.0',
    releases: [release(3, 'v4.01.0'), release(2, 'v04.1.0'), release(1, 'v4.0.0')] },
]) {
  test(`draft version ${name}`, () => {
    const result = run(stepScript(draftWorkflow, 'Update draft release'), { releases, tags });
    assert.equal(result.status, 0, result.stderr + result.stdout);
    const notes = result.calls.find(call => call.includes('repos/o/r/releases/generate-notes'));
    assert.equal(field(notes, 'tag_name'), `v${version}`);
    assert.equal(field(notes, 'previous_tag_name'), previous);
    assert.equal(field(notes, 'target_commitish'), sha);
    const write = result.calls.at(-1);
    assert.deepEqual(write.slice(0, 4), ['api', '-X', 'POST', 'repos/o/r/releases']);
    assert.equal(field(write, 'tag_name'), `v${version}`);
    assert.equal(field(write, 'name'), version);
    assert.equal(field(write, 'target_commitish'), sha);
    assert.equal(field(write, 'draft'), 'true');
    assert.equal(field(write, 'body'), `## Changes\n* Something (#1)\n\n${marker}`);
  });
}

const ownedReleases = [release(88, 'v9.9.9', { draft: true, body: 'hand-written draft' }),
  release(77, 'v4.0.5', { draft: true, body: `notes\n\n${marker}` }),
  release(66, 'v4.0.4', { body: `published\n\n${marker}` }), ...published];
const releaseWrites = calls => calls.filter(call => call.includes('PATCH') || call.includes('POST'))
  .filter(call => !call.includes('repos/o/r/releases/generate-notes'));

test('the draft updates only its own draft release, without resending draft', () => {
  const result = run(stepScript(draftWorkflow, 'Update draft release'), { releases: ownedReleases, release: { draft: true } });
  assert.equal(result.status, 0, result.stderr + result.stdout);
  const writes = releaseWrites(result.calls);
  assert.deepEqual(writes.map(call => call.slice(0, 4)), [['api', '-X', 'PATCH', 'repos/o/r/releases/77']]);
  assert.equal(field(writes[0], 'tag_name'), 'v4.0.5');
  assert.equal(field(writes[0], 'draft'), undefined, 'An update must never set draft');
  assert.equal(field(writes[0], 'prerelease'), undefined);
  const reread = result.calls.findIndex(call => call[1] === 'repos/o/r/releases/77');
  assert.ok(reread >= 0 && reread < result.calls.indexOf(writes[0]), 'Re-read the draft right before updating it');
});

// Owner overrides: the draft's current tag (release.json) against the suggestion v4.0.1.
for (const { name, current, tags = [], kept } of [
  { name: 'keeps a higher minor the owner set', current: 'v4.1.0', kept: true },
  { name: 'keeps a higher major prerelease', current: 'v5.0.0-rc.1', kept: true },
  { name: 'keeps a higher unprefixed tag', current: '4.0.2', kept: true },
  { name: 'returns to the suggestion when the tag is equal', current: 'v4.0.1', kept: false },
  { name: 'returns to the suggestion when the tag is lower', current: 'v4.0.0', kept: false },
  { name: 'returns to the suggestion below its prerelease', current: 'v4.0.1-rc.1', kept: false },
  { name: 'returns to the suggestion when the tag is not a version', current: 'next', kept: false },
  { name: 'returns to the suggestion when the tag has a leading zero', current: 'v4.01.0', kept: false },
  { name: 'returns to the suggestion when the kept tag already exists', current: 'v4.1.0', tags: ['v4.1.0'], kept: false },
  { name: 'returns to the suggestion when the kept tag exists unprefixed', current: 'v4.1.0', tags: ['4.1.0'], kept: false },
]) {
  test(`draft override ${name}`, () => {
    const result = run(stepScript(draftWorkflow, 'Update draft release'),
      { releases: ownedReleases.filter(r => r.id !== 66), tags, release: { draft: true, tag_name: current } });
    assert.equal(result.status, 0, result.stderr + result.stdout);
    const [write] = releaseWrites(result.calls);
    assert.deepEqual(write.slice(0, 4), ['api', '-X', 'PATCH', 'repos/o/r/releases/77']);
    const notes = result.calls.find(call => call.includes('repos/o/r/releases/generate-notes'));
    assert.equal(field(notes, 'tag_name'), kept ? current : 'v4.0.1');
    assert.equal(field(write, 'tag_name'), kept ? undefined : 'v4.0.1');
    assert.equal(field(write, 'name'), kept ? undefined : '4.0.1');
    assert.equal(field(write, 'target_commitish'), sha);
    assert.equal(field(write, 'draft'), undefined);
    assert.match(result.stdout, kept ? new RegExp(`Keeping the draft's tag ${current}`) : /Using the suggested tag v4\.0\.1/);
  });
}

// The step's own shell functions, run in isolation.
function shellFunctions() {
  const script = stepScript(draftWorkflow, 'Update draft release');
  const functions = [...script.matchAll(/^(?:is_release_tag|compare_numbers|version_greater)\(\) \{[\s\S]*?^\}$/gm)].map(match => match[0]);
  assert.equal(functions.length, 3, 'The draft step defines its version functions');
  return functions.join('\n') + '\n';
}

function shell(command) {
  return spawnSync('bash', ['--noprofile', '--norc', '-c', `set -euo pipefail\n${shellFunctions()}${command}`], { encoding: 'utf8' });
}

for (const [lower, higher] of [['1.10.1', '1.11.0'], ['1.9.0', '1.10.0'], ['2.0.0-rc.9', '2.0.0-rc.10'], ['2.0.0-rc.1', '2.0.0'],
  ['1.0.0-alpha', '1.0.0-alpha.1'], ['1.0.0-alpha.1', '1.0.0-beta'], ['1.0.0-1', '1.0.0-alpha'], ['1.0.0-alpha.beta', '1.0.0-beta'],
  ['1.0.0-beta.2', '1.0.0-beta.11'], ['1.0.0-rc.1', '1.0.0'], ['0.9.9', '1.0.0'], ['1.2.3-Beta', '1.2.3-beta'],
  ['1.0.0-1.99999999999999999998', '1.0.0-1.99999999999999999999']]) {
  test(`SemVer precedence ${lower} < ${higher}`, () => {
    const result = shell(`version_greater ${higher} ${lower} && echo greater; version_greater ${lower} ${higher} || echo lower`);
    assert.equal(result.stdout, 'greater\nlower\n', result.stderr);
  });
}

for (const version of ['1.2.3', '1.0.0-rc.1', '2.0.0-alpha.beta.1']) {
  test(`SemVer precedence: ${version} is not greater than itself`, () => {
    const result = shell(`version_greater ${version} ${version} || echo equal`);
    assert.equal(result.stdout, 'equal\n', result.stderr);
  });
}

test('several marked drafts warn and update the newest', () => {
  const releases = [release(77, 'v4.0.5', { draft: true, body: `a\n\n${marker}` }),
    release(120, 'v4.0.6', { draft: true, body: `b\n\n${marker}` }),
    release(99, 'v4.0.7', { draft: true, body: `c\n\n${marker}` }), ...published];
  const result = run(stepScript(draftWorkflow, 'Update draft release'), { releases, release: { draft: true } });
  assert.equal(result.status, 0, result.stderr + result.stdout);
  assert.match(result.stdout, /::warning::Several drafts carry the release-draft marker \(77 99 120\); updating the newest, 120\./);
  assert.deepEqual(releaseWrites(result.calls).map(call => call.slice(0, 4)), [['api', '-X', 'PATCH', 'repos/o/r/releases/120']]);
});

test('one marked draft updates without a warning', () => {
  const result = run(stepScript(draftWorkflow, 'Update draft release'), { releases: ownedReleases, release: { draft: true } });
  assert.equal(result.status, 0, result.stderr + result.stdout);
  assert.doesNotMatch(result.stdout, /::warning::/);
});

test('a re-run of an older main run leaves the draft alone', () => {
  const result = run(stepScript(draftWorkflow, 'Update draft release'), { releases: ownedReleases, release: { draft: true }, mainTip: 'c'.repeat(40) });
  assert.equal(result.status, 0, result.stderr + result.stdout);
  assert.match(result.stdout, /Main has moved on/);
  assert.deepEqual(releaseWrites(result.calls), []);
  assert.ok(!result.calls.some(call => call.includes('repos/o/r/releases/generate-notes')));
});

for (const [path, releases] of [['create', published], ['update', ownedReleases]]) {
  test(`failed notes generation writes nothing on the ${path} path`, () => {
    const result = run(stepScript(draftWorkflow, 'Update draft release'),
      { releases, release: { draft: true }, env: { FAIL_MATCH: 'releases/generate-notes' } });
    assert.notEqual(result.status, 0);
    assert.ok(result.calls.some(call => call.includes('repos/o/r/releases/generate-notes')), 'Notes were requested');
    assert.deepEqual(releaseWrites(result.calls).filter(call => !call.includes('repos/o/r/releases/generate-notes')), []);
  });
}

test('the draft fails instead of suggesting a version above 65534', () => {
  const tags = Array.from({ length: 4 }, (_, index) => `v4.0.${65531 + index}`);
  const result = run(stepScript(draftWorkflow, 'Update draft release'), { releases: [release(1, 'v4.0.65530')], tags });
  assert.notEqual(result.status, 0);
  assert.match(result.stdout, /above the AssemblyVersion limit/);
  assert.deepEqual(releaseWrites(result.calls), []);
});

test('a draft published since it was listed is left alone', () => {
  const result = run(stepScript(draftWorkflow, 'Update draft release'), { releases: ownedReleases, release: { draft: false } });
  assert.equal(result.status, 0, result.stderr + result.stdout);
  assert.deepEqual(releaseWrites(result.calls), []);
  assert.match(result.stdout, /published meanwhile/);
});

test('a failed GitHub lookup fails the draft instead of using defaults', () => {
  const failing = stepScript(draftWorkflow, 'Update draft release')
    .replace('set -euo pipefail', 'set -euo pipefail\ngh() { return 1; }');
  const result = run(failing, { releases: published });
  assert.notEqual(result.status, 0);
  assert.doesNotMatch(result.stdout, /Draft release/);
});

test('the draft needs a published stable release', () => {
  const result = run(stepScript(draftWorkflow, 'Update draft release'),
    { releases: [release(1, 'v1.0.0-rc.1', { prerelease: true }), release(2, 'v1.0.0', { draft: true })] });
  assert.notEqual(result.status, 0);
  assert.match(result.stdout, /No published stable release/);
  assert.ok(!result.calls.some(call => call.includes('POST')));
});

const acceptedTags = ['v4.1.0', '4.1.0', 'v1.2.3', '1.2.3', 'v0.0.0', 'v1.2.3-beta.1', 'v1.2.3-beta01', 'v1.2.3-0', 'v4.1.0-rc.1',
  'v5.0.0-beta.2.x-y', 'v65534.65534.65534'];
const rejectedTags = ['v1.02.3', 'v01.2.3', 'v1.2.3-beta..1', 'v1.2.3-beta.01', 'v1.2.3-01', 'v1.2.3-', 'v1.2', 'v65535.0.0',
  'v1.65535.0', 'v1.2.9999999999', 'release-1', 'v4.1.0+build.1', 'V4.1.0', 'v4.1.0 ', ''];

test('the draft and the release run share the release tag grammar', () => {
  for (const tag of acceptedTags) assert.equal(shell(`is_release_tag '${tag}'`).status, 0, tag);
  for (const tag of rejectedTags) assert.notEqual(shell(`is_release_tag '${tag}'`).status, 0, tag);
});

for (const tag of acceptedTags) {
  test(`release ${tag} publishes its tag's version`, () => {
    const result = run(stepScript(dotnetWorkflow, 'Verify release'), { env: { RELEASE_TAG: tag, RELEASE_TARGET: sha } });
    assert.equal(result.status, 0, result.stderr + result.stdout);
    assert.equal(result.output, `version=${tag.replace(/^v/, '')}\n`);
  });
}

for (const tag of rejectedTags) {
  test(`release tag '${tag}' is rejected`, () => {
    const result = run(stepScript(dotnetWorkflow, 'Verify release'), { env: { RELEASE_TAG: tag, RELEASE_TARGET: sha } });
    assert.notEqual(result.status, 0);
    assert.match(result.stdout, /is not a version/);
    assert.equal(result.output, '');
  });
}

test('a release must be on main', () => {
  const result = run(stepScript(dotnetWorkflow, 'Verify release'), { onMain: false, env: { RELEASE_TAG: 'v4.1.0', RELEASE_TARGET: sha } });
  assert.notEqual(result.status, 0);
  assert.match(result.stdout, /not on main/);
});

for (const [target, allowed] of [['b'.repeat(40), false], ['main', true], [sha, true]]) {
  test(`a release targeting ${target.slice(0, 8)} ${allowed ? 'publishes' : 'is rejected when its tag already existed'}`, () => {
    const result = run(stepScript(dotnetWorkflow, 'Verify release'), { env: { RELEASE_TAG: 'v4.0.1', RELEASE_TARGET: target } });
    assert.equal(result.status === 0, allowed, result.stdout);
    if (!allowed) assert.match(result.stdout, /Choose an unused tag/);
  });
}

for (const [tag, prerelease] of [['v4.1.0', false], ['v4.1.0-rc.1', true]]) {
  test(`release ${tag} is renamed after its edited tag`, () => {
    const body = `## Changes\n* Compare https://github.com/o/r/compare/v3.0.0...v3.1.0 for context\n\n`
      + `**Full Changelog**: https://github.com/o/r/compare/v4.0.0...v4.0.29\n\n${marker}`;
    const result = run(stepScript(dotnetWorkflow, 'Name release after its tag'), {
      release: { body }, env: { RELEASE_ID: '42', RELEASE_TAG: tag, VERSION: tag.slice(1) },
    });
    assert.equal(result.status, 0, result.stderr + result.stdout);
    const patch = result.calls.at(-1);
    assert.deepEqual(patch.slice(0, 4), ['api', '-X', 'PATCH', 'repos/o/r/releases/42']);
    assert.equal(field(patch, 'name'), tag.slice(1));
    assert.equal(field(patch, 'body'), body.replace('v4.0.0...v4.0.29', `v4.0.0...${tag}`));
    assert.equal(field(patch, 'prerelease') === 'true', prerelease);
    assert.equal(field(patch, 'make_latest') === 'false', prerelease);
  });
}

test('the release is renamed even when publishing failed part-way', () => {
  const step = dotnetWorkflow.match(/- name: Name release after its tag\r?\n([\s\S]*?)run: \|/)?.[1];
  assert.match(step, /^ {8}if: always\(\)$/m);
  assert.match(step, /^ {8}continue-on-error: true$/m);
});

test('release metadata reaches scripts only through env', () => {
  for (const name of ['Verify release', 'Name release after its tag']) {
    assert.doesNotMatch(stepScript(dotnetWorkflow, name), /\$\{\{/, name);
  }
});
