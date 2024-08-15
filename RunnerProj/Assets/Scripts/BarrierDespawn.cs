using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierDespawn : MonoBehaviour
{

    public bool NeedToDestroy = true;

    private Player _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (NeedToDestroy)
            if (_player.transform.position.x > this.transform.position.x + 20)
            {
                Destroy(this.gameObject);
            }
    }

    public void SelfDestroy()
    {
        Destroy(this.gameObject);
    }

}
