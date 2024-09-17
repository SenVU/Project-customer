using System;
using UnityEngine;

public class MiscEvents
{
    public event Action onFoodColleted;
    public void FoodCollected()
    {
        if (onFoodColleted != null)
        {
            onFoodColleted();
        }
    }

    public event Action onDig;
    public void Dig()
    {
        if (onDig != null)
        {
            onDig();
        }
    }

    public event Action onPositionJoined;
    public void PositionJoined()
    {
        if (onPositionJoined != null)
        {
            onPositionJoined();
        }
    }
}
