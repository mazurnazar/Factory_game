using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ICarryItem
{
    public IEnumerator TakeItems(WareHouse wareHouse);
    public IEnumerator GiveItems(WareHouse wareHouse);
}
public class Player : MonoBehaviour, ICarryItem, ISetPosition
{
    
    [SerializeField] private List<Product> items;

    [SerializeField] private int bagCapacity = 20;
    [SerializeField] private int bagHeight = 10;
    [SerializeField] private float givingItemDelay = 0.1f;
    [SerializeField] private float takingItemDelay = 0.5f;
    [SerializeField] private float linearMovementDelay = 0.01f;
    [SerializeField] CollectItems collectItems;

    private void Start()
    {
        items = new List<Product>();
    }
    // method for taking items from warehouse
    public IEnumerator TakeItems(WareHouse wareHouse)
    {
        List<Product> products = wareHouse.Products;
        collectItems.collecting = true;
        for (int i = products.Count - 1; i>=0;i--)
        {
            if (!products[i].OnGround) { continue; }
            if (items.Count < bagCapacity) // check if number of items fits in bag
            {
                items.Add(products[i]); // add product to list
                // move item to bag, position, rotation, movement 
                products[i].gameObject.transform.parent = transform;
                StartCoroutine( LinearMove(products[i], products[i].transform.position, CalculateNewPosition(items.Count - 1)));
                products[i].gameObject.transform.localEulerAngles = Vector3.zero;
                products.Remove(products[i]);

                // factory create products
                if (!wareHouse.Factory.running) StartCoroutine(wareHouse.Factory.CreateProduct());
                wareHouse.RestorePositions(); // and restore positions in case if product was added to store before player took previous
                yield return new WaitForSeconds(takingItemDelay);
            }
            
        }
        collectItems.collecting = false;
    }
    // linear movement of product from old location to new 
    public IEnumerator LinearMove(Product product, Vector3 from, Vector3 to)
    {
        int i = 0;
        while (i <= 10)
        {
            float count = i / 10f; // value for linear interpolation between old and new position
            product.transform.position = Vector3.Lerp(from ,transform.TransformPoint(to) , count);
            i += 1;
            yield return new WaitForSeconds(linearMovementDelay);
        }
    }
    // caluculate new position depending on number of item in new store
    public Vector3 CalculateNewPosition(int count)
    {
        int x, y;
        x = count / bagHeight;
        count -= x * bagHeight;
        y = count % bagHeight;
        return new Vector3(x, (y + 1) * 4.1f, 0);
    }
    // giving items to warehouse
    public IEnumerator GiveItems(WareHouse wareHouse)
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            for (int j = 0; j < wareHouse.Factory.NeededProducts.Count; j++)
            {
                // check if its not out of range of store capacity and if this store needs this item
                if (items.Count - i < wareHouse.StoreCapacity && items[i].ProductName == wareHouse.Factory.NeededProducts[j].ProductName)
                {
                    wareHouse.AddProducts(items[i]); // add item to store
                    items.Remove(items[i]); // remove from bag
                    yield return new WaitForSeconds(givingItemDelay);
                    break;
                }
            }
        }
        collectItems.giving = false;
    }
}
