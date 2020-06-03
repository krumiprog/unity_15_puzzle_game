using System.Collections;
using UnityEngine;

public class MoveTile : MonoBehaviour
{
    public int IndexInList { get; set; }
    
    [SerializeField] private float _timeMove = 0.5f;
    [SerializeField] private float _stepTile = 1.2f;

    public void ChangePosition(MoveDirection direction)
    {
        Vector3 targetPos = transform.position;

        switch (direction)
        {
            case MoveDirection.Up:
                targetPos += new Vector3(0f, 0f, _stepTile);
                break;
            case MoveDirection.Right:
                targetPos += new Vector3(_stepTile, 0f, 0f);
                break;
            case MoveDirection.Down:
                targetPos += new Vector3(0f, 0f, -_stepTile);
                break;
            case MoveDirection.Left:
                targetPos += new Vector3(-_stepTile, 0f, 0f);
                break;
            default: return;
        }

        StartCoroutine(Move(targetPos));
    }

    public IEnumerator Move(Vector3 targetPos)
    {
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;

        while (elapsedTime < _timeMove)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, (elapsedTime / _timeMove));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.position = targetPos;
        GameManager.Instance.CheckSolved();
    }

    private void OnMouseDown()
    {
        ChangePosition(GameManager.Instance.DetermDirection(gameObject));
    }
}
