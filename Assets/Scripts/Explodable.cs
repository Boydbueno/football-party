using UnityEngine;
using System.Collections;

public class Explodable : MonoBehaviour {

    /// <summary>
    /// Given a GameObject, spawns Explosion on the gameObjects position.
    /// </summary>
    /// <param name="position">The position to spawn the smoke</param>
    public void Explode(Vector3 position)
    {
        GameObject explosionPrefab = (GameObject)Resources.Load("Explosion_PS");
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = position;
        Destroy(explosion, 1.0f);
    }

}
