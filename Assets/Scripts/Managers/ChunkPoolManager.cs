using System.Collections.Generic;
using System.Linq;
using Environment;
using UnityEngine;
using Utils;

namespace Managers
{
    /// <summary>
    /// Manages a pool of chunk objects to efficiently reuse them instead of instantiating and destroying.
    /// </summary>
    public class ChunkPoolManager : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance of the ChunkPoolManager.
        /// </summary>
        public ChunkPoolManager Instance { get; private set; } 

        /// <summary>
        /// List of chunk prefab pools for different chunk types.
        /// </summary>
        [SerializeField]
        public List<ChunkPrefabPool> chunkPools;

        // Dictionary to hold pools of chunk prefabs keyed by an index.
        private Dictionary<int, ChunkPrefabPool> _chunkPoolDictionary;


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            InitializePools();
        }

        /// <summary>
        /// Initializes the dictionary of chunk pools for quick access by index.
        /// </summary>
        private void InitializePools()
        {
            _chunkPoolDictionary = new Dictionary<int, ChunkPrefabPool>();

            var index = 0;
            foreach (var pool in chunkPools)
            {
                _chunkPoolDictionary.Add(index, pool);
                index++;
            }
        }

        /// <summary>
        /// Retrieves a chunk from the specified pool, or creates a new one if the pool is empty.
        /// </summary>
        /// <param name="prefabIndex">The index of the chunk prefab in the pool.</param>
        /// <param name="position">The position to place the chunk.</param>
        /// <param name="rotation">The rotation of the chunk.</param>
        /// <returns>The retrieved or newly created chunk.</returns>
        public Chunk GetChunkFromPool(int prefabIndex, Vector3 position, Quaternion rotation)
        {
            var selectedPool = _chunkPoolDictionary[prefabIndex];

            Chunk chunk;
            if (selectedPool.Pool.Count > 0)
            {
                chunk = selectedPool.Pool.Dequeue();
                var transform1 = chunk.transform;
                transform1.position = position;
                transform1.rotation = rotation;
            }
            else
            {
                var poolGameObject = Instantiate(selectedPool.prefab, position, rotation, transform);
                chunk = poolGameObject.GetComponent<Chunk>();
                chunk.ListSpawnPoint = chunk.GetComponentsInChildren<SpawnPoint>().ToList();
            }

            return chunk;
        }

        /// <summary>
        /// Returns a chunk to the specified pool and deactivates it.
        /// </summary>
        /// <param name="chunk">The chunk to return to the pool.</param>
        /// <param name="prefabIndex">The index of the chunk prefab in the pool.</param>
        public void ReturnChunkToPool(Chunk chunk, int prefabIndex)
        {
            if (_chunkPoolDictionary.ContainsKey(prefabIndex))
            {
                chunk.gameObject.SetActive(false);
                _chunkPoolDictionary[prefabIndex].Pool.Enqueue(chunk);
            }   
            else
            {
                Debug.LogError($"Key not found in dictionary: {prefabIndex}");
            }
        }
    }
}
