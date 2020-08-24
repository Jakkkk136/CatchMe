using System;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Plate : MonoBehaviour
{
    
    //Used to check win condition
    public delegate void OnItemFall(int itemsCaught);
    public static event OnItemFall onItemFall;
    
    //Used to change UI, when item caught
    public delegate void OnItemFallType(Sprite icon);
    public static event OnItemFallType onItemFallType;

    //Sets to true, when level is over and no more need to check collisions with falling items
    private bool plateIsPassive = false;

    private Rigidbody rb;
    private float speed;
    private Camera mainCamera;

    [Header("Set In Inspector")] 
    public Joystick joystick;
    public FallItemSO fiSO;
    
    [Header("Set Dynamically")]
    [Tooltip("Number of items caught on level")]
    public int itemsCaught;

    private List<Collider> collided = new List<Collider>();

    private void Awake()
    {
        joystick.DeadZone = 0.001f;
        rb = GetComponent<Rigidbody>();
        UIManager.onPlayerPassive += ChangeStateToPassive;
    }

    private void Start()
    {
        speed = fiSO.playerVelocity;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        
        float verticalInputJoy = joystick.Vertical;
        float horizontalUnputJoy = joystick.Horizontal;

        Vector3 vel = new Vector3(horizontalUnputJoy, 0f, verticalInputJoy);

        rb.velocity = vel * speed;


        Vector3 pos = mainCamera.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x, 0.1f, 0.9f);
        transform.position = mainCamera.ViewportToWorldPoint(pos);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (plateIsPassive) return;

        //To avoid double or more collisions with one item
        if (collided.IndexOf(other) == -1)
        {
            collided.Add(other);
        }
        else
        {
            return;
        }

        GameObject otherGo = other.gameObject;
        Item otherItem = otherGo.GetComponent<Item>();
        if (otherItem != null)
        {
            itemsCaught++;
            onItemFall?.Invoke(itemsCaught);
            onItemFallType?.Invoke(otherItem.icon);
        }
    }

    public void ChangeStateToPassive()
    {
        plateIsPassive = true;
        joystick.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        UIManager.onPlayerPassive -= ChangeStateToPassive;
        collided.Clear();
    }
}
 

 
