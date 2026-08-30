using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using QFSW.QC;

namespace Assets.Resources.Scripts
{
    public class HandManager : MonoBehaviour
    {
        [SerializeField] private int maxHandSize;
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private SplineContainer splineContainer;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private GameObject cardHolder;

        private readonly List<GameObject> _handCards = new();

        [Command]
        private void DebugReset()
        {
            foreach (var card in _handCards)
                Destroy(card);
            _handCards.Clear();
        }

        [Command]
        private void DrawCard()
        {
            if (_handCards.Count >= maxHandSize) return;
            var card = Instantiate(cardPrefab, spawnPoint.position, spawnPoint.rotation);
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

            // Making curvature
            var cardSpacing = 1f / _handCards.Count;
            var firstCardPosition = 0.5f - (_handCards.Count - 1) * cardSpacing / 2;

            var i = 0;
            foreach (var card in _handCards)
            {
                var position = firstCardPosition + i++ * cardSpacing; //position of each card
                
                splineContainer.Evaluate(position, out var pos, out var tangent, out var up);
                Vector3 splinePosition   = pos;
                Vector3 cardForwardDir   = -up;
                Vector3 splineTangentDir = tangent;
                
                var cardUpDir = Vector3.Cross(cardForwardDir, splineTangentDir).normalized;
                var rotation = Quaternion.LookRotation(cardForwardDir, cardUpDir);
                card.transform.DOMove(splinePosition, 0.25f);
                card.transform.DORotateQuaternion(rotation, 0.25f);
            }
        }
    }
}