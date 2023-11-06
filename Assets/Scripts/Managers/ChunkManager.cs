using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Managers
{
    /// <summary>
    /// Manages chunks in the game world, handling the loading and unloading of chunks based on the player's position.
    /// </summary>
    public class ChunkManager : MonoBehaviour
    {
        /// <summary>
        /// The distance within which chunks are considered active and should be loaded.
        /// </summary>
        [SerializeField] private int viewDistance = 5;

        private readonly Dictionary<Vector2Int, int> _chunksData = new();
        private readonly Dictionary<Vector2Int, List<int>> _chunksSpawnPoint = new();
        private readonly Dictionary<Vector2Int, Chunk> _activeChunks = new();
        private readonly Queue<GameObject> _chunkPool = new();
        private ChunkPoolManager _chunkPoolManager;
        private Vector2Int _playerChunkPosition;
        private System.Random _random;

        /// <summary>
        /// Static property defining the size of each chunk.
        /// </summary>
        private static int ChunkSize => 6;

        
        private void Start()
        {
            _chunkPoolManager = FindObjectOfType<ChunkPoolManager>();
            _random = new System.Random();
            _playerChunkPosition = Vector2Int.one;
        }

        
        private void Update()
        {
            Vector2Int currentChunkPosition = GetCurrentChunkPosition();
            if (currentChunkPosition != _playerChunkPosition)
            {
                CheckChunksToUpdate(currentChunkPosition);
            }
        }

        
        private void OnDestroy()
        {
            foreach (var chunk in _chunkPool)
            {
                Destroy(chunk);
            }
            _chunkPool.Clear();
        }
        
        
        /// <summary>
        /// Calculates the current chunk position based on the player's world position.
        /// </summary>
        /// <returns>The current chunk position as a Vector2Int.</returns>
        private Vector2Int GetCurrentChunkPosition()
        {
            var playerPosition = GetPlayerPosition();
            return new Vector2Int(
                Mathf.FloorToInt(playerPosition.x / ChunkSize),
                Mathf.FloorToInt(playerPosition.z / ChunkSize)
            );
        }


        /// <summary>
        /// Checks and updates the list of active chunks based on the current chunk position.
        /// </summary>
        /// <param name="currentChunkPosition">The current chunk position to evaluate.</param>
        private void CheckChunksToUpdate(Vector2Int currentChunkPosition)
        {
            var chunksToRemove = new List<Vector2Int>();
            foreach (var chunk in _activeChunks)
            {
                if (Vector2Int.Distance(chunk.Key, currentChunkPosition) > viewDistance)
                {
                    chunksToRemove.Add(chunk.Key);
                }
            }

            foreach (var position in chunksToRemove)
            {
                DeactivateOrStoreChunk(position);
            }

            for (var x = -viewDistance; x <= viewDistance; x++)
            {
                for (var z = -viewDistance; z <= viewDistance; z++)
                {
                    var chunkPosition = new Vector2Int(x, z) + currentChunkPosition;
                    if (!_activeChunks.ContainsKey(chunkPosition))
                    {
                        // CreateChunk(chunkPosition);
                        CreateOrActivateChunk(chunkPosition);
                    }
                }
            }

            _playerChunkPosition = currentChunkPosition;
        }

    
        /// <summary>
        /// Either activates an existing chunk or creates a new one at the specified position.
        /// </summary>
        /// <param name="position">The position to create or activate the chunk.</param>
        private void CreateOrActivateChunk(Vector2Int position)
        {
            if (_activeChunks.TryGetValue(position, out var existingChunk))
            {
                existingChunk.gameObject.SetActive(true);
            }
            else
            {
                if (_chunksData.TryGetValue(position, out var chunkIndex))
                {
                    var chunk = _chunkPoolManager.GetChunkFromPool(chunkIndex, CalculateWorldPosition(position), Quaternion.identity);
                    _activeChunks[position] = chunk;
                    
                    var listSpawnPoint = _chunksSpawnPoint[position];

                    for (var i = 0; i < chunk.ListSpawnPoint.Count - 1; i++)
                    {
                        var spawnPoint = chunk.ListSpawnPoint[i];
                        spawnPoint.SpawnChance = listSpawnPoint[i];
                    }
                    
                    chunk.gameObject.SetActive(true);
                }
                else
                {
                    var prefabIndex = _random.Next(_chunkPoolManager.Instance.chunkPools.Count);

                    chunkIndex = prefabIndex;

                    var chunk = _chunkPoolManager.GetChunkFromPool(chunkIndex, CalculateWorldPosition(position), Quaternion.identity);
                    chunk.gameObject.SetActive(true);
                    
                    var listSpawnPoint = new List<int>();
                    foreach (var spawnPoint in chunk.ListSpawnPoint)
                    {
                        listSpawnPoint.Add(spawnPoint.spawnedDecoration != null ? 100 : 0);
                    }

                    _chunksData.Add(position, chunkIndex);
                    _chunksSpawnPoint.Add(position, listSpawnPoint);
                    _activeChunks.Add(position, chunk);
                }
            }
        }
    
        
        /// <summary>
        /// Calculates the world position for a given chunk based on its position within the grid.
        /// </summary>
        /// <param name="chunkPosition">The chunk position within the grid.</param>
        /// <returns>The world position as a Vector3.</returns>
        private static Vector3 CalculateWorldPosition(Vector2Int chunkPosition)
        {
            return new Vector3(chunkPosition.x * ChunkSize, 0, chunkPosition.y * ChunkSize);
        }

        
        /// <summary>
        /// Deactivates or stores a chunk at the given position.
        /// </summary>
        /// <param name="position">The position of the chunk to deactivate or store.</param>
        private void DeactivateOrStoreChunk(Vector2Int position)
        {
            if (!_activeChunks.TryGetValue(position, out var chunk)) return;
            var chunkIndex = _chunksData[position];
            _chunkPoolManager.ReturnChunkToPool(chunk, chunkIndex);
            _activeChunks.Remove(position);
        }
        
        
        /// <summary>
        /// Retrieves the current position of the player in the game world.
        /// </summary>
        /// <returns>The player's position as a Vector3.</returns>
        private static Vector3 GetPlayerPosition()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                return player.transform.position;
            }
            else
            {
                Debug.LogError("Player object not found! Make sure your player GameObject has the 'Player' tag.");
                return Vector3.zero;
            }
        }
    }
}