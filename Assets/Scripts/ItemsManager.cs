using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    public static ItemsManager itemsManager;

    private void Awake()
    {
        itemsManager = this;
    }

    private void Update()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void InstantiateItem(GameObject placeholder, Items itemData, Vector2 instantiatePosition, Quaternion rotation)
    {
        Instantiate(placeholder, instantiatePosition, rotation);
        placeholder.GetComponent<ItemInfo>().SetItemData(itemData);
        placeholder.AddComponent<BoxCollider2D>();
        placeholder.GetComponent<BoxCollider2D>().isTrigger = true;
    }
}
