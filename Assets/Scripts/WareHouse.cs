using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWareHouse
{
    public void AddProducts(Product product);
    public void DeleteProducts(Product product);
}
public interface ISetPosition
{
    public IEnumerator LinearMove(Product product, Vector3 from, Vector3 to);
    public Vector3 CalculateNewPosition(int count);
}

public class WareHouse : MonoBehaviour, IWareHouse, ISetPosition
{
    [SerializeField] private List<Product> products;
    public List<Product> Products { get => products; private set { } }

  
    [SerializeField] ConcreteFactory factory;
    public ConcreteFactory Factory { get => factory; }


    [SerializeField] private int storeCapacity = 50; // store capacity
    public int StoreCapacity { get => storeCapacity; private set { } }

    [SerializeField] private int xMax = 5, zMax = 5; // size of store of products
    [SerializeField] private float spacing = 2.1f;// spacing between products


    private void Awake()
    {
        products = new List<Product>();
        factory = transform.parent.GetComponent<ConcreteFactory>();
        // if theres already product in warehouse add it to list
        Product[] prod = transform.GetComponentsInChildren<Product>();
        for (int i = 0; i < prod.Length; i++)
        {
            AddProducts(prod[i]);
        }
    }
    // adding product to this warehouse
    public void AddProducts(Product product)
    {
        products.Add(product);
        product.transform.parent = transform;
        StartCoroutine(LinearMove(product, product.transform.position,transform.TransformPoint(CalculateNewPosition(products.Count-1))));
        product.transform.localEulerAngles = new Vector3(0, 0, 0);
        if (!factory.running) StartCoroutine(factory.CreateProduct());

    }
    // linear movement of product from old location to new 
    public IEnumerator LinearMove(Product product, Vector3 from, Vector3 to)
    {
        int i = 0;
        while (i <= 10)
        {
            float count = i / 10f;
            product.transform.position = Vector3.Lerp(from, to, count);
            i += 1;
            yield return new WaitForSeconds(0.01f);
        }
        product.OnGround = true;
        if (product.Delete) DeleteProducts(product); // if product can be deleted ->delete it
    }
    // caluculate new position depending on number of item in new store
    public Vector3 CalculateNewPosition(int count)
    {
        int x, y, z;
        y = count / (xMax * zMax);
        count -= y * xMax * zMax;
        z = count / xMax - 2;
        x = count % xMax - 2;

        return new Vector3(x, y + 0.5f, z) * spacing;
    }
    // restore position of objects in warehouse
    public void RestorePositions()
    {
        for (int i = 0; i < products.Count; i++)
        {
            products[i].transform.localPosition = CalculateNewPosition(i);
        }
    }
    // delete products from warehouse list and return to pool
    public void DeleteProducts(Product product)
    {
        product.Release();
        product.Delete = false;
        products.Remove(product);
        RestorePositions();

    }
}
