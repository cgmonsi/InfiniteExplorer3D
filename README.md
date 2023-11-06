## Overview

This Unity project is designed to implement an infinite 3D world generation system with dynamic chunk loading and decorative element placement. Each chunk in the world is unique with its own set of obstacles and decorative elements, and the player can navigate this world with basic movement controls.

### Controllers

- **BallController.cs**
  - Handles player movement within the game world. Allows for basic navigational controls to move the player character around.

- **CameraController.cs**
  - Manages the camera behavior to follow the player character from a top-down perspective. Ensures the camera stays within bounds and provides a consistent view of the action.

### Environment

- **SpawnPoint.cs**
  - Defines the locations within a chunk where obstacles and decorative elements can be instantiated. It controls the randomness and variety of elements that appear in the world.

### Managers

- **ChunkManager.cs**
  - Responsible for the management of chunks in the player's vicinity. It dynamically loads and unloads chunks based on the player's movement to create an infinite world feeling.

- **ChunkPoolManager.cs**
  - Manages a pool of chunk prefabs for efficient instantiation and recycling of chunk objects. It ensures there is no lag or stutter when new chunks are required for the player's progression through the world.

### Utils

- **Chunk.cs**
  - Represents a single chunk of the game world. Holds a list of spawn points.

- **ChunkPrefabPool.cs**
  - Defines a pool for a specific chunk type. It includes functionality to queue and dequeue prefabs to avoid unnecessary instantiation.

## Functionality

### Infinite World Generation

The world is generated in chunks around the player, ensuring that as the player moves, new chunks are loaded into the world seamlessly, and distant chunks are unloaded.

### Unique Chunk Appearance

Each chunk has a distinct look with different obstacles and decorations, which are controlled by the `SpawnPoint` instances within the `Chunk`. The chunk's appearance remains the same upon return to ensure continuity in the game world.


### Chunk Persistence

Chunks retain their appearance and element placement even when the player leaves and returns to them, providing a consistent world for the player to explore.

### Player Movement

Basic player movement is implemented through the `BallController`, which allows the player to navigate the infinite world.

### Additional Features

Some chunks may have boundaries such as walls to limit the player's movement and create a more challenging environment.

## Instructions

To set up this project:

1. Create a new Unity project.
2. Add the provided scripts and prefabs to the respective folders.
3. Set up the main scene with a `ChunkManager` and `ChunkPoolManager`.
4. Assign the chunk prefabs to the `ChunkPoolManager`.
5. Add a player object with the `BallController` script.
6. Place the `CameraController` script on the main camera.
7. Run the scene to see the infinite world in action.