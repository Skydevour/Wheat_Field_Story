using System;
using UnityEngine;

namespace PlayerController
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D playerRigidBody;
        [SerializeField] private float playerSpeed;
        [SerializeField] private Animator[] playerAnimators;//人物身上得所有animtor;
        
        private float inputX;
        private float inputY;
        private Vector2 movementInput;//输入得二维向量；
        private bool isMoving;//是否正在移动；
        
        void Update()
        {
            PlayerInput();
            SwitchAnimation();
        }

        private void FixedUpdate()
        {
            Movement();//刚体移动一般放入FixedUpData
        }

        /// <summary>
        /// 玩家移动输入
        /// </summary>
        private void PlayerInput()
        {
            if (inputY == 0)
            {
                inputX = Input.GetAxisRaw("Horizontal");
            }

            if (inputX == 0)
            {
                inputY = Input.GetAxisRaw("Vertical");
            }

            movementInput = new Vector2(inputX, inputY);
            isMoving = movementInput != Vector2.zero;
        }

        /// <summary>
        /// 刚体移动
        /// </summary>
        private void Movement()
        {
            playerRigidBody.MovePosition(playerRigidBody.position + movementInput * playerSpeed * Time.deltaTime);
        }

        /// <summary>
        /// 转换人物动画
        /// </summary>
        private void SwitchAnimation()
        {
            foreach (var playerAnimator in playerAnimators)
            {
                playerAnimator.SetBool("IsMoving", isMoving);
                if (isMoving)
                {
                    playerAnimator.SetFloat("InputX", inputX);
                    playerAnimator.SetFloat("InputY", inputY);
                }
            }
        }
    }
}
