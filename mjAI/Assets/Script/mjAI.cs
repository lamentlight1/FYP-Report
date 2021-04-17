using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;

public class mjAI : Agent
{
    public GameManagerScript gms;
    public int playerNumber;
    public int[] playerHand;

    public override void OnEpisodeBegin()
    {
        switch (GetComponent<BehaviorParameters>().TeamId) //assign playerNumber
        {
            case 0:
                playerNumber = 1;
                break;
            case 1:
                playerNumber = 2;
                break;
            case 2:
                playerNumber = 3;
                break;
            case 3:
                playerNumber = 4;
                break;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        playerHand = gms.ComputeHand(playerNumber);
        for (int i=0;i<34;i++)
        {
            sensor.AddObservation(playerHand[i]);
        }
        int[] playerExposed = gms.ComputeExposed(playerNumber);
        for (int i = 0; i < 34; i++)
        {
            sensor.AddObservation(playerExposed[i]);
        }
        int[] others = gms.ComputeOthers(playerNumber);
        for (int i = 0; i < 34; i++)
        {
            sensor.AddObservation(others[i]);
        }
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        //Take action
        switch (gms.gamePhase)
        {
            case 0: //discard phase
                gms.TakeAction(Mathf.FloorToInt(vectorAction[0]), playerNumber);
                break;
            case 1: //ponggang phase
                gms.TakeAction(Mathf.FloorToInt(vectorAction[1]), playerNumber);
                break;
            case 2: //chow phase
                gms.TakeAction(Mathf.FloorToInt(vectorAction[1]), playerNumber);
                break;
        }       
    }

    public override void CollectDiscreteActionMasks(DiscreteActionMasker actionMasker)
    {
        switch (gms.gamePhase)
        {
            case 0:                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
                List<int> zeroFields = new List<int>();
                for (int i = 0; i < 34; i++)
                {
                    if (playerHand[i] == 0)
                        zeroFields.Add(i);
                }
                zeroFields.ToArray();
                actionMasker.SetMask(0, zeroFields);
                Debug.Log("Action received: " + string.Join(", ", playerHand));
                break;
            case -1:
                break;
            default:
                List<int> nullChoices = new List<int>();
                GameObject overlayPanel = GameObject.Find("OverlayPanel(Clone)");

                if (overlayPanel)
                {
                    for (int i = overlayPanel.transform.childCount - 1; i < 4; i++)
                    {
                        nullChoices.Add(i);
                    }
                    nullChoices.ToArray();
                    actionMasker.SetMask(1, nullChoices);

                }                
                break;
        }
        
    }
}
