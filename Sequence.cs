using System;
using System.Collections.Generic;
using UnityEngine;

namespace DI_Sequences
{
    [Serializable]
    public class Sequence
    {
        public enum ActionType
        {
            SelectType,
            Debug,
            MoveTransform
        }
        public bool running;
        [SerializeField] private List<IAction> actions = new List<IAction>();
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
