using System;
using UnityEngine;

public class PlayerBagItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D itemCollider2D;

    private Sprite sprite;

    public int itemId;
    public Data.ItemDetails itemDetail;

    private void Start()
    {
        // 物品存在
        if (itemId != 0)
        {
            Init(itemId);
        }
    }

    public void Init(int id)
    {
        itemId = id;
        itemDetail = PlayerBagManager.Instance.GetItemDetails(itemId);
        if (itemDetail != null)
        {
            spriteRenderer.sprite = itemDetail.ItemIcon;
            // 频繁访问属性效率很低
            sprite = spriteRenderer.sprite;

            Vector2 spriteSize = new Vector2(sprite.bounds.size.x, sprite.bounds.size.y);
            itemCollider2D.size = spriteSize;
            // 避免图片轴心偏离中心点
            itemCollider2D.offset =
                new Vector2(sprite.bounds.center.x, sprite.bounds.center.y);
        }
    }
}