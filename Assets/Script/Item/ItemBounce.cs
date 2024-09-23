using DG.Tweening;
using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class ItemBounce : MonoBehaviour
    {
        private Transform spriteTrans;
        private BoxCollider2D coll;

        public float gravity = -3.5f;
        private bool isGround;
        private float distance;
        private Vector2 direction;
        private Vector3 targetPos;
        private Animator animator;

        private void Awake()
        {
            spriteTrans = transform.GetChild(0);
            coll = GetComponent<BoxCollider2D>();
            animator = GetComponentInChildren<Animator>();
            coll.enabled = false;
        }

        private void Update()
        {
            Bounce();
        }

        public void InitBounceItem(Vector3 target, Vector2 dir)
        {
            direction = dir;
            targetPos = target;
            distance = Vector3.Distance(target, transform.position);

            spriteTrans.position += Vector3.up * 1.5f;
        }

        private void Bounce()
        {
            if (animator.enabled) return;
            isGround = spriteTrans.position.y <= transform.position.y;

            if (Vector3.Distance(transform.position, targetPos) > 0.1f)
            {
                transform.position += (Vector3)direction * distance * -gravity * Time.deltaTime;
            }

            if (!isGround)
            {
                spriteTrans.position += Vector3.up * gravity * Time.deltaTime;
            }
            else
            {
                spriteTrans.position = transform.position;
                coll.enabled = true;
                animator.enabled = true;
            }
        }
    }
}

