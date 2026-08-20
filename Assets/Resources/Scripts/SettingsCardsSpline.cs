
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
            var position = firstCardPosition + i++ * cardSpacing; //position of each card //0.125 ; 0.375 ; 0.5 ; 0.75

            splineContainer.Evaluate(position, out var pos, out var tangent, out var up);
            Vector3 splinePosition = pos;
            Vector3 cardForwardDir = -up;
            Vector3 splineTangentDir = tangent;

            var cardUpDir = Vector3.Cross(cardForwardDir, splineTangentDir).normalized;
            var rotation = Quaternion.LookRotation(cardForwardDir, cardUpDir);
            //Debug.Log("proso");
            card.transform.DOMove(splinePosition, 0.25f);
            card.transform.DORotateQuaternion(rotation, 0.25f);

        }
        Debug.Log("awake");
    }

    private void Start()
    {
        // mislim da bi 2. i 3, kartica trebalo da imaju pos y 200 i rotaciju Z oko 6,7 (tjst -6,-7 za 3. karticu); ali nmg sad to da namestim jer me nesto jebe a nemam net da pogledam
        //mogao bi to da namestim zasebno na svakoj kartici ili to nije resenje
    }

}
