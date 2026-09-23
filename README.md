# VRoom Sync

`vroom-sync` is the synchronization package of the VRoom platform.

It connects VRoom applications with the shared backend and is responsible for storing, loading, and synchronizing VRoom scenes and related data.

The first implementation will use Firebase and will primarily support the `vroom-scanner` application.

## Logic

VRoom applications should not implement Firebase or backend synchronization separately.

`vroom-sync` provides a shared layer between VRoom applications and persistent storage.

Initial flow:

Meta Quest Scan
→ `3d-unity-spatial`
→ `3d-unity-scene`
→ `3d-scene-schema`
→ `vroom-sync`
→ Firebase

VRoom scans any interior (a hotel is just one example) — `vroom-sync` doesn't assume what kind of place is being scanned. For the scanner MVP, the package will allow a scanned space (room, corridor, or common area) to be stored under a specific interior and floor. `3d-scene-schema` has no concept of interiors, floors, or spaces at all — those identifiers belong entirely to `vroom-sync`; see its README for why the scene document itself carries none of this.

Example structure:

interiors/{interiorId}/floors/{floorId}/spaces/{spaceId}

(Earlier drafts of this README appended a trailing `/scene` segment -- that's actually an invalid Firestore document path, since it leaves the path ending on a collection rather than a document. The space's own document *is* the scene.)

Later, the same package can be used by desktop, mobile, web, Quest, and viewer applications.

### Status (version A)

`Runtime/SceneSyncService.cs` has one method: `UploadSceneAsync(SceneData, interiorId, floorId, spaceId)`. It writes the scene as a single JSON string (via `3d-unity-scene`'s `SceneJson`) into a `json` field on the space's document, alongside `schemaVersion`, `sceneId`, and a server-timestamped `updatedAt` for identifying a scene without parsing the blob. No download/load path, no retries, no offline queue yet -- version A is upload-only, matching `vroom-scanner`'s scan-and-upload-only scope.

Mapping every schema field to native Firestore fields (to query into scene internals directly) is deliberately deferred -- not needed until something actually needs to query inside a scene rather than just fetch/display the whole thing.

Unlike `3d-unity-spatial`'s optional MRUK dependency, this package hard-references `Firebase.Firestore`/`Firebase.App` -- Firebase sync is this package's entire purpose, so the consuming project (`vroom-scanner`) must have the Firebase Unity SDK's Firestore + App installed for this package to compile at all. Written against the Firebase Unity SDK's Firestore API as commonly documented, but not verified against a live installed SDK version in the Editor yet.

## Responsibilities

- Firebase connection
- Scene upload
- Scene download
- Scene updates
- Interior, floor, and space identification
- Synchronization status
- Error handling
- Offline and retry handling
- Scene version support
- Authentication integration
- Multi-client synchronization

## Roadmap

### Phase 1: Scanner MVP

- Firebase project integration
- Firebase authentication support
- Save scene by interior ID, floor ID, and space ID
- Load existing scene
- Update existing scene
- Upload `3d-scene-schema` data
- Basic synchronization status
- Basic error handling

### Phase 2: Reliable Synchronization

- Automatic retries
- Offline queue
- Last synchronization timestamp
- Scene revision tracking
- Conflict detection
- Partial scene updates
- Upload progress
- Download progress

### Phase 3: Multi-Platform

- Unity desktop support
- Meta Quest support
- Mobile support
- Web support
- Shared synchronization API
- Real-time scene updates

### Phase 4: Production

- Scene version history
- Conflict resolution
- Permissions
- Team access
- Audit history
- Performance optimization
- Stable v1 API
