﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DI_Sequences
{
    public class SequenceManager : MonoBehaviour
    {
        public static List<SequenceManager> All = new List<SequenceManager>();

        public List<Sequence> sequences = new List<Sequence>();

        [Space(30)]
        public bool playOnAwake;
        public int currentIndex;
        Coroutine sequenceRoutine;

        void OnEnable ()
        {
            All.Add(this);
        }

        void OnDisable ()
        {
            All.Remove(this);
        }

        void Start ()
        {
            StartStage();
        }

        public void StartStage ()
        {
            StartSequence(0);
        }

        public void StartSequence (int index)
        {
            if (sequenceRoutine != null)
            {
                StopCoroutine(sequenceRoutine);
            }
            currentIndex = index;
            sequenceRoutine = StartCoroutine(RunSequence(sequences[currentIndex]));
        }

        public void StopSequence ()
        {
            if (sequenceRoutine != null)
            {
                StopCoroutine(sequenceRoutine);
            }
        }

        IEnumerator RunSequence (Sequence sequence)
        {
            foreach (var action in sequence.actions)
            {
                action.Begin();
            }

            while (sequence.Running)
            {
                yield return null;
            }
            yield break;
        }

    }
}
