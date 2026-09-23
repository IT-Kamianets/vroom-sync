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

For the scanner MVP, the package will allow a scanned hotel room to be stored under a specific hotel and room.

Example structure:

hotels/{hotelId}/rooms/{roomId}/scene

Later, the same package can be used by desktop, mobile, web, Quest, and viewer applications.

## Responsibilities

- Firebase connection
- Scene upload
- Scene download
- Scene updates
- Hotel and room identification
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
- Save scene by hotel ID and room ID
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
