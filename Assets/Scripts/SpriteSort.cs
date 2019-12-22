using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

[ExecuteInEditMode]
public class SpriteSort : MonoBehaviour
{
    private SpriteRenderer sprite;
    private MeshRenderer skeletonAnimation;
    [SerializeField] private bool runOnUpdate = false;
    
    void Awake ()
    {
        skeletonAnimation = GetComponent<MeshRenderer>();
        this.sprite = GetComponent<SpriteRenderer>();
        SetPosition();
    }

    void Update ()
    {
        if (!this.runOnUpdate && !Application.isEditor) return;
        SetPosition();
    }

    void SetPosition () {
        Vector3 newPosition = this.transform.position;
        newPosition.z = this.transform.position.y;
        this.transform.position = newPosition;
        if (this.sprite)
        {
            this.sprite.sortingOrder = (int) -this.transform.position.y;
        }

        if (this.skeletonAnimation)
        {
            this.skeletonAnimation.sortingOrder = (int) -this.transform.position.y;
        }
    }
}
