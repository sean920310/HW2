using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MultiPlayerSlideState : MultiPlayerBaseState
{
    bool trailEnd = false;

    public MultiPlayerSlideState(MultiPlayerStatesManager context, MultiPlayerStateFactory factory)
        : base(context, factory)
    {
        _context = context;
        _factory = factory;
    }

    public override void EnterState()
    {
        _context.CanSlide = false;

        _context.PlayerAnimator.SetBool("isSlide", true);
        _context.PlayerAnimator.SetFloat("velocityX", 1f);

        _context.PlayerRigidbody.AddForce(new Vector2(_context.facingDirection() * _context.PlayerSlideForce, 0), ForceMode2D.Impulse);

        _context.PlayerBoxCollider.size = _context.SlideColliderSize;
        _context.PlayerBoxCollider.offset = _context.SlideColliderShift;

        _context.PlayerBoxCollider.sharedMaterial = _context.SlidePhysics;

        _context.startCorutine(slideTrail());

        _context.IsPlayerInvincible = _context.SlideInvincible;
    }

    public override void UpdateState()
    {
        if (_context.LastMove != Vector2.zero && _context.LastMove.x < 0f)
            _context.FacingLeft();
        else if (_context.LastMove != Vector2.zero && _context.LastMove.x > 0f)
            _context.FacingRight();

        _context.PlayerAnimator.SetFloat("velocityX", Mathf.Abs(_context.PlayerRigidbody.velocity.x));

        // invincible end when speed close to 0
        if (Mathf.Abs(_context.PlayerRigidbody.velocity.x) <= _context.SlideInvincibleEndSpeedX)
        {
            _context.IsPlayerInvincible = false;
        }

        // Show Sprite or not when invincible
        if (!_context.SlideInvincibleShowSprite)
        {
            if (_context.IsPlayerInvincible)
            {
                _context.PlayerSpriteRenderer.enabled = false;
            }
            else
            {
                _context.PlayerSpriteRenderer.enabled = true;
            }
        }

        CheckSwitchState();
    }

    public override void FixedUpdateState()
    {
        _context.PlayerRigidbody.velocity = new Vector2(_context.facingDirection() * Mathf.Abs(_context.PlayerRigidbody.velocity.x), _context.PlayerRigidbody.velocity.y);
    }

    public override void ExitState()
    {
        trailEnd = true;

        _context.startCorutine(SlideCoolDown());

        _context.PlayerAnimator.SetBool("isSlide", false);
        _context.PlayerAnimator.SetFloat("velocityX", 0f);

        _context.PlayerBoxCollider.size = _context.NormalColliderSize;
        _context.PlayerBoxCollider.offset = _context.NormalColliderShift;

        _context.PlayerBoxCollider.sharedMaterial = _context.NormalPhysics;

        _context.PlayerSpriteRenderer.enabled = true;
    }

    public override void CheckSwitchState()
    {
        if (_context.IsCrouchRelease)
        {
            _context.SwitchState(_factory.Idle());
        }
        else if (_context.IsAttackPress)
        {
            _context.SwitchState(_factory.MainWeapon());
        }
        else if (_context.IsBlockingPress)
        {
            _context.SwitchState(_factory.BlockingWeapon());
        }
    }

    IEnumerator SlideCoolDown()
    {
        yield return new WaitForSeconds(_context.PlayerSlideCoolDownTime);
        _context.CanSlide = true;
    }

    IEnumerator slideTrail()
    {
        while(!trailEnd)
        {
            GameObject tempObject = new GameObject();
            tempObject.transform.SetPositionAndRotation(_context.transform.position, _context.transform.rotation);
            tempObject.transform.localScale = _context.transform.localScale;    
            SpriteRenderer sr = tempObject.AddComponent<SpriteRenderer>();
            sr.material = new Material(Shader.Find("Sprites/Default"));
            SpriteRenderer playerSR = _context.GetComponent<SpriteRenderer>();

            sr.sprite = playerSR.sprite;
            sr.renderingLayerMask = playerSR.renderingLayerMask;
            sr.sortingLayerID = playerSR.sortingLayerID;
            sr.sortingLayerName = playerSR.sortingLayerName;
            sr.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            GameObject.Destroy(tempObject, _context.SlideTrailDestoryTime);

            _context.startCorutine(trailAlphaChange(tempObject));

            yield return new WaitForSeconds(_context.SlideTrailSpawnTime);
        }
    }
    IEnumerator trailAlphaChange(GameObject trailObj)
    {
        SpriteRenderer sr = trailObj.GetComponent<SpriteRenderer>();
        float aliveTime = 0, alpha = 1.0f;
        float maxAliveTime = _context.SlideTrailDestoryTime;

        while (trailObj != null)
        {
            aliveTime += 0.01f;
            alpha = Mathf.Lerp(0.8f, 0f, aliveTime / maxAliveTime);
            sr.color = new Color(1.0f, 1.0f, 1.0f, alpha);

            yield return new WaitForSeconds(0.01f);
        }

    }
}
