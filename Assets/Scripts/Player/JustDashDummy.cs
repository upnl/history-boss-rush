using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustDashDummy : MonoBehaviour
{
    // [SerializeField] private LayerMask HitboxLayer;
    private float dashStartUpTime = 0.05f;
    private float dashInvulnTime = 0.3f;
    private IPlayerController playerController;
    
    public void StartUp(float startUpTime, float invulvTime, IPlayerController playerController)
    {
        dashStartUpTime = startUpTime;
        dashInvulnTime = invulvTime;
        this.playerController = playerController;
    }

    private float totalTime = 0f;
    private void Update()
    {
        totalTime += Time.deltaTime;
        if (dashStartUpTime + dashInvulnTime < totalTime)
        {
            Destroy(gameObject);
        }
    }

    private bool isDashSuccess = false;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("DeathHitBox"))
        {
            if( totalTime > dashStartUpTime && totalTime < dashStartUpTime + dashInvulnTime && !isDashSuccess)
            {
                isDashSuccess = true;
                playerController.OnDashSuccess();
                Destroy(gameObject);
            }
        }
    }


}
