using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Spawner : MonoBehaviour
{
    [Tooltip("Determines if the spawner starts automatically on start or waits until its told to start")]
    [SerializeField] private bool spawnOnStart = true;
    [Tooltip("Determines the min and max number of seconds between spawns")]
    [SerializeField] private Vector2 spawnIntervalRange = new Vector2(1, 1);
    [Tooltip("Determines which gameobjects to spawn")]
    [SerializeField] private GameObject[] prefabsToSpawn;
    [Tooltip("Determines the direction the spawned objects will move in")]
    [SerializeField] private Vector3 spawnVector = new Vector3(1, 0, 0);
    [SerializeField, HideInInspector] private Vector3 spawnVectorNormalized = new Vector3(1, 0, 0);
    [Tooltip("Determines how quickly the randos will move accross the world")]
    [SerializeField] private Vector2 spawnVelocityRange = new Vector2(1, 2);
    [Tooltip("Determines the maximum numbers of randos allowed before they start despawning (used to keep memory reasonable).")]
    [Min(1)]
    [SerializeField] private int maxChildren = 10;
    [SerializeField] Urchin urchin;
    [SerializeField] GameObject parentObject;

    [SerializeField, Tooltip("Automatically becomes a trigger at start")] private BoxCollider boxCollider;

    //private variables
    private GameObject[] children;
    private float[] childVelocities;
    private int currentChild = 0;
    private bool spawning = false;

    private Vector3 offset = new Vector3();
    private float intialY;

    void Start()
    {
        //ensures current child is one
        currentChild = 0;
        //creates a storage spot for children for destruction later
        children = new GameObject[maxChildren];
        //creates a storage spot for child speeds
        childVelocities = new float[maxChildren];
        //ensures the box collider is a trigger
        boxCollider.isTrigger = true;

        urchin = FindObjectOfType<Urchin>();


        offset = transform.position - urchin.transform.position;
        intialY = transform.position.y;
    }

    //this is called after on disable at start
    private void OnEnable()
    {
        if (spawnOnStart)
        {
            StartSpawning();
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        //store normalized version of the spawnVector when possibly changed
        spawnVectorNormalized = spawnVector.normalized;
    }
#endif


    private void FixedUpdate()
    {
        transform.position = new Vector3(urchin.transform.position.x + offset.x, intialY, urchin.transform.position.z + offset.z);

        for (int i = 0; i < maxChildren; i++)
        {
            if (children[i] != null)
            {
                //move the child along the spawn vector
                children[i].transform.position += spawnVectorNormalized * childVelocities[i] * Time.fixedDeltaTime;
            }
        }

    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnRando());
    }

    public void StopSpawningImmediate()
    {
        StopSpawning(immediate: true);
    }

    public void StopSpawningOverTime()
    {
        StopSpawning(immediate: false);
    }

    private void StopSpawning(bool immediate = true)
    {
        spawning = false;
        StopAllCoroutines();

        if (immediate)
        {

            foreach (GameObject child in children)
            {
                GameObject.Destroy(child);
            }
        }
        else
        {
            StartCoroutine(DespawnOverTime());
        }
    }

    /// <summary>
    /// Causes a prefab to spawn at a randomized cadence
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnRando()
    {
        if (!spawning)
        {
            spawning = true;
            //determine if we need to flip the rando
            var shouldFlip = spawnVector.x > 0;
            float spawnInterval;

            while (true)
            {
                spawnInterval = Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
                yield return new WaitForSeconds(spawnInterval);

                //if we have a child in the current zone destroy it, this will start after the first time the children array is filled
                if (children[currentChild] != null)
                {
                    GameObject.Destroy(children[currentChild]);
                }

                //select a point inside the box collider to spawn at
                var spawnPoint = GetRandomPointInsideCollider(boxCollider);
                //select a random rando to spawn
                int randoChosen = Random.Range(0, prefabsToSpawn.Length);

                //create the rando
                var rando = GameObject.Instantiate(prefabsToSpawn[randoChosen], spawnPoint, Quaternion.identity, parentObject.transform);
                //name the rando
                rando.name = this.name + " [" + rando.name + "]";
                //store the rando
                children[currentChild] = rando;

                //Set the speed of the child
                childVelocities[currentChild] = Random.Range(spawnVelocityRange.x, spawnVelocityRange.y);

                //if we can accept additional current children increase our child index, otherwise set it back to 0
                currentChild = currentChild + 1 >= maxChildren ? 0 : currentChild + 1;

                if (shouldFlip) //this is if we are moving an non-animated thing
                {
                    var flippedRotation = new Quaternion(rando.transform.rotation.x, 180, rando.transform.rotation.z, rando.transform.rotation.w);
                    rando.transform.rotation = flippedRotation;
                }

            }
        }
    }
IEnumerator DespawnOverTime()
    {
        //we can just use the minimum spawn interval for despawning
        yield return new WaitForSeconds(spawnIntervalRange.x);

        for (int childrenDestroyed = 0; childrenDestroyed < maxChildren; childrenDestroyed++)
        {
            //if we have a child in the current zone destroy it, this will start after the first time the children array is filled
            if (children[currentChild] != null)
            {
                GameObject.Destroy(children[currentChild]);
            }

            //if we can accept additional current children increase our child index, otherwise set it back to 0
            currentChild = currentChild + 1 >= maxChildren ? 0 : currentChild + 1;

            yield return new WaitForSeconds(spawnIntervalRange.x);
        }
    }

    /// <summary>
    /// Returns a random point inside of the provided box collider
    /// </summary>
    /// <param name="boxCollider">the box colider to have a point provided within</param>
    /// <returns>Vector 3 that is within the provided box collider</returns>
    public static Vector3 GetRandomPointInsideCollider(BoxCollider boxCollider)
    {
        Vector3 extents = boxCollider.size / 2f;
        Vector3 point = new Vector3(
            Random.Range(-extents.x, extents.x),
            Random.Range(-extents.y, extents.y),
            Random.Range(-extents.z, extents.z)
        ) + boxCollider.center;
        return boxCollider.transform.TransformPoint(point);
    }
}