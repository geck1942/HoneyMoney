    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Cinemachine;
    using UnityEngine;

    public class ScenarioStep : MonoBehaviour
    {
        public ScenarioState State;
        public List<GameObject> activateNoDelay;
        public float delay = 0;
        public List<GameObject> activateAfterDelay;
        public string message;
        public float messageDuration = 10f;
        public CinemachineVirtualCamera specialCamera = null;
        public float cameraDuration = 0f;
        public bool freezePlayer = false;
        public float EndAfterDelay = 0f;
        public float EndAfterLootedQuantity = 0;
        public string EndAfterLootedItemName = "";
        
        private Coroutine waitAndShowHelpCoroutine = null;
        
        void Start()
        {
            this.HideItems();
            
        }
        
        public virtual void Begin()
        {
            PlayerController.Instance.OnLoot += PlayerLoot;
            this.waitAndShowHelpCoroutine = StartCoroutine(this.ShowItems());
            Debug.Log(this.message);

            if (this.specialCamera != null)
                CameraController.Instance.SwitchToSpecialCamera(this.specialCamera, this.cameraDuration);
            
            if(this.freezePlayer)
                PlayerController.Instance.player.characterController.enabled = false;
        }


        public virtual void End()
        {
            PlayerController.Instance.OnLoot -= PlayerLoot;
            
            if(this.waitAndShowHelpCoroutine != null)
                StopCoroutine(this.waitAndShowHelpCoroutine);
            this.HideItems();

            if(this.freezePlayer)
                PlayerController.Instance.player.characterController.enabled = true;
        }

        
        private IEnumerator ShowItems()
        {
            foreach (var item in activateNoDelay)
                if(item != null)
                    item.SetActive(true);

            if (activateAfterDelay.Count > 0)
            {
                if(this.delay > 0)
                    yield return new WaitForSeconds(this.delay);
                
                foreach (var item in activateAfterDelay)
                    if(item != null)
                        item.SetActive(true);
            }
            
            if(this.EndAfterDelay > 0)
            {
                yield return new WaitForSeconds(this.EndAfterDelay - this.delay);
                ScenarioController.Instance.GoToNextStep();
            }
        }

        public void HideItems()
        {
            foreach (var item in activateNoDelay)
                if(item != null)
                    item.SetActive(false);
            foreach (var item in activateAfterDelay)
                if(item != null)
                    item.SetActive(false);
        }
        
        private void PlayerLoot(string resource, float quantity, IInteractable target)
        {
            if (!string.IsNullOrEmpty(EndAfterLootedItemName) && resource == EndAfterLootedItemName)
            {
                this.EndAfterLootedQuantity -= quantity;
                if(this.EndAfterLootedQuantity == 0)
                    ScenarioController.Instance.GoToNextStep();
            }
        }
    }
