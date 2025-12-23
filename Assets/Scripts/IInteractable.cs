    using UnityEngine;

    public interface IInteractable
    {
        public Transform Transform { get; }
        public float InteractionDistance { get; }
    }
