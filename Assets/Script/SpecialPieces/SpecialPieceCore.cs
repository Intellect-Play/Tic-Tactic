using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class SpecialPieceCore : PieceMovePlayer
{
    public Animator animator;
    public AnimatorOverrideController overrideController;
    public Sprite sprite;
    public SpecialPieceData specialPieceData;

    public override void Start()
    {
        base.Start();

        GetComponent<RectTransform>().localScale = new Vector2(2f, 2f);

    }
    public void SetupSpecial(SpecialPieceData _specialPieceData)
    {
        GetComponent<Image>().sprite = _specialPieceData.XSprite;        
        specialPieceData = _specialPieceData;
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (animator == null)
        {
            Debug.LogError("Animation component is missing on " + gameObject.name);
        }
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;
        if (playerValue == PieceType.Enemy)
        {
            overrideController["IdlePlaceholder"] = specialPieceData.EnemyAnimeIdle;
            overrideController["AttackPlaceholder"] = specialPieceData.EnemyAnimeAttack;
        }
        else
        {
            overrideController["IdlePlaceholder"] = specialPieceData.PlayerAnimeIdle;
            overrideController["AttackPlaceholder"] = specialPieceData.PlayerAnimeAttack;

        }
       // animator.Rebind();

        if (_specialPieceData.EnemyBombs.Count > 0 && playerValue == PieceType.Enemy)
        {
            Debug.Log("Enemy Bombs Count: " + specialPieceData.EnemyBombs.Count);
            overrideController["BombPlaceholder1"] = specialPieceData.EnemyBombs[0];
            overrideController["BombPlaceholder2"] = specialPieceData.EnemyBombs[1];
            overrideController["BombPlaceholder3"] = specialPieceData.EnemyBombs[1];

            //animator.SetTrigger("Bomb");
        }
        else if (_specialPieceData.PlayerBombs.Count > 0 && playerValue == PieceType.Player)
        {
            Debug.Log("Player Bombs Count: " + _specialPieceData.PlayerBombs.Count);
            overrideController["BombPlaceholder1"] = specialPieceData.PlayerBombs[0];
            overrideController["BombPlaceholder2"] = specialPieceData.PlayerBombs[1];
            overrideController["BombPlaceholder3"] = specialPieceData.PlayerBombs[2];
     

        }
        animator.Rebind();

    }
    public void AddToList()
    {
        AllSpecialPiecesMove.Instance.AddPiece(this);
    }

    public virtual void MoveEnd(Action onMoveComplete)
    {
        onMoveComplete?.Invoke();
    }


    public abstract void MoveStart(Action onMoveComplete);

    public void RemoveFromList()
    {
        AllSpecialPiecesMove.Instance.RemovePiece(this);
    }
    public override void ChangeCell(Cell cell)
    {
        base.ChangeCell(cell);
        //AddToList();
    }
    public override void Back()
    {
        RemoveFromList();
        base.Back();
    }
    public override void DestroyPiece()
    {
        RemoveFromList();
        base.DestroyPiece();
    }
    public override void Placed(bool _isPlaced)
    {
        if (!IsPlaced&&_isPlaced)
        {

            AddToList();
        }
        base.Placed(_isPlaced);

    }


    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (playerValue != PieceType.Player) return;
        base.OnBeginDrag(eventData);

    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (playerValue != PieceType.Player) return;
        base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (playerValue != PieceType.Player) return;
        base.OnEndDrag(eventData);
    }
}
