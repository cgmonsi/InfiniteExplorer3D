using UnityEngine;

namespace Environment
{
    /// <summary>
    /// Represents a point in the environment where objects can be dynamically spawned.
    /// </summary>
    public class SpawnPoint : MonoBehaviour
    {
        /// <summary>Color of the gizmo to display in the editor for this spawn point.</summary>
        [SerializeField] private Color gizmoColor = new(0, 0, 1, 0.2f);

        /// <summary>Prefab data of the object to spawn.</summary>
        [SerializeField] private GameObject prefabData;

        /// <summary>Chance that the prefab will spawn when the spawn point is enabled.</summary>
        [field: SerializeField] public float SpawnChance { get; set; } = 10f;

        /// <summary>Reference to the spawned decoration, if any.</summary>
        public GameObject spawnedDecoration;
        
        // Cached transform component for efficiency.
        private Transform _transform;


        private void Awake()
        {
            _transform = transform;
        }
        
        
        private void OnEnable()
        {
            SpawnPrefab();
        }
        
        
        private void OnDisable()
        {
            Destroy(spawnedDecoration);
        }
    
        /// <summary>
        /// Instantiates the prefab as a decoration if the random chance check passes.
        /// </summary>
        private void SpawnPrefab()
        {
            if (prefabData == null || !(Random.Range(0f, 100f) <= SpawnChance)) return;
            
            var newDecorations = Instantiate(prefabData, _transform.position, Quaternion.identity, _transform);
            spawnedDecoration = newDecorations;
        }

        /// <summary>
        /// Draws a gizmo in the scene view for easy identification of the spawn point.
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawSphere(transform.position, 0.5f);
        }
    }
}
