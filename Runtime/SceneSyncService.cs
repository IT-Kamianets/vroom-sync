using System.Collections.Generic;
using System.Threading.Tasks;
using ITKamianets.Engine.Scene;
using ITKamianets.Engine.Scene.Model;

#if FIRESTORE_PRESENT
using Firebase.Firestore;
#endif

namespace VRoom.Sync
{
    /// <summary>
    /// Uploads a SceneData to Firestore at interiors/{interiorId}/floors/{floorId}/spaces/{spaceId}.
    /// interiorId/floorId/spaceId are vroom-sync's own identifiers -- 3d-scene-schema has no
    /// concept of them. Version A is upload-only; no download/load path yet.
    ///
    /// Storage shape: the full scene is written as a single JSON string (via SceneJson) in the
    /// "json" field, alongside a few plain fields (schemaVersion, sceneId, updatedAt) so a scene
    /// can be identified without parsing the blob. This is deliberately simple for version A --
    /// mapping every schema field to native Firestore fields (for querying into scene internals)
    /// is not needed yet and can be added later without changing this method's signature.
    ///
    /// Compiles to a no-op (throws) without the Firebase Firestore SDK installed, so this
    /// package -- and anything that depends on it, like vroom-scanner -- can still build without
    /// Firebase present. Written against the Firebase Unity SDK's Firestore API as commonly
    /// documented, but not verified against a live installed SDK version in the Editor yet.
    /// </summary>
    public class SceneSyncService
    {
        public async Task UploadSceneAsync(SceneData scene, string interiorId, string floorId, string spaceId)
        {
#if !FIRESTORE_PRESENT
            throw new System.InvalidOperationException(
                "Firebase Firestore (com.google.firebase.firestore) is not installed in this project.");
#else
            var db = FirebaseFirestore.DefaultInstance;
            var docRef = db
                .Collection("interiors").Document(interiorId)
                .Collection("floors").Document(floorId)
                .Collection("spaces").Document(spaceId);

            var data = new Dictionary<string, object>
            {
                { "schemaVersion", scene.SchemaVersion },
                { "sceneId", scene.SceneId },
                { "updatedAt", FieldValue.ServerTimestamp },
                { "json", SceneJson.Serialize(scene) }
            };

            await docRef.SetAsync(data, SetOptions.MergeAll);
#endif
        }
    }
}
