using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConcreteFactory : Factory
{
    [SerializeField] private List<Product> neededProducts; // products that needed to create new products
    public List<Product> NeededProducts { get => neededProducts; private set { } }

    [SerializeField] private float developementDelay; // speed of products creating
    [SerializeField] private GameObject storedWareHouse, usedWareHouse; // objects for storing products and for their using

    private string message = ""; // message for displaying 
    public string Message { get => message; }
    private WareHouse stored, used;
    public bool running = false;

    public delegate void MessageChanged();
    public event MessageChanged ShowMessage;

    [SerializeField] ObjectPool objectPool; // pool of objects

    // taking a product from pool
    public override IProduct GetProduct(Vector3 position)
    {
        Product newProduct = objectPool.GetPooledObject();
        newProduct.transform.parent = transform;
        newProduct.transform.position = transform.position;   

        newProduct.Initialize();
        return newProduct;
    }
    private void Start()
    {
        stored = storedWareHouse.GetComponent<WareHouse>();
        used = usedWareHouse.GetComponent<WareHouse>();
        StartCoroutine(CreateProduct());
    }
    // create new product
    public override IEnumerator CreateProduct()
    {
        // check if needs other products for creating and if theres available ones
        if (neededProducts.Count == 0 || CheckAvailableProducts())
        {
            if (stored.Products.Count + 1 <= stored.StoreCapacity) // check if store isnt full
            {
                running = true;
                stored.AddProducts((Product)GetProduct(transform.position));
                yield return new WaitForSeconds(developementDelay);
                StartCoroutine(CreateProduct());
            }
            else
            {
                running = false; 
                message = "store out of capacity";
                ShowMessage?.Invoke();
            }
        }
        else running = false;
    }
    // checking for available products
    bool CheckAvailableProducts()
    {
        message = "";
        bool check = false;
        List<Product> products = new List<Product>();
        for (int i = 0; i < neededProducts.Count; i++)
        {
            if (used.Products.Count == 0)
            {
                message += neededProducts[i].ProductName + " ";
            }
            else
            {
                for (int j = used.Products.Count - 1; j >= 0; j--)
                {
                    if (used.Products[j].ProductName == neededProducts[i].ProductName)
                    {
                        check = true;
                        products.Add(used.Products[j]);
                        break;
                    }
                    else if (j == 0)
                    {
                        check = false;
                        message += neededProducts[i].ProductName;
                        message += " ";
                    }
                }
                if (!check) break;
            }
        }
        
        ShowMessage.Invoke(); // invoking event of changing message
        if (check)  Delete(products); 
        return check;
    }
    // delete products from warehouse
    public override void Delete(List<Product> products)
    {
        for (int i = 0; i < products.Count; i++)
        {
            products[i].transform.parent = transform;
            StartCoroutine(used.LinearMove(products[i], products[i].transform.position, transform.TransformPoint(Vector3.zero)));
            products[i].Delete = true;
        }
    }


}
