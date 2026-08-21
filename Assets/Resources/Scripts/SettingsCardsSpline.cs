
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class SettingsCardsSpline : MonoBehaviour
{

    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private GameObject[] settingsCards;
    [SerializeField] private float cardSpacing = 0.25f;
    [SerializeField] private float firstCardPosition = 0.125f;

    //private readonly List<GameObject> _handCards = new();

    private void Awake()
    {
        var i = 0;
        foreach (var card in settingsCards)
        {
            var positionOnSpline = firstCardPosition + i * cardSpacing;
            splineContainer.Evaluate(positionOnSpline, out var pos, out var tangent, out var up);

            Vector3 finalPosition = pos;
            Vector3 cardForwardDir = -up;
            Vector3 splineTangentDir = tangent;
            var cardUpDir = Vector3.Cross(cardForwardDir, splineTangentDir).normalized;
            Quaternion finalRotation = Quaternion.LookRotation(cardForwardDir, cardUpDir);

            if (i == 1 || i == 2)
            {
                finalPosition.y = -63f; // Postavljam finalni cilj na 200, nzm kako radi

                // Rotacija na Z osi (Euler uglovi)
                Vector3 euler = finalRotation.eulerAngles;
                if (i == 1) euler.z = 6.5f;   
                if (i == 2) euler.z = -6.5f;  
                finalRotation = Quaternion.Euler(euler);
            }

            card.transform.DOMove(finalPosition, 0.25f);
            card.transform.DORotateQuaternion(finalRotation, 0.25f);

            i++;
            Debug.Log("pozicija" + finalPosition.ToString());
        }
    }



}
