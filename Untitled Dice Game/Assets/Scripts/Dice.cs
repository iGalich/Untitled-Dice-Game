using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour
{
    [SerializeField] private Sprite[] _diceSprites;
    [SerializeField] private int _loopCount = 20;
    private SpriteRenderer _spriteRenderer;
    private int _whosTurn = 1;
    private bool _coroutineAllowed = true;
    private WaitForSeconds _spriteChangeDelay = new WaitForSeconds(0.05f);

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _diceSprites[5];
    }

    private void OnMouseDown()
    {
        if (_coroutineAllowed)
        {
            StartCoroutine(RollTheDice());
        }
    }

    private IEnumerator RollTheDice()
    {
        _coroutineAllowed = false;
        int randomDiceSide = 0;

        for (int i = 0; i <= _loopCount; i++)
        {
            randomDiceSide = Random.Range(0, 6);
            _spriteRenderer.sprite = _diceSprites[randomDiceSide];
            yield return _spriteChangeDelay;
        }

        GameManager.Instance.DiceSideThrown = randomDiceSide + 1;

        GameManager.Instance.MovePlayer(_whosTurn);

        _whosTurn++;
        if (_whosTurn == 3)
            _whosTurn = 1;

        _coroutineAllowed = true;
    }
}