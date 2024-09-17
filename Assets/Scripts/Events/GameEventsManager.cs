using System;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }

//     public InputEvents inputEvents;
//     public PlayerEvents playerEvents;
    public QuestEvents questEvents;
    public FoodEvents foodEvents;
    // public DigEvents digEvents;
    public JoinPositionEvents joinPositionEvents;
    // public ProtectBabyPolarBearEvents protectBabyPolarBearEvents;
    public MiscEvents miscEvents;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
        }
        instance = this;

//         // initialize all events
//         inputEvents = new InputEvents();
//         playerEvents = new PlayerEvents();
        questEvents = new QuestEvents();
        foodEvents = new FoodEvents();
        // digEvents = new DigEvents();
        joinPositionEvents = new JoinPositionEvents();
        // protectBabyPolarBearEvents = new ProtectBabyPolarBearEvents();
        miscEvents = new MiscEvents();
    }
}