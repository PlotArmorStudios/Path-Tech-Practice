using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    private Animator _animtor;
    void Start()
    {
        _animtor = GetComponent<Animator>();
    }

    public void SwitchState(string stateName)
    {
        _animtor.Play(stateName);
    }
}
