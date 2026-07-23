using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;
using static UnityEditor.PlayerSettings;

public class HandManager : MonoBehaviour
{
    [SerializeField] private int MaxHandSize;
    [SerializeField] private GameObject CardPrefab;
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private GameObject CardHolder;

    private List<GameObject> handCards = new();

    //samo trnt za test
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            DrawCard();
    }


    private void DrawCard()
    {
        if (handCards.Count >= MaxHandSize) return;
        GameObject card = Instantiate(CardPrefab, SpawnPoint.position, SpawnPoint.rotation);
        card.transform.SetParent(CardHolder.transform,false);//stavi true da vidis sta ce da se desi
        handCards.Add(card);
        UpdateCardPosition();
    }

    private void UpdateCardPosition()
    {
        if (handCards.Count == 0)
        {
            Debug.Log("odigrane sve kartice, treba se pozove sledeca funcija za novu turu");
            return;
        }
        float CardSpacing = 1f/MaxHandSize; //splien je float od 0f do 1f
        float firstCardPostion = 0.5f - (handCards.Count-1) * CardSpacing/2;
        Spline spline = splineContainer.Spline;
        for (int i = 0; i < handCards.Count; i++)
        {
            float position = firstCardPostion + i * CardSpacing; //postion of each card
            Vector3 splinePosition = spline.EvaluatePosition(position); //converts it to world postion

            Vector3 cardForwardDir = spline.EvaluateUpVector(position);
            Vector3 splineTangentDir = spline.EvaluateTangent(position);
            Vector3 cardUpDir = Vector3.Cross(cardForwardDir, splineTangentDir).normalized;
            Quaternion rotation = Quaternion.LookRotation(cardForwardDir, cardUpDir);
            handCards[i].transform.DOMove(splinePosition, 0.25f);
            handCards[i].transform.DORotateQuaternion(rotation, 0.25f);
        }
    }
}
