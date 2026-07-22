using UnityEngine;

public class RandomButtonOrder : MonoBehaviour
{
    private void OnEnable()
    {
        ShuffleChildren();
    }

    private void ShuffleChildren()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            int randomIndex = Random.Range(i, childCount);
            transform.GetChild(randomIndex).SetSiblingIndex(i);
        }
    }
}
