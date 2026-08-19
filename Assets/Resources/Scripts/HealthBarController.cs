using UnityEngine;
using QFSW.QC;
public class HealthBarController : MonoBehaviour
{

    [Command]
    public void loseOneHP()
    {
        int CurentHP = BattleManager.Instance.loseHPAmount(1);
        UpdateHealthVisuals(11-CurentHP -1); // ovde treba da se poveze sa max hp i cureent al sad nmg
    }

    [Command]
    private void UpdateHealthVisuals(int i)
    {
        GameObject child = transform.GetChild(i).gameObject;
        child.SetActive(true);
    }

    [Command]
    private void ResetHealthVisuals()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

}
