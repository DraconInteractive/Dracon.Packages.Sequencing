using System;
using System.Collections;
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
            MoveTransform,
            WaitDuration,
            TransferStage
        }
        
        [SerializeReference] public List<SequenceAction> actions = new List<SequenceAction>();

        public bool Running
        {
            get
            {
                foreach (var action in actions)
                {
                    if (action.running)
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
    [Serializable]
    public class SequenceAction
    {
        [HideInInspector]
        public string name;
        public bool running;

        public virtual void Begin()
        {
            running = true;
            Run();
        }

        public virtual void Run ()
        {
            Complete();
        }

        public virtual void Complete ()
        {
            running = false;
        }
    }

    [Serializable]
    public class DebugAction : SequenceAction
    {
        public string message;

        public DebugAction()
        {
            name = "Debug";
        }

        public override void Run()
        {
            Debug.Log(message);
            Complete();
        }
    }

    [Serializable]
    public class MoveTransformAction : SequenceAction
    {
        public Transform target;
        public Transform endPosition;
        public float duration;
        public bool instant;

        public MoveTransformAction()
        {
            name = "Move Transform";
        }

        public override void Run()
        {
            if (instant)
            {
                target.position = endPosition.position;
                Complete();
            }
            else
            {
                SequenceManager.All[0].StartCoroutine(Move());
            }
        }

        public IEnumerator Move ()
        {
            Vector3 start = target.position;
            for (float f = 0; f < 1; f += Time.deltaTime / duration)
            {
                target.position = Vector3.Lerp(start, endPosition.position, f);
                yield return null;
            }
            target.position = endPosition.position;
            Complete();
            yield break;
        }
    }

    [Serializable]
    public class WaitDurationAction : SequenceAction
    {
        public float duration;
        public WaitDurationAction()
        {
            name = "Wait Duration";
        }

        public override void Run()
        {
            SequenceManager.All[0].StartCoroutine(Wait());
        }

        public IEnumerator Wait()
        {
            yield return new WaitForSeconds(duration);
            Complete();
            yield break;
        }
    }

    [Serializable]
    public class TransferStageAction : SequenceAction
    {
        public SequenceManager oldStage;
        public SequenceManager newStage;

        public TransferStageAction ()
        {
            name = "Transfer Stage";
        }

        public override void Run()
        {
            oldStage.StopSequence();
            newStage.StartStage();
            Complete();
        }
    }
}
