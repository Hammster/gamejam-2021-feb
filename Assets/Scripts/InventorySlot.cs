using System;
using UnityEngine;


    public class InventorySlot : MonoBehaviour
    {

        public TileScriptableObject slotType;
        private SpriteRenderer spriteRenderer;
        private bool enabled = true;
        private InventoryManager inventoryManager;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Start()
        {
            inventoryManager = GetComponentInParent<InventoryManager>();
            spriteRenderer.sprite = slotType.fgSprite;
        }

        private void Update()
        {
            if (enabled && !spriteRenderer.enabled)
            {
                spriteRenderer.enabled = true;
            } else if ( !enabled && spriteRenderer.enabled)
            {
                spriteRenderer.enabled = false;
            }
        }

        public void EnableItem()
        {
            enabled = true;
        }
        
        public void DisableItem()
        {
            enabled = false;
        }

        private void OnMouseDown()
        {
            inventoryManager.DisableItem(slotType);
        }
    }
