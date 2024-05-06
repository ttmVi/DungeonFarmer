using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    public static ItemsManager itemsManager;
    [SerializeField] private GameObject player;

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
        GameObject item = Instantiate(placeholder, instantiatePosition, rotation);
        item.GetComponent<ItemInfo>().SetItemData(itemData);
        item.GetComponent<ItemInfo>().SetPicker(player);
        item.AddComponent<BoxCollider2D>();
        item.GetComponent<BoxCollider2D>().isTrigger = true;
    }
}
