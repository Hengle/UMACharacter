﻿/// Simple, really.  There is no need to initialize or even refer to TaskManager.
/// When the first Task is created in an application, a "TaskManager" GameObject
/// will automatically be added to the scene root with the TaskManager component
/// attached.  This component will be responsible for dispatching all coroutines
/// behind the scenes.
/// 协程管理类
/// Task also provides an event that is triggered when the coroutine exits.

using UnityEngine;
using System.Collections;

/// A Task object represents a coroutine.  Tasks can be started, paused, and stopped.
/// It is an error to attempt to start a task that has been stopped or which has
/// naturally terminated.
/// A Task object represents a coroutine.  Tasks can be started, paused, and stopped.
/// It is an error to attempt to start a task that has been stopped or which has
/// naturally terminated.
public class Task
{
    /// Creates a new Task object for the given coroutine.
    ///
    /// If autoStart is true (default) the task is automatically started
    /// upon construction.
    public Task(IEnumerator c, bool autoStart = true)
    {
        state = TaskServices.CreateTask(c);
        if (autoStart)
            Start();
    }

    /// Begins execution of the coroutine
    public void Start()
    {
        state.Start();
    }

    /// Discontinues execution of the coroutine at its next yield.
    public void Stop()
    {
        state.Stop();
    }

    TaskServices.TaskState state;

    public Coroutine coroutine
    {
        get
        {
            return state.coroutine;
        }
    }
}

public class TaskServices : MonoBehaviour
{
    public class TaskState
    {

        public Coroutine coroutine;

        IEnumerator ienumerator;
        bool running;

        public TaskState(IEnumerator c)
        {
            ienumerator = c;
        }

        public void Start()
        {
            coroutine = singleton.StartCoroutine(ienumerator);
        }

        public void Stop()
        {
            singleton.StopCoroutine(ienumerator);
        }
    }

    static TaskServices singleton;

    public static TaskState CreateTask(IEnumerator coroutine)
    {
        if (singleton == null)
        {
            GameObject go = new GameObject("TaskManager");
            DontDestroyOnLoad(go);
            singleton = go.AddComponent<TaskServices>();
        }
        return new TaskState(coroutine);
    }
}
