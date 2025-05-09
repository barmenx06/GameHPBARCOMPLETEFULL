using UnityEngine;
using UnityEngine.UI;


public class MobileAttackButton : MonoBehaviour
{
    public PlayerAttack playerAttack;

    public void OnAttackButtonPressed()
    {
        if (playerAttack != null)
        {
            playerAttack.TriggerAttack();
        }
    }
}