using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace _Scripts
{
    [CreateAssetMenu(fileName = "Fall Item", menuName = "Scriptable Objects/Fall Item", order = 0)]
    public class FallItemSO : ScriptableObject
    {
        public static FallItemSO S;

        public FallItemSO()
        {
            S = this;
        }

        private void OnEnable()
        {
            DontDestroyOnLoad(this);
        }

        [Range(0f, 10f)] public float playerVelocity = 3f;
        [Tooltip("0 = Mean normal gravity")]
        public float minDragOfFallingObject = 0.5f;
        [Tooltip("Closer to lunar gravity")]
        public float maxDragOfFallingObject = 2f;
        [Range(1,12)]
        public int itemsToCatch = 1;
        public int currentLevel = 1;
        
        [Tooltip("Add one item to catch each [] levels")]
        public int extraItemWhenPassLevels = 2;
        
        public GameObject[] itemPrefabs;

        public GameObject GetItemPrefab()
        {
            int ndx = Random.Range(0, itemPrefabs.Length);
            return itemPrefabs[ndx];
        }

        public int ItemsToCatch
        {
            get
            {
                return itemsToCatch;
            }
            set
            {
                if (value > 12) value = 12;
                itemsToCatch = value;
            }
        }
        
        public void ResetProgress()
        {
            itemsToCatch = 1;
            currentLevel = 1;
        }
    }
}