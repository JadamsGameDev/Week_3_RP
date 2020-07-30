using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum State
{
    DEFAULT,
    WIN,
    DEAD,
};

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private State CurrentState = State.DEFAULT;
    //private Text WinText;
    [SerializeField]
    private List<UIState> UIStates;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        SwitchState();
    }

    // Update is called once per frame
    private void SwitchState()
    {
        switch (CurrentState)
        {
            case State.DEFAULT:
                TurnOFf();
                UIStates[0].gameObject.SetActive(true);
                    
                break;
            case State.WIN:
                TurnOFf();
                UIStates[1].gameObject.SetActive(true);
                break;
            case State.DEAD:
                TurnOFf();
                UIStates[2].gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void DefaultState()
    {
        CurrentState = State.DEFAULT;

        SwitchState();
    }

    public void Win()
    {
        CurrentState = State.WIN;

        SwitchState();
    }

    public void Dead()
    {
        CurrentState = State.DEAD;

        SwitchState();
    }

    public void TurnOFf ()
    {
        foreach (UIState UIs in UIStates)
        {
            UIs.gameObject.SetActive(false);
        }
    }
}
