using System.Collections.Generic;
using Environment;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Represents a chunk in the game, which can contain multiple spawn points.
    /// </summary>
    public class Chunk : MonoBehaviour
    {
        /// <summary>
        /// A list of SpawnPoint objects contained within this chunk.
        /// </summary>
        public List<SpawnPoint> ListSpawnPoint = new();
    }
}