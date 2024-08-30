using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runner.BarierDespawnManager;
using Runner.SpawnBarrier;

namespace Runner.ResetLevel
{



    public class ResetLevelManager : MonoBehaviour
    {
        [SerializeField] private SpawnBarrierManager _spawnBarrierManager;
        public List<GameObject> RestoreList = new List<GameObject>();
        public List<Vector3> originalPositions = new List<Vector3>();
        // Start is called before the first frame update
        void Start()
        {
            foreach (GameObject gameObject in RestoreList)
            {
                if (gameObject != null)
                {
                    originalPositions.Add(gameObject.transform.position);
                }
            }
        }

        public void RestoreObjects()
        {
            _spawnBarrierManager.ResetBarrierxPosition();

            List<BarrierDespawn> BarrierToDestroy = new List<BarrierDespawn>(FindObjectsOfType<BarrierDespawn>());
            foreach (BarrierDespawn Barrier in BarrierToDestroy)
            {
                if (Barrier.NeedToDestroy)
                {
                    Barrier.SelfDestroy();
                }

            }
            for (int i = 0; i < RestoreList.Count; i++)
            {
                if (RestoreList[i] != null)
                {
                    RestoreList[i].transform.position = originalPositions[i];
                    Debug.Log("Restored Object: " + RestoreList[i].name);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
