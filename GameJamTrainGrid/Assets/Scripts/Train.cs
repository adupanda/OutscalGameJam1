using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    int trainIndex;
    int pathPositionIndex;

    [SerializeField]
    float moveDelay;

    SpriteRenderer spriteRenderer;

    Vector2 finalPosition;

    

    public void SetColor(Color trainColor)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = trainColor;
    }

    public void SetTrainIndex(int index)
    {
        trainIndex = index;
    }
    public void SetFinalPosition(Vector2 finalPos)
    {
        finalPosition = finalPos;
        StartCoroutine(MoveOnPath());
    }
    public IEnumerator MoveOnPath()
    {
        bool _isMoving = false;
        while (!_isMoving)
        {
            
            

            transform.position = GridManager.Instance.GetPositionFromPath(trainIndex, pathPositionIndex);
            GridManager.Instance.UpdateTrainPositions(trainIndex, transform.position);
            
            Vector2 Vec2Pos = new Vector2(transform.position.x, transform.position.y);
            
            if (Vec2Pos == finalPosition)
            {
                _isMoving = true;
                StopAllCoroutines();
            }
            pathPositionIndex++;
            yield return new WaitForSeconds(moveDelay);

        }

    }

    

}
