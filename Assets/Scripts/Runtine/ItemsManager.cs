using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemsManager : MonoBehaviour
{
    public static ItemsManager itemsManager;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject itemPlaceholder;

    private void Awake()
    {
        itemsManager = this;
    }

    public void InstantiateRealRandomItems(GameObject placeholder, Items[] itemsList, Vector2 instantiatePosition, Quaternion rotation)
    {
        StartCoroutine(RandomItems(placeholder, itemsList, instantiatePosition, rotation));
    }

    private IEnumerator RandomItems(GameObject placeholder, Items[] itemsList, Vector2 instantiatePosition, Quaternion rotation)
    {
        int numberOfInstantiateItems = Random.Range(0, itemsList.Length);
        int itemsLeft = itemsList.Length;
        if (placeholder == null)
        {
            placeholder = itemPlaceholder;
        }

        foreach (var item in itemsList)
        {
            if (itemsLeft > numberOfInstantiateItems)
            {
                int random = Random.Range(0, 2);
                if (random == 0) { continue; }
                else if (random == 1)
                {
                    InstantiateItem(placeholder, item, instantiatePosition, rotation);
                    numberOfInstantiateItems--;
                    itemsLeft--;
                }
            }
            else
            {
                InstantiateItem(placeholder, item, instantiatePosition, rotation);
                numberOfInstantiateItems--;
                itemsLeft--;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void InstantiateRandomItems(GameObject placeholder, Items[] itemsList, Vector2 instantiatePosition, Quaternion rotation)
    {
        int numberOfItems = 0;
        //restart:
        foreach (var item in itemsList)
        {
            //int random = Random.Range(0, 1);
            //if (random == 0) { continue; }
            //else if (random == 1)
            //{
                if (placeholder != null)
                {
                    InstantiateItemInSemiRange(placeholder, item, instantiatePosition, 0.1f, 1.5f, rotation);
                }
                else
                {
                    InstantiateItemInSemiRange(itemPlaceholder, item, instantiatePosition, 0.1f, 1.5f, rotation);
                }
                numberOfItems++;
            //}
        }

        //if (numberOfItems == 0) { goto restart; }
    }

    public void InstantiateItem(GameObject placeholder, Items itemData, Vector2 instantiatePosition, Quaternion rotation)
    {
        GameObject item = Instantiate(placeholder, instantiatePosition, rotation);
        item.GetComponent<ItemInfo>().SetItemData(itemData);
        item.GetComponent<ItemInfo>().SetPicker(player);
        //item.AddComponent<BoxCollider2D>();
        //item.GetComponent<BoxCollider2D>().isTrigger = true;
        GameManager manager = FindObjectOfType<GameManager>();
        if (manager.inDungeon)
        {
            item.transform.parent = manager.dungeon.transform;
        }
        else if (manager.inFarm)
        {
            item.transform.parent = manager.farm.transform;
        }
    }

    public void InstantiateItemInRange(GameObject placeholder, Items itemData, Vector2 instantiateCenter, float minInstantiateRadius, float maxInstantiateRadius, Quaternion rotation)
    {
        Vector2 randomPosition = GetRandomPositionWithinRange(instantiateCenter, minInstantiateRadius, maxInstantiateRadius);

        GameObject item = Instantiate(placeholder, randomPosition, rotation);
        item.GetComponent<ItemInfo>().SetItemData(itemData);
        item.GetComponent<ItemInfo>().SetPicker(player);
        //item.AddComponent<BoxCollider2D>();
        //item.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void InstantiateItemInLine(GameObject placeholder, Items itemData, Vector2 instantiateCenter, float minInstantiateRadius, float maxInstantiateRadius, Quaternion rotation)
    {
        Vector2 randomPosition = GetRandomPositionWithinRange(instantiateCenter, minInstantiateRadius, maxInstantiateRadius);
        randomPosition = new Vector2(randomPosition.x, instantiateCenter.y);

        GameObject item = Instantiate(placeholder, randomPosition, rotation);
        item.GetComponent<ItemInfo>().SetItemData(itemData);
        item.GetComponent<ItemInfo>().SetPicker(player);
        //item.AddComponent<BoxCollider2D>();
        //item.GetComponent<BoxCollider2D>().isTrigger = true;
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
        //item.AddComponent<BoxCollider2D>();
        //item.GetComponent<BoxCollider2D>().isTrigger = true;
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

    public void UsePotion(InputAction.CallbackContext context, Items potion)
    {
        if (context.started && potion.GetPotionData().GetPotionEffectEvent() != null && GetComponent<InventoryManager>().CheckForItems(potion))
        {
            potion.GetPotionData().GetPotionEffectEvent().Invoke();
            GetComponent<InventoryManager>().UseUpPotion(potion);
        }
    }

    public void UsePotion(Items potion)
    {
        if (potion.GetPotionData().GetPotionEffectEvent() != null && GetComponent<InventoryManager>().CheckForItems(potion))
        {
            potion.GetPotionData().GetPotionEffectEvent().Invoke();
            GetComponent<InventoryManager>().UseUpPotion(potion);
        }
    }
}
