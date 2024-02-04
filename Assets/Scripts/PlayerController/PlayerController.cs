using System;
using UnityEngine;

namespace PlayerController
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D playerRigidBody;
        [SerializeField] private float playerSpeed;
        private float inputX;
        private float inputY;
        private Vector2 movementInput;//输入得二维向量；
        private bool isMoving;//是否正在移动；
        
        void Update()
        {
            PlayerInput();
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
            if (inputY==0)
            {
                inputX = Input.GetAxisRaw("Horizontal");
            }

            if (inputX==0)
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
    }
}
