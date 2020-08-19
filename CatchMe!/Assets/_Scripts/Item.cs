using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts
{
    public class Item : MonoBehaviour
    {
        
        //Used to change sprite on UI, when item caught
        [Header("Set In Inspector")]
        public Sprite icon;
        
        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.drag = Random.Range(FallItemSO.S.maxDragOfFallingObject, FallItemSO.S.maxDragOfFallingObject);
        }
    }
}