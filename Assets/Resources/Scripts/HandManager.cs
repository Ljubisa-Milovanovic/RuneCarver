using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

namespace Resources.Scripts
{
    public class HandManager : MonoBehaviour
    {
        [SerializeField] private int maxHandSize;
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private SplineContainer splineContainer;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private GameObject cardHolder;

        private readonly List<GameObject> _handCards = new();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                DrawCard();
        }


        private void DrawCard()
        {
            if (_handCards.Count >= maxHandSize) return;
            GameObject card = Instantiate(cardPrefab, spawnPoint.position, spawnPoint.rotation);
            card.transform.SetParent(cardHolder.transform, false); //stavi true da vidis sta ce da se desi
            _handCards.Add(card);
            UpdateCardPosition();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void UpdateCardPosition()
        {
            if (_handCards.Count == 0)
            {
                Debug.Log("odigrane sve kartice, treba se pozove sledeca funcija za novu turu");
                return;
            }

            var cardSpacing = 1f / maxHandSize; //splien je float od 0f do 1f
            var firstCardPosition = 0.5f - (_handCards.Count - 1) * cardSpacing / 2;
            var spline = splineContainer.Spline;
            
            var i = 0;
            foreach (var card in _handCards)
            {
                var position = firstCardPosition + (30*i++) * cardSpacing; //postion of each card
                Vector3 splinePosition = spline.EvaluatePosition(position); //converts it to world postion

                Vector3 cardForwardDir = -spline.EvaluateUpVector(position);
                Vector3 splineTangentDir = spline.EvaluateTangent(position);
                var cardUpDir = Vector3.Cross(cardForwardDir, splineTangentDir).normalized;
                var rotation = Quaternion.LookRotation(cardForwardDir, cardUpDir);
                card.transform.DOMove(splinePosition, 0.25f);
                card.transform.DORotateQuaternion(rotation, 0.25f);
            }
        }
    }
}