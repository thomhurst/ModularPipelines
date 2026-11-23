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
    "GET repos/o/r/git/matching-refs/tags/"*)
      jq --arg endpoint "$endpoint" '($endpoint | sub("^repos/o/r/git/matching-refs/"; "refs/")) as $prefix
        | [.[] | select(.ref | startswith($prefix))]' "$FIXTURES/tags.json" | jq -r "$filter" ;;
    "POST repos/o/r/releases/generate-notes") jq -r "$filter" "$FIXTURES/notes.json" ;;
    *) jq -r "$filter" "$FIXTURES/release.json" ;;
  esac
}
`;

function run(script, { releases = [], tags = [], release = {}, onMain = true, env = {} } = {}) {
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
        ...process.env, ...env, FIXTURES: directory, GH_LOG: join(directory, 'gh.log'), ON_MAIN: String(onMain),
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
  assert.doesNotMatch(stepScript(draftWorkflow, 'Update draft release'), /\$\{\{/, 'Pass expressions through env');
});

for (const { name, releases, tags, version, previous } of [
  { name: 'skips tags left by the retired tag-per-commit flow', releases: published, tags: retiredTags, version: '4.0.29', previous: 'v4.0.0' },
  { name: 'suggests one patch after the latest release', releases: published, tags: ['v4.0.0', 'v3.9.1'], version: '4.0.1', previous: 'v4.0.0' },
  { name: 'ignores prereleases and orders versions numerically', version: '4.10.3', previous: 'v4.10.2',
    releases: [release(9, 'v5.0.0-rc.1', { prerelease: true }), release(8, 'v4.9.9'), release(7, 'v4.10.2'), release(6, 'v4.2.10'),
      release(5, 'v6.0.0-beta.1'), release(4, 'v7.0.0', { draft: true })] },
  { name: 'accepts a release tag without the v prefix', releases: [release(1, '4.3.0')], tags: [], version: '4.3.1', previous: '4.3.0' },
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

test('the draft updates only its own draft release', () => {
  const releases = [release(88, 'v9.9.9', { draft: true, body: 'hand-written draft' }),
    release(77, 'v4.0.5', { draft: true, body: `notes\n\n${marker}` }),
    release(66, 'v4.0.4', { body: `published\n\n${marker}` }), ...published];
  const result = run(stepScript(draftWorkflow, 'Update draft release'), { releases });
  assert.equal(result.status, 0, result.stderr + result.stdout);
  const writes = result.calls.filter(call => call.includes('PATCH') || call.includes('POST'))
    .filter(call => !call.includes('repos/o/r/releases/generate-notes'));
  assert.deepEqual(writes.map(call => call.slice(0, 4)), [['api', '-X', 'PATCH', 'repos/o/r/releases/77']]);
  assert.equal(field(writes[0], 'tag_name'), 'v4.0.5');
});

test('the draft needs a published stable release', () => {
  const result = run(stepScript(draftWorkflow, 'Update draft release'),
    { releases: [release(1, 'v1.0.0-rc.1', { prerelease: true }), release(2, 'v1.0.0', { draft: true })] });
  assert.notEqual(result.status, 0);
  assert.match(result.stdout, /No published stable release/);
  assert.ok(!result.calls.some(call => call.includes('POST')));
});

for (const tag of ['v4.1.0', '4.1.0', 'v4.1.0-rc.1', 'v5.0.0-beta.2.x-y', 'v0.0.0']) {
  test(`release ${tag} publishes its tag's version`, () => {
    const result = run(stepScript(dotnetWorkflow, 'Verify release'), { env: { RELEASE_TAG: tag, RELEASE_TARGET: sha } });
    assert.equal(result.status, 0, result.stderr + result.stdout);
    assert.equal(result.output, `version=${tag.replace(/^v/, '')}\n`);
  });
}

for (const tag of ['v4.1', 'release-1', 'v4.1.0+build.1', 'v04.1.0', 'v4.1.0-', 'v4.1.0-rc..1', 'V4.1.0', 'v4.1.0 ', '']) {
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
    const body = `## Changes\n* Something\n\n**Full Changelog**: https://github.com/o/r/compare/v4.0.0...v4.0.29\n\n${marker}`;
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

test('release metadata reaches scripts only through env', () => {
  for (const name of ['Verify release', 'Name release after its tag']) {
    assert.doesNotMatch(stepScript(dotnetWorkflow, name), /\$\{\{/, name);
  }
});
