using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts
{
    public class Item : MonoBehaviour
    {
        
        //Used to change sprite on UI, when item caught
        [Header("Set In Inspector")]
        public Sprite icon;
        public FallItemSO fiSO;
        
        private Rigidbody rb;


        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            ChooseRandomDrag();
            
        }

        private void ChooseRandomDrag()
        {
            rb.drag = Random.Range(fiSO.maxDragOfFallingObject, fiSO.maxDragOfFallingObject);
        }
    }
}