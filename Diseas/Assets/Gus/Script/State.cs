using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
  public abstract void OnEnterState();

  public abstract void OnExitState();

  public abstract void OnUpdateState();
}
