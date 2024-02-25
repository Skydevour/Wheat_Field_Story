using System;
using CommonFramework.Runtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Sprite normalCursor;
    [SerializeField] private Sprite usingTool;
    [SerializeField] private Sprite usingSeed;
    [SerializeField] private Sprite usingItem;

    private Sprite currentCursorSprite;
    private Image cursorImage;
    private RectTransform fadeCanvas;
    private Camera mainCamera;
    private Vector3 mouseWorldPos;
    private Vector3Int mouseGridPos;
    private Grid currentGrid;
    private Data.ItemDetails currentItem;

    private void OnEnable()
    {
        EventCenter.StartListenToEvent<ItemSelectedEvent>(ItemSelected);
    }

    private void OnDisable()
    {
        EventCenter.StopListenToEvent<ItemSelectedEvent>(ItemSelected);
    }

    private void Start()
    {
        fadeCanvas = GameObject.FindGameObjectWithTag("FadeCanvas").GetComponent<RectTransform>();
        cursorImage = fadeCanvas.transform.GetChild(0).GetComponent<Image>();
        currentCursorSprite = normalCursor;
        SetCursorImage(normalCursor);
    }

    private void Update()
    {
        if (fadeCanvas == null)
        {
            return;
        }

        cursorImage.transform.position = Input.mousePosition;
        // 有点不太合理，后面再说
        // if (!IsInteractWithUI())
        // {
        //     SetCursorImage(currentCursorSprite);
        // }
        // else
        // {
        //     SetCursorImage(normalCursor);
        // }
    }

    private void SetCursorImage(Sprite cursorSprite)
    {
        cursorImage.sprite = cursorSprite;
        cursorImage.color = new Color(1, 1, 1, 1);
    }

    private bool IsInteractWithUI()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }

        return false;
    }

    private void ItemSelected(ItemSelectedEvent evt)
    {
        if (!evt.IsSelected)
        {
            SetCursorImage(normalCursor);
        }
        else
        {
            currentCursorSprite = evt.ItemDetails.ItemType switch
            {
                Enums.ItemType.Commodity => usingItem,
                Enums.ItemType.ChopTool => usingTool,
                Enums.ItemType.Seed => usingSeed,
                Enums.ItemType.HoeTool => usingTool,
                _ => normalCursor
            };
            SetCursorImage(currentCursorSprite);
        }
    }

    /// <summary>
    /// 待补充
    /// </summary>
    private void CheckCursorUse()
    {
        mouseWorldPos =
            mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                Input.mousePosition.z));
        mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);
        Data.TileDetails currentTile = new Data.TileDetails();
        if (currentTile != null)
        {
            switch (currentItem.ItemType)
            {
                case Enums.ItemType.Commodity:
                    break;
                case Enums.ItemType.HoeTool:
                    if (currentTile.CanDig)
                    {
                        SetCursorUse();
                    }
                    else
                    {
                        SetCursorNotUse();
                    }

                    break;
                case Enums.ItemType.WaterTool:
                    if (currentTile.DaySinceDug > -1 && currentTile.DaySinceWater == -1)
                    {
                        SetCursorUse();
                    }
                    else
                    {
                        SetCursorNotUse();
                    }

                    break;
            }
        }
    }

    /// <summary>
    /// 待补充
    /// </summary>
    private void SetCursorUse()
    {
    }

    /// <summary>
    /// 待补充
    /// </summary>
    private void SetCursorNotUse()
    {
    }
}