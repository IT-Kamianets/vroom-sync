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

interiors/{interiorId}/floors/{floorId}/spaces/{spaceId}/scene

Later, the same package can be used by desktop, mobile, web, Quest, and viewer applications.

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
