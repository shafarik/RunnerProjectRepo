using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpawnLines : MonoBehaviour
{
    [SerializeField] private float _lineLenght = 100.0f;
    [SerializeField] private LayerMask _whatIsPlayer;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            this.transform.position = new Vector3(this.transform.position.x + _lineLenght, this.transform.position.y, this.transform.position.z);
        }
    }
}
