using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CloudGenerator : MonoBehaviour
{
    public Sprite cloudSprite;
    public int maxCount = 30, curCount;
    public float curXPos, curYPos;
    public GameObject prefab;
    public Vector3 firstPrefabPosition = new Vector3(0, -3, 0);
    public Transform parent;
    private bool _isFirst = true;
    
    public void Generate()
    {
        curXPos = firstPrefabPosition.x;
        curYPos = firstPrefabPosition.y;

        while (curCount < maxCount)
        {
            var sizeXOffset = cloudSprite.rect.width / 100 / 2 * prefab.transform.localScale.x;
            var sizeYOffset = cloudSprite.rect.height / 100 / 2 * prefab.transform.localScale.y;
            if (_isFirst)
            {
                var obj = Instantiate(prefab, new Vector3(curXPos, curYPos, 0), Quaternion.identity);
                obj.transform.SetParent(parent, true);
                curCount++;
                _isFirst = false;
            }
            else
            {
                var pos = new Vector3(Random.Range(-2 + sizeXOffset, 2 - sizeXOffset),
                    curYPos + Random.Range(1 + sizeYOffset, 2.5f + sizeYOffset), 0);

                if (Math.Abs(curXPos - pos.x) < 0.4f) continue;

                var obj = Instantiate(prefab, pos, Quaternion.identity);
                obj.transform.SetParent(parent, true);
                curXPos = pos.x;
                curYPos = pos.y;
                curCount++;
            }
        }
    }
}