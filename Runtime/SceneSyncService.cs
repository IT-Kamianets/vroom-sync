using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Firestore;
using ITKamianets.Engine.Scene;
using ITKamianets.Engine.Scene.Model;

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
    /// NOTE: written against the Firebase Unity SDK's Firestore API as commonly documented, but
    /// not verified against a live installed SDK version in the Editor. Hard-references
    /// Firebase.Firestore/App (installed as loose DLLs under Assets/Firebase, not a UPM package --
    /// so there's no version-defines mechanism to guard this optionally, unlike 3d-unity-spatial's
    /// MRUK reference).
    /// </summary>
    public class SceneSyncService
    {
        public async Task UploadSceneAsync(SceneData scene, string interiorId, string floorId, string spaceId)
        {
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
        }
    }
}
