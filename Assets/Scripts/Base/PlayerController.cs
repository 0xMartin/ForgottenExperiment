using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base {

    public class PlayerController : MonoBehaviour
    {

        [Header("Animator")]
        public GameObject handModel;
        public GameObject flashlightModel;
        public GameObject weaponModel;
        
        [Header("Physics")]
        public float moveSpeed;
        public float jumpForce;

        private Animator handAnimator;
        private Animator flashlightAnimator;
        private Animator weaponAnimator;
        
        public enum Equipment {
            Hand, FlashLight, Gun
        }

        private Equipment currentEquipment; // 0 - ruka, 1 - flash-light, 2 - zbran
        
        private bool isMoving;
        private bool isJumping;
        private bool isCrouching;
        
        private Rigidbody rb;
        
        private void Start()
        {
            handAnimator = handModel.GetComponent<Animator>();
            flashlightAnimator = flashlightModel.GetComponent<Animator>();
            weaponAnimator = weaponModel.GetComponent<Animator>();
            
            currentEquipment = Equipment.Hand;
            handAnimator.SetTrigger("Show");
            
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            // Pohyb
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            
            isMoving = (Mathf.Abs(horizontalInput) > 0 || Mathf.Abs(verticalInput) > 0);
            
            // Skákání
            if (Input.GetButtonDown("Jump") && !isJumping)
            {
                isJumping = true;
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            
            // Přikrčení
            isCrouching = Input.GetKey(KeyCode.LeftControl);
            
            // Střelba
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
            
            // Přepínání vybavení
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SwitchEquipment(Equipment.Hand);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchEquipment(Equipment.FlashLight);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SwitchEquipment(Equipment.Gun);
            }
            
            // Aktualizace animací
            UpdateAnimations();
        }

        private void FixedUpdate()
        {
            // Pohyb
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(horizontalInput, 0, verticalInput).normalized;
            rb.MovePosition(rb.position + transform.TransformDirection(movement) * moveSpeed * Time.fixedDeltaTime);
        }

        private void Shoot()
        {
            if (currentEquipment == Equipment.Gun) // Je vybrána zbraň
            {
                // Provádění střelby
                weaponAnimator.SetTrigger("Shoot");
            }
        }

        private void SwitchEquipment(Equipment equipment)
        {
            // Skrývání současného vybavení
            switch (currentEquipment)
            {
                case Equipment.Hand:
                    handAnimator.SetTrigger("Hide");
                    break;
                case Equipment.FlashLight:
                    flashlightAnimator.SetTrigger("Hide");
                    break;
                case Equipment.Gun:
                    weaponAnimator.SetTrigger("Hide");
                    break;
            }
            
            // Změna vybavení
            currentEquipment = equipment;
            
            // Zobrazování nového vybavení
            switch (currentEquipment)
            {
                case Equipment.Hand:
                    handAnimator.SetTrigger("Show");
                    break;
                case Equipment.FlashLight:
                    flashlightAnimator.SetTrigger("Show");
                    break;
                case Equipment.Gun:
                    weaponAnimator.SetTrigger("Show");
                    break;
            }
        }

        private void UpdateAnimations()
        {
            // Aktualizace animací ruky
            handAnimator.SetBool("IsMoving", isMoving);
            handAnimator.SetBool("IsJumping", isJumping);
            handAnimator.SetBool("IsCrouching", isCrouching);

            // Aktualizace animací baterky
            flashlightAnimator.SetBool("IsMoving", isMoving);
            flashlightAnimator.SetBool("IsJumping", isJumping);
            flashlightAnimator.SetBool("IsCrouching", isCrouching);

            // Aktualizace animací zbraně
            weaponAnimator.SetBool("IsMoving", isMoving);
            weaponAnimator.SetBool("IsJumping", isJumping);
            weaponAnimator.SetBool("IsCrouching", isCrouching);

            // Resetování skokového stavu
            isJumping = false;
        }

    }

}


