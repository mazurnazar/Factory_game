using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : MonoBehaviour, IProduct
{
    [SerializeField] private string productName; //name of product
    public string ProductName { get => productName; set => productName = value; }

    [SerializeField] private ObjectPool pool; // pool from where take or return this product
    public ObjectPool Pool { get => pool; set => pool = value; }

    [SerializeField] private bool onground = false; // if product on ground to take it from store
    public bool OnGround { get => onground; set => onground = value; }

    [SerializeField] private bool delete = false; // if product can be deleted and returned to store
    public bool Delete { get => delete; set => delete = value; }
    public void Release() // return this product to pool
    {
        pool.ReturnToPool(this);
    }

    public void Initialize() //set the name of product
    {
        gameObject.name = productName;

    }
}
