using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProduct
{
    public string ProductName { get; set; }
    public void Initialize();
}
public interface ICreate
{
    public IEnumerator CreateProduct();
}
public interface IDelete
{
    public void Delete(List<Product> products);
}
public abstract class Factory : MonoBehaviour, ICreate, IDelete
{
    public abstract IProduct GetProduct(Vector3 position);
    public abstract IEnumerator CreateProduct();
    public abstract void Delete(List<Product> products);
}
