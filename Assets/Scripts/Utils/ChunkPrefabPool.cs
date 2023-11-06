using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    [System.Serializable]
    public class ChunkPrefabPool
    {
        public GameObject prefab;
        public Queue<Chunk> Pool = new();
    }
}