using System;
using System.Collections.Generic;
using UnityEngine;

namespace DI_Sequences
{
    public class Sequence : MonoBehaviour
    {
        public bool running;
        [SerializeField] public List<IAction> actions = new List<IAction>();
    }

    public interface IAction
    {
        void Begin();
        void Complete();
    }

    [Serializable]
    public class DebugAction : IAction
    {
        [HideInInspector]
        public string name;
        public bool running;

        public string message;

        public DebugAction()
        {
            name = "Debug Action";
        }

        public void Begin()
        {
            throw new NotImplementedException();
        }

        public void Complete()
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public class MoveTransformAction : IAction
    {
        [HideInInspector]
        public string name;
        public bool running;

        public Transform target;
        public Transform endPosition;

        public MoveTransformAction()
        {
            name = "Move Transform Action";
        }

        public void Begin()
        {
            throw new NotImplementedException();
        }

        public void Complete()
        {
            throw new NotImplementedException();
        }
    }
}
