using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    Sprite intersectionSprite;

    
    public List<int> paths = new List<int>();


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void SetCellColor(Color cellColor)
    {
        spriteRenderer.color = cellColor;
        
    }
    
    public void SetSprite(Sprite spriteToSet)
    {
        spriteRenderer.sprite = spriteToSet;
    }
    

    public void AddPathReference(int index)
    {
        if(paths.Contains(index)) { return; }

        paths.Add(index);
        if(paths.Count > 1 ) 
        {
            spriteRenderer.sprite = intersectionSprite;
        }
        
    }

    public List<int> GetPathReferences()
    {
        return paths;
    }
}
