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

    public void InstantiateItemInRange(GameObject placeholder, Items itemData, Vector2 instantiateCenter, float minInstantiateRadius, float maxInstantiateRadius, Quaternion rotation)
    {
        Vector2 randomPosition = GetRandomPositionWithinRange(instantiateCenter, minInstantiateRadius, maxInstantiateRadius);

        GameObject item = Instantiate(placeholder, randomPosition, rotation);
        item.GetComponent<ItemInfo>().SetItemData(itemData);
        item.GetComponent<ItemInfo>().SetPicker(player);
        item.AddComponent<BoxCollider2D>();
        item.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void InstantiateItemInLine(GameObject placeholder, Items itemData, Vector2 instantiateCenter, float minInstantiateRadius, float maxInstantiateRadius, Quaternion rotation)
    {
        Vector2 randomPosition = GetRandomPositionWithinRange(instantiateCenter, minInstantiateRadius, maxInstantiateRadius);
        randomPosition = new Vector2(randomPosition.x, instantiateCenter.y);

        GameObject item = Instantiate(placeholder, randomPosition, rotation);
        item.GetComponent<ItemInfo>().SetItemData(itemData);
        item.GetComponent<ItemInfo>().SetPicker(player);
        item.AddComponent<BoxCollider2D>();
        item.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void InstantiateItemInSemiRange(GameObject placeholder, Items itemData, Vector2 instantiateCenter, float minInstantiateRadius, float maxInstantiateRadius, Quaternion rotation)
    {
        restart:
        Vector2 randomPosition = GetRandomPositionWithinRange(instantiateCenter, minInstantiateRadius, maxInstantiateRadius);
        if (randomPosition.y < instantiateCenter.y)
        {
            goto restart;
        }

        GameObject item = Instantiate(placeholder, randomPosition, rotation);
        item.GetComponent<ItemInfo>().SetItemData(itemData);
        item.GetComponent<ItemInfo>().SetPicker(player);
        item.AddComponent<BoxCollider2D>();
        item.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private Vector2 GetRandomPositionWithinRange(Vector2 center, float minRadius, float maxRadius)
    {
        float randomAngle = Random.value * 360f;
        float randomRadius = Random.Range(minRadius, maxRadius);

        Vector2 randomPosition;
        randomPosition.x = center.x + randomRadius * Mathf.Cos(randomAngle * Mathf.Deg2Rad);
        randomPosition.y = center.y + randomRadius * Mathf.Sin(randomAngle * Mathf.Deg2Rad);

        return randomPosition;
    }
}
