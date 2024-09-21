using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState { FreeRoam, Dialogue, Battle}

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;


    GameState state;

    private void Start()
    {
        DialogueManager.Instance.OnShowDialogue += () =>
        {
            state = GameState.Dialogue;
        };
        DialogueManager.Instance.OnHideDialogue += () =>
        {
            if (state == GameState.Dialogue)
                state = GameState.FreeRoam;
        };
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
            Debug.Log("In free roam");
        } else if (state == GameState.Dialogue)
        {
            DialogueManager.Instance.HandleUpdate();
            Debug.Log("In dialogue");
        } else if (state == GameState.Battle)
        {
            Debug.Log("In battle");
        }
    }
}
