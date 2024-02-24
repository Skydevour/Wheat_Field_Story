using System;
using System.Collections;
using CommonFramework.Runtime;
using Events.PlayerAnimtionEvents;
using UnityEngine;

namespace PlayerController
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D playerRigidBody;
        [SerializeField] private float playerSpeed;
        [SerializeField] private Animator[] playerAnimators;//人物身上得所有animtor;
        
        private float inputX;//键盘输入X
        private float inputY;//键盘输入Y
        private float mouseX;//根据鼠标位置播放动画
        private float mouseY;//根据鼠标位置播放动画
        private Vector2 movementInput;//输入得二维向量；
        
        private bool isMoving;//是否正在移动；
        private bool canNotInput;//玩家不能操作

        private void OnEnable()
        {
            EventCenter.StartListenToEvent<AfterSceneLoadEvent>(OnAfterSceneLoad);
            EventCenter.StartListenToEvent<BeforeSceneUnLoadEvent>(OnBeforeSceneUnLoad);
            EventCenter.StartListenToEvent<PlayerAnimationBeforeExecuteEvent>(OnPlayerAnimationBeforeExecute);
        }

        private void OnDisable()
        {
            EventCenter.StopListenToEvent<AfterSceneLoadEvent>(OnAfterSceneLoad);
            EventCenter.StopListenToEvent<BeforeSceneUnLoadEvent>(OnBeforeSceneUnLoad);
            EventCenter.StopListenToEvent<PlayerAnimationBeforeExecuteEvent>(OnPlayerAnimationBeforeExecute);
        }

        void Update()
        {
            if (canNotInput==false)
            {
                PlayerInput();
            }
            else
            {
                isMoving = false;
            }
            
            SwitchPlayerAnimation();
        }

        private void FixedUpdate()
        {
            if (!canNotInput)
            {
                Movement();//刚体移动一般放入FixedUpData
            }
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
        private void SwitchPlayerAnimation()
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

        private IEnumerator UseToolRoutine(Vector3 mouseWorldPos,Data.ItemDetails itemDetails)
        {
            canNotInput = true;
            yield return null;
            foreach (var playerAnimator in playerAnimators)
            {
                playerAnimator.SetTrigger("UseTool");
                playerAnimator.SetFloat("InputX",mouseX);
                playerAnimator.SetFloat("InputY",mouseY);
            }

            yield return new WaitForSeconds(0.45f);
            EventCenter.TriggerEvent(new ExecuteAfterAnimationEvent(mouseWorldPos,itemDetails));
            yield return new WaitForSeconds(0.2f);
            canNotInput = false;
        }

        #region EventCenter

        private void OnAfterSceneLoad(AfterSceneLoadEvent afterSceneLoadEvent)
        {
            canNotInput = false;
        }
        
        private void OnBeforeSceneUnLoad(BeforeSceneUnLoadEvent beforeSceneUnLoadEvent)
        {
            canNotInput = true;
        }
        
        /// <summary>
        /// 物品点击后播放相应动画
        /// </summary>
        /// <param name="playerAnimationBeforeExecuteEvent"></param>
        private void OnPlayerAnimationBeforeExecute(PlayerAnimationBeforeExecuteEvent playerAnimationBeforeExecuteEvent)
        {
            if (playerAnimationBeforeExecuteEvent.ItemDetails.ItemType != Enums.ItemType.Seed &&
                playerAnimationBeforeExecuteEvent.ItemDetails.ItemType != Enums.ItemType.Commodity &&
                playerAnimationBeforeExecuteEvent.ItemDetails.ItemType != Enums.ItemType.Furniture)
            {
                mouseX = playerAnimationBeforeExecuteEvent.MouseWorldPos.x - transform.position.x;
                mouseY = playerAnimationBeforeExecuteEvent.MouseWorldPos.y - transform.position.y;
                if (Mathf.Abs(mouseX) > Mathf.Abs(mouseY))
                {
                    mouseY = 0;
                }
                else
                {
                    mouseX = 0;
                }

                StartCoroutine(UseToolRoutine(playerAnimationBeforeExecuteEvent.MouseWorldPos,
                    playerAnimationBeforeExecuteEvent.ItemDetails));
            }
            else
            {
                EventCenter.TriggerEvent(new ExecuteAfterAnimationEvent(playerAnimationBeforeExecuteEvent.MouseWorldPos,
                    playerAnimationBeforeExecuteEvent.ItemDetails));
            }
        }

        #endregion

    }
}
