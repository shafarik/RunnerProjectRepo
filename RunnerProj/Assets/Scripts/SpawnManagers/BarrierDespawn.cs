using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runner.PlayerCharacter;

namespace Runner.BarierDespawnManager
{

    public class BarrierDespawn : MonoBehaviour
    {

        public bool NeedToDestroy = true;

        private Player _player;
        // Start is called before the first frame update

        public void Init(Player player)
        {
            _player = player;
        }

        // Update is called once per frame
        void Update()
        {
            if (NeedToDestroy && _player != null)
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
}
